using System.Data.Common;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Top10Gappers_Test
{
    internal class Program
    {
        private const string HorizontalBar = "-----------------------------------------------------------";
        private const string PreMarketDataUrl = "https://api.polygon.io/v2/snapshot/locale/us/markets/stocks/gainers?apiKey=";
        private const string EodDataUrl = "https://api.polygon.io/v2/aggs/grouped/locale/us/market/stocks/";
        private static readonly HttpClient client = new HttpClient();
        private static int updateCount = 0;

        static async Task Main(string[] args)
        {
            Console.Clear();

            await ProgramLoop();
        }

        static string LoadKey()
        {
            string path = @"key.txt";

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Error: File was not found.", path);
            }
            else
            {
                return File.ReadAllText(path);
            }
        }

        static async Task ProgramLoop()
        {
            Console.WriteLine("Polygon.io Pre-Market Gappers Scanner (Top 10)\n");
            while (true)
            {
                try
                {
                    await FetchAndEODData();
                    //await FetchAndDisplayTop10Gappers();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }

                string displayUpdateCount = updateCount > 0 ? $" Updated {updateCount} times." : "..";
                Console.WriteLine($"Updating data in 60 seconds.{displayUpdateCount}");
                updateCount++;
                await Task.Delay(60000);
            }
        }

        static async Task FetchAndEODData()
        {
            // Get yesterday's date in YYYY-MM-DD format
            string date = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd");

            // Polygon Grouped Daily (EOD) endpoint
            string url = $"{EodDataUrl}{date}?adjusted=true&apiKey={LoadKey()}";

            var response = await client.GetStringAsync(url);
            var json = JObject.Parse(response);

            var tickers = json["results"]
                .Select(t => new
                {
                    Ticker = t["T"].ToString(),
                    Open = (decimal)t["o"],
                    Close = (decimal)t["c"],
                    Change = (decimal)t["c"] - (decimal)t["o"],
                    ChangePct = ((decimal)t["c"] - (decimal)t["o"]) / (decimal)t["o"]
                })
                .OrderByDescending(t => t.ChangePct)
                .Take(10)
                .ToList();

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Top 10 Gappers (EOD Test) for {date}:");
            Console.ResetColor();

            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine($"{"Symbol",-10}{"Close Price",-15}{"Change",-10}{"% Change"}");
            Console.WriteLine("-----------------------------------------------------------");

            foreach (var t in tickers)
            {
                // Symbol
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{t.Ticker,-10}");

                // Price
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{t.Close,-15:C}");

                // Raw Change
                Console.ForegroundColor = t.Change >= 0 ? ConsoleColor.Green : ConsoleColor.Red;
                Console.Write($"{(t.Change >= 0 ? "+" : "")}{t.Change,-10:0.00}");

                // % Change
                Console.ForegroundColor = t.ChangePct >= 0 ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine($"{t.ChangePct,0:P2}");

                Console.ResetColor();
            }
        }

        static async Task FetchAndDisplayTop10Gappers()
        {
            string url = $"{PreMarketDataUrl}{LoadKey()}";
            var responseData = await client.GetStringAsync(url);
            var json = JObject.Parse(responseData);
            var tickers = json["tickers"]
                .Where(t => t["preMarket"]?["changePercent"] != null)
                .Select(t => new
                {
                    Ticker = t["ticker"]?.ToString(),
                    Last = (decimal)t["lastTrade"]["p"],
                    PreMarketChange = (decimal)t["preMarket"]["change"],
                    PreMarketChangePercent = (decimal)t["preMarket"]["changePercent"]
                })
                .OrderByDescending(t => t.PreMarketChangePercent)
                .Take(10)
                .ToList();

            Console.Clear();
            Console.WriteLine("Top 10 Pre-Market Gappers:");
            Console.WriteLine(HorizontalBar);
            Console.WriteLine($"{"Symbol", -10}{"Last Price", -15}{"Change", -10}{"% Change"}");
            Console.WriteLine(HorizontalBar);

            foreach (var t in tickers)
            {
                Console.WriteLine($"{t.Ticker, -10}{t.Last, -15}{t.PreMarketChange, -10:+0.00;-0.00}{t.PreMarketChangePercent:P2}");
            }
        }
    }
}

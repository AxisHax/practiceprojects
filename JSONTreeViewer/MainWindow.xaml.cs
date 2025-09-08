namespace JSONTreeViewer
{
    using System.IO;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using Microsoft.Win32;


    public class JsonNode
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public List<JsonNode> Children { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadJson_Click(object sender, RoutedEventArgs e) 
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() is true)
            {
                string path = openFileDialog.FileName;
                Thread loadThread = new Thread(() => LoadAndDisplayJson(path))
                {
                    IsBackground = true,
                    Name = "JsonLoaderThread"
                };

                loadThread.SetApartmentState(ApartmentState.STA);
                loadThread.Start();
            }
        }

        private void LoadAndDisplayJson(string path)
        {
            try
            {
                // Read file asynchronously.
                string json = File.ReadAllText(path);

                // Parse JSON on a thread-pool thread.
                JsonDocument document = JsonDocument.Parse(json);

                //Build TreeView
                var rootItem = CreateTreeViewItem(document.RootElement, Path.GetFileName(path));

                // Marshal back to UI thread to update TreeView.
                Dispatcher.Invoke(() =>
                {
                    JSONTreeView.Items.Clear();
                    JSONTreeView.Items.Add(rootItem);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading JSON: {ex.Message}", "Loading error.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private TreeViewItem CreateTreeViewItem(JsonElement element, string header)
        {
            ArgumentException.ThrowIfNullOrEmpty(header, nameof(header));

            TreeViewItem item = new TreeViewItem() { Header = header };

            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (var property in element.EnumerateObject())
                    {
                        item.Items.Add(CreateTreeViewItem(property.Value, property.Name));
                    }
                    break;

                case JsonValueKind.Array:
                    int index = 0;
                    
                    foreach (var child in element.EnumerateArray())
                    {
                        item.Items.Add(CreateTreeViewItem(child, $"[{index}]"));
                        index++;
                    }
                    break;

                case JsonValueKind.String:
                    item.Header = $"{header}: \"{element.GetString()}\"";
                    break;

                case JsonValueKind.Number:
                    item.Header = $"{header}: \"{element.GetRawText()}\"";
                    break;

                case JsonValueKind.True:
                case JsonValueKind.False:
                    item.Header = $"{header}: {element.GetRawText()}";
                    break;

                case JsonValueKind.Null:
                    item.Header = $"{header}: null";
                    break;

                default:
                    item.Header = $"{header}: {element.GetRawText()}";
                    break;
            }

            return item;
        }
    }
}
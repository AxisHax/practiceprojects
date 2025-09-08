namespace LongestCommonPrefix
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(LongestCommonPrefix(["flower", "flow", "flight"]));
            //Console.WriteLine(IncrementChar('A'));
        }

        static char IncrementChar(char c)
        {
            return (char)(c + 1);
        }
        static string LongestCommonPrefix(string[] strs)
        {
            // Check your inputs.
            if (strs is null || strs.Length < 1)
            {
                return "";
            }

            if (strs.Length == 1)
            {
                return strs[0];
            }

            // Find the smallest string and use that to compare the prefixes of the other strings.
            string smallestPrefix = strs[0];
            int stringToSkip = 0;

            for (int i = 1; i < strs.Length; i++)
            {
                if (strs[i].Length < smallestPrefix.Length)
                {
                    // Set the new smallest string.
                    smallestPrefix = strs[i];
                    stringToSkip = i;
                }
            }

            // Compare with every other string.
            for (int i = 0; i < strs.Length; i++)
            {
                if ((i == stringToSkip) || string.Equals(strs[i], smallestPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                // Find the first unique character and remove all characters after it, making a new common prefix.
                for (int j = 0; j < Math.Min(strs[i].Length, smallestPrefix.Length); j++)
                {
                    if (strs[i][j] != smallestPrefix[j])
                    {
                        smallestPrefix = smallestPrefix.Remove(j);
                    }
                }
            }

            return smallestPrefix;
        }
    }

}

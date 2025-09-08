//	<copyright file="ReadTextFile.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for ReadTextFile.
//	</summary>
namespace Problem1
{
    /// <summary>
    /// Solution to Option 1: Read a text file and print each line to the console.
    /// </summary>
    internal class ReadTextFile
    {
        /// <summary>
        /// Reads a text file from the provided path and prints each line to the screen.
        /// </summary>
        /// <param name="filePath">Relative path to a text file.</param>
        public static void ReadFileAndOutputText(string filePath)
        {
            StreamReader stream = new StreamReader(filePath);
            string? line = stream.ReadLine();

            while (line != null)
            {
                Console.WriteLine(line);
                line = stream.ReadLine();
            }
        }

        /// <summary>
        /// Main function.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            string path = @"text.txt";
            ReadFileAndOutputText(path);
        }
    }
}

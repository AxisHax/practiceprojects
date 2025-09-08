//	<copyright file="PrintPrimeNumbers.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for PrintPrimeNumbers.
//	</summary>
namespace Problem2
{
    /// <summary>
    /// Solution for Option 2: Printing all prime numbers from 1-100.
    /// </summary>
    internal class PrintPrimeNumbers
    {
        /// <summary>
        /// Check if the number given is prime.
        /// </summary>
        /// <param name="n">An integer.</param>
        /// <returns>True if the number is prime, and false if it isn't.</returns>
        public static bool IsPrime(int n)
        {
            for (int i = 2; i < n; i++)
            {
                if ((n % i) == 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Main function.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            // Checking all prime numbers from 1-100.
            for (int i = 1; i <= 100; i++)
            {
                if(IsPrime(i))
                {
                    Console.WriteLine(i);
                }
            }
        }
    }
}

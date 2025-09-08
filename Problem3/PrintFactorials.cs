//	<copyright file="PrintFactorials.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for PrintFactorials.
//	</summary>
namespace Problem3
{
    using System.Numerics;

    /// <summary>
    /// Solution for Option 3: Printing factorials.
    /// </summary>
    internal class PrintFactorials
    {
        /// <summary>
        /// Calculate the factorial of the target.
        /// </summary>
        /// <param name="target">The factorial we are looking for.</param>
        /// <returns>Factorial of the number given.</returns>
        public static BigInteger Factorial(int target)
        {
            if (target <= 1)
            {
                return 1;
            }

            return target * Factorial(target - 1);
        }

        /// <summary>
        /// Main function.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        private static void Main(string[] args)
        {
            for (int i = 1; i <= 100; i++)
            {
                Console.WriteLine("{0}! = {1}", i, Factorial(i));
            }
        }
    }
}

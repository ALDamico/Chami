using System;

namespace WhereAmI
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentEnvironment = Environment.GetEnvironmentVariable("_CHAMI_ENV");
            if (string.IsNullOrEmpty(currentEnvironment))
            {
                currentEnvironment = Environment.GetEnvironmentVariable("USER");
            }

            if (string.IsNullOrEmpty(currentEnvironment))
            {
                Console.WriteLine("Unable to find the current environment!");
                Environment.Exit(-1);
            }

            Console.WriteLine(currentEnvironment);
            Environment.Exit(0);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEMESTRAL_PROJECT.UTILS
{
    /// <summary>
    /// Utility class handling formatting of standard output
    /// </summary>
    static class ConsoleFormat
    {
        /// <summary>
        /// Default text color of output to standard output
        /// </summary>
        private static const ConsoleColor DefaultForeground
            = ConsoleColor.Gray;

        /// <summary>
        /// Default background color of standard output
        /// </summary>
        private static const ConsoleColor DefaultBackground
            = ConsoleColor.Black;

        /// <summary>
        /// Color of text when printing error messages
        /// </summary>
        private static const ConsoleColor ErrorForeground
            = ConsoleColor.Red;

        /// <summary>
        /// Color of background when printing error messages
        /// </summary>
        private static const ConsoleColor ErrorBackground
            = ConsoleFormat.DefaultBackground;
        
        /// <summary>
        /// Sets default preset of standard output
        /// </summary>
        public static void Default()
        {
            Console.ForegroundColor = ConsoleFormat.DefaultForeground;
            Console.BackgroundColor = ConsoleFormat.DefaultBackground;
        }

        /// <summary>
        /// Sets actual format of standard output to errorneous preset
        /// </summary>
        public static void Error()
        {
            Console.ForegroundColor = ConsoleFormat.ErrorForeground;
            Console.BackgroundColor = ConsoleFormat.ErrorBackground;
        }

    }
}

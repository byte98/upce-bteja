using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HW06.Lexer;
using HW06.Parser;

namespace HW06
{
    /// <summary>
    /// Class representing interpreter of PL/0 language
    /// </summary>
    class Interpreter
    {
        /// <summary>
        /// Counter of printed messages
        /// </summary>
        private static int COUNTER = 0;

        /// <summary>
        /// Counter of printed error messages
        /// </summary>
        private static int ERR_COUNTER = 0;

        /// <summary>
        /// Limit for printed errors
        /// </summary>
        private static readonly int MAX_ERR = 8;

        /// <summary>
        /// Parser of program
        /// </summary>
        private Parser.Parser parser;

        /// <summary>
        /// Creates new interpreter of PL/0 program
        /// </summary>
        /// <param name="parser">Parser of PL/0 source</param>
        public Interpreter(Parser.Parser parser)
        {
            this.parser = parser;
        }

        /// <summary>
        /// Executes program
        /// </summary>
        public void Execute()
        {
            Parser.Nodes.ExecutableBlockNode exec = this.parser.GetExecutable();
            if (exec == null)
            {
                Interpreter.PrintError("Cannot execute program (nothing to execute)!", null);
            }
            else
            {
                exec.Execute();
            }
        }

        /// <summary>
        /// Prints message to console
        /// </summary>
        /// <param name="message">Message which will be printed to console</param>
        /// <param name="token">Token providing more information</param>
        public static void Print(string message, Token token)
        {
            if (Program.DEBUG)
            {
                Interpreter.COUNTER++;
                Console.WriteLine("(" + String.Format("{00:0#}", Interpreter.COUNTER) + ") " + message);
                if (token != null)
                {
                    Console.WriteLine("      " + token.ToString());
                }
            }
        }

        /// <summary>
        /// Prints error message to console
        /// </summary>
        /// <param name="message">Message which will be printed to console</param>
        /// <param name="token">Token which caused error</param>
        public static void PrintError(string message, Token token)
        {
            if (Interpreter.ERR_COUNTER < Interpreter.MAX_ERR)
            {
                Interpreter.ERR_COUNTER++;
                Interpreter.COUNTER++;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("(" + String.Format("{00:0#}", Interpreter.ERR_COUNTER) + ") " + message);
                if (token != null)
                {
                    string line = "     File " + token.GetFile() + " [line: " + token.GetRow() + "; column: " + token.GetColumn() + "]: ";
                    Console.WriteLine(line + token.GetLine());
                    for (int i = 0; i < line.Length; i++)
                    {
                        Console.Write(" ");
                    }
                    for (int i = 0; i < token.GetColumn() - 1; i++)
                    {
                        Console.Write(" ");
                    }
                    Console.Write("˄");
                    for (int i = 1; i < token.GetContent().Length; i++)
                    {
                        Console.Write("ˉ");
                    }
                    Console.WriteLine();
                }
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Maximum errors (" + Interpreter.MAX_ERR + ") printed! Program exits!");
                Console.ForegroundColor = ConsoleColor.Gray;
                Program.Exit();
            }

        }
    }
}

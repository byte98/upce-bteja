using System;
using System.IO;
using SEMESTRAL_PROJECT.UTILS;

namespace SEMESTRAL_PROJECT.LEXILYZER
{
    /// <summary>
    /// Class representing one token of source code
    /// </summary>
    class Token
    {
        /// <summary>
        /// Path to file with program
        /// </summary>
        private readonly String file;

        /// <summary>
        /// Line with code containing token
        /// </summary>
        private readonly String line;

        /// <summary>
        /// Number of column with token
        /// </summary>
        private readonly int column;

        /// <summary>
        /// Number of row containing token
        /// </summary>
        private readonly int row;

        /// <summary>
        /// Content of token
        /// </summary>
        private readonly string content;

        /// <summary>
        /// Creates new token
        /// </summary>
        /// <param name="content">Content of token</param>
        /// <param name="file">Path to file with source code</param>
        /// <param name="line">Line containing token</param>
        /// <param name="row">Number of row with token</param>
        /// <param name="column">Number of column with token</param>
        public Token(string content, String file, String line, int row, int column)
        {
            this.content = content;
            this.file = file;
            this.line = line;
            this.row = row;
            this.column = column;
        }

        /// <summary>
        /// Writes error message to standard output
        /// </summary>
        /// <param name="message">Content of message</param>
        public void WriteError(String message)
        {
            ConsoleFormat.Error();
            Console.WriteLine("Syntax error:");
            string prefix = Path.GetFileName(this.file)
                + " (L:" + this.row.ToString() + "; C:" + this.column.ToString() + ")";
            Console.Write(prefix);
            Console.Write(" ");
            Console.WriteLine(this.line);
            for (int i = 0; i < prefix.Length + 1 + this.column; i++)
            {
                Console.Write(" ");
            }
            Console.Write("^");
            for (int i = 0; i < this.content.Length; i++)
            {
                Console.Write("~");
            }
            Console.WriteLine("");
            Console.WriteLine(message);
            ConsoleFormat.Default();
        }

        /// <summary>
        /// Gets content of token
        /// </summary>
        /// <returns>Content of token</returns>
        public string GetContent()
        {
            return this.content;
        }
    }
}

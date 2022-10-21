using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEMESTRAL_PROJECT.LEXILYZER
{
    /// <summary>
    /// Class representing lexical analyzator of program
    /// </summary>
    class Lexilyzer
    {
        /// <summary>
        /// Results of lexical analyzer
        /// </summary>
        private Queue<Token> results;

        /// <summary>
        /// Path to file which is being analyzed
        /// </summary>
        private String file;

        /// <summary>
        /// Creates new lexical analyzator of program
        /// </summary>
        /// <param name="file">File which will be analyzed</param>
        /// <param name="language">Name of language which is in file</param>
        public Lexilyzer(String file, String language)
        {
            this.file = file;
            this.results = new Queue<Token>();
        }
    }
}

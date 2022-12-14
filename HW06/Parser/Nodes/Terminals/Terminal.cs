using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW04.Parser.Nodes.Terminals
{
    /// <summary>
    /// Class representing terminal state in AST
    /// </summary>
    abstract class Terminal
    {
        /// <summary>
        /// Name of terminal
        /// </summary>
        private string name;

        /// <summary>
        /// Value of terminal
        /// </summary>
        private double value;
    }
}

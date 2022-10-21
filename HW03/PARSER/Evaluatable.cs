using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW03.Parser
{
    /// <summary>
    /// Defines interface for evaluatable classes
    /// </summary>
    interface Evaluatable
    {
        /// <summary>
        /// Evaluates node in AST
        /// </summary>
        /// <returns>Value of node in AST</returns>
        double Evaluate();
    }
}

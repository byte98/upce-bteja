using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralProject.Parser.Nodes
{
    /// <summary>
    /// Interface abstracting all methods for executable nodes
    /// </summary>
    public interface IExecutableNode
    {
        /// <summary>
        /// Executes node
        /// </summary>
        void Execute();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralProject.Parser.Nodes
{
    /// <summary>
    /// Interface abstracting all nodes which holds some kind of a value
    /// </summary>
    public interface IValueNode: IExecutableNode
    {
        /// <summary>
        /// Checks, whether node has defined value
        /// </summary>
        /// <returns>TRUE if node has defined value, FALSE otherwise</returns>
        bool HasValue();

        /// <summary>
        /// Gets value of node
        /// </summary>
        /// <returns>Value of node</returns>
        Value GetValue();
    }
}

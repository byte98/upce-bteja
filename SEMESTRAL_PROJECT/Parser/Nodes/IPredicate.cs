using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralProject.Parser.Nodes
{
    /// <summary>
    /// Interface abstracting all nodes about which makes sense deciding,
    /// whether they are true or false
    /// </summary>
    public interface IPredicate: IExecutableNode
    {
        /// <summary>
        /// Checks, whether predicate is TRUE or not
        /// </summary>
        /// <returns>TRUE if predicate is TRUE, FALSE otherwise</returns>
        bool IsTrue();
    }
}

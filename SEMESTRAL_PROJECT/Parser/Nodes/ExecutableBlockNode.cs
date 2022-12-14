using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemestralProject.Lexer;
using SemestralProject.Parser.Nodes;

namespace SemestralProject.Parser.Nodes
{
    /// <summary>
    /// Class abstracting block node which is also executable
    /// </summary>
    public abstract class ExecutableBlockNode: BlockNode, IExecutableNode
    {
        /// <summary>
        /// Creates new executable block node
        /// </summary>
        /// <param name="tokens">Tokens from source code</param>
        /// <param name="token">Token representing node</param>
        /// <param name="parent">Parent block to which this block belongs to</param>
        public ExecutableBlockNode(Lexer.TokenStream tokens, Lexer.Token token, BlockNode block)
            : base(tokens, token, block) { }

        /// <summary>
        /// Executes node
        /// </summary>
        public abstract void Execute();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HW05.Lexer;

namespace HW05.Parser.Nodes
{
    /// <summary>
    /// Class representing executable node in AST
    /// </summary>
    abstract class ExecutableNode: Node
    {
        /// <summary>
        /// Block to which node belongs to
        /// </summary>
        protected BlockNode block;

        /// <summary>
        /// Creates new executable node in AST
        /// </summary>
        /// <param name="tokens">Stream of tokens from lexer</param>
        /// <param name="token">Token defining node in AST</param>
        /// <param name="block">Block to which node belongs to</param>
        public ExecutableNode(TokenStream tokens, Token token, BlockNode block): base(tokens, token)
        {
            this.block = block;
        }

        /// <summary>
        /// Executes node and stores result into value of node
        /// </summary>
        public abstract void Execute();
    }
}

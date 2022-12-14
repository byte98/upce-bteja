using HW05.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW05.Parser.Nodes
{
    /// <summary>
    /// Class representing block of code which is executable
    /// </summary>
    abstract class ExecutableBlockNode : BlockNode
    {
        /// <summary>
        /// Creates new block node which is executable
        /// </summary>
        /// <param name="tokens">Stream of tokens from lexer</param>
        /// <param name="token">Token defining node in AST</param>
        /// <param name="block">Parential block of code</param>
        public ExecutableBlockNode(TokenStream tokens, Token token, BlockNode block) : base(tokens, token, block)
        {
        }

        /// <summary>
        /// Executes node and stores result into value of node
        /// </summary>
        public abstract void Execute();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HW04.Lexer;

namespace HW04.Parser.Nodes
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
        }

        /// <summary>
        /// Executes node and stores result into value of node
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Executes node and stores result into value of node
        /// </summary>
        /// <param name="arg">Argument of node</param>
        //public abstract void Execute(double arg);

        /// <summary>
        /// Executes node and stores result into value of node
        /// </summary>
        /// <param name="arg1">First argument of node</param>
        /// <param name="arg2">Second argument of node</param>
        // public abstract void Execute(double arg1, double arg2);
    }
}

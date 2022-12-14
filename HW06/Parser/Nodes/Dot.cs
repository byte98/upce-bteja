using HW06.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW06.Parser.Nodes
{
    /// <summary>
    /// Class representing dot node in AST
    /// </summary>
    class Dot : Node
    {
        /// <summary>
        /// Creates new dot node in AST
        /// </summary>
        /// <param name="tokens">Stream of tokens from lexer</param>
        /// <param name="token">Token defining node in AST</param>
        public Dot(TokenStream tokens, Token token) : base(tokens, token)
        {
        }

        public override void Build()
        {
            // Node is in fact leaf in AST, so there is nothing more to build
        }
    }
}

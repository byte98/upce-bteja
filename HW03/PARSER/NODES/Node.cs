using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HW03.Lexer;

namespace HW03.Parser.Nodes
{
    /// <summary>
    /// Class representing node in AST
    /// </summary>
    abstract class Node
    {
        /// <summary>
        /// Tokens browser
        /// </summary>
        protected Lexer.Lexer.LexerResults tokens;

        /// <summary>
        /// Token which defines node in AST
        /// </summary>
        protected Lexer.Token token;

        /// <summary>
        /// Creates new node in AST
        /// </summary>
        /// <param name="tokens">Browser of tokens</param>
        /// <param name="token">Token which defines node in AST</param>
        public Node(Lexer.Lexer.LexerResults tokens, Lexer.Token token)
        {
            this.tokens = tokens;
            this.token = token;
        }

        /// <summary>
        /// Builds AST subtree
        /// </summary>
        public abstract void Build();
    }
}

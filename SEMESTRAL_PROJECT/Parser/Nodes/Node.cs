using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemestralProject.Lexer;

namespace SemestralProject.Parser.Nodes
{
    /// <summary>
    /// Class representing node in AST (or CST?)
    /// </summary>
    public abstract class Node
    {
        /// <summary>
        /// Counter of created nodes
        /// </summary>
        private static long COUNTER = 0;

        /// <summary>
        /// Identifier of node
        /// </summary>
        protected readonly long id;

        /// <summary>
        /// Tokens from source code
        /// </summary>
        protected readonly Lexer.TokenStream tokens;

        /// <summary>
        /// Token representing node
        /// </summary>
        protected readonly Lexer.Token token;

        /// <summary>
        /// Block to which this node belongs to
        /// </summary>
        protected readonly BlockNode block;

        /// <summary>
        /// Creates new node in AST
        /// </summary>
        /// <param name="tokens">Tokens from source code</param>
        /// <param name="token">Token representing node</param>
        /// <param name="block">Block to which thid node belongs to</param>
        public Node(Lexer.TokenStream tokens, Lexer.Token token, BlockNode block)
        {
            this.tokens = tokens;
            this.token = token;
            this.block = block;
            this.id = Node.COUNTER;
            Node.COUNTER++;
        }

        /// <summary>
        /// Prints message which informs about start of node building
        /// </summary>
        protected void StartBuild()
        {
            Parser.Print("Started building " + this.ToString() + " ...", this.token);
        }

        /// <summary>
        /// Prints message which informs about finish of node building
        /// </summary>
        protected void FinishBuild()
        {
            Parser.Print("Building " + this.ToString() + " finished", this.token);
        }

        /// <summary>
        /// Builds node
        /// </summary>
        public abstract void Build();

        /// <summary>
        /// Gets block to which this node belongs to
        /// </summary>
        /// <returns>Block to which this node belongs to</returns>
        public BlockNode GetBlock()
        {
            return this.block;
        }

        /// <summary>
        /// Prints unexepected end of program error message
        /// </summary>
        /// <param name="expected">Expected token</param>
        protected void PrintUnexpectedEndOfProgram(string expected)
        {
            this.PrintUnexpectedEndOfProgram(expected, this.token);
        }

        /// <summary>
        /// Prints unexepected end of program error message
        /// </summary>
        /// <param name="expected">Expected token</param>
        /// <param name="token">Token representing errorneous state</param>
        protected void PrintUnexpectedEndOfProgram(string expected, Lexer.Token token)
        {
            Parser.PrintError("Unexpected end of program during building " + this.ToString() + " (expected " + expected + ")!", token);
        }

        /// <summary>
        /// Prints unexpected token error message
        /// </summary>
        /// <param name="expected">Expected token</param>
        /// <param name="token">Unexpected token</param>
        protected void PrintUnexpectedToken(string expected, Lexer.Token token)
        {
            Parser.PrintError("Unexpected token during building " + this.ToString() + " (expected " + expected + ")!", token);
        }

        public override string ToString()
        {
            return this.GetType().Name + "#" + this.id;
        }

        public override bool Equals(object obj)
        {
            bool reti = false;
            if (obj.GetType().IsInstanceOfType(this))
            {
                Node other = (Node)obj;
                reti = other.id == this.id;
            }
            return reti;
        }
    }
}

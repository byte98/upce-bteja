using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HW05.Lexer;

namespace HW05.Parser.Nodes
{
    /// <summary>
    /// Class representing node in AST
    /// </summary>
    abstract class Node
    {
        /// <summary>
        /// Counter of created nodes
        /// </summary>
        private static long counter = 0;

        /// <summary>
        /// Identifier of node
        /// </summary>
        protected long id;

        /// <summary>
        /// Stream of tokens from lexer
        /// </summary>
        protected TokenStream tokens;

        /// <summary>
        /// Token defining node in AST
        /// </summary>
        protected Token token;

        /// <summary>
        /// Flag, whether node has defined value
        /// </summary>
        protected bool hasValue = false;

        /// <summary>
        /// Value of node
        /// </summary>
        protected double value = double.NaN;

        /// <summary>
        /// Flag, whether value can be edited
        /// </summary>
        protected bool valueEditable = false;

        /// <summary>
        /// Flag, whether node has name
        /// </summary>
        protected bool hasName = false;

        /// <summary>
        /// Name of node
        /// </summary>
        protected string name = "";

        /// <summary>
        /// Creates new node in AST
        /// </summary>
        /// <param name="tokens">Stream of tokens from lexer</param>
        /// <param name="token">Token defining node in AST</param>
        public Node(TokenStream tokens, Token token)
        {
            this.tokens = tokens;
            this.token = token;
            this.id = Node.counter;
            Node.counter++;
        }

        /// <summary>
        /// Builds subtree for node
        /// </summary>
        public abstract void Build();

        /// <summary>
        /// Checks, whether node has value
        /// </summary>
        /// <returns>TRUE if node has defined value, FALSE otherwise</returns>
        public bool HasValue()
        {
            return this.hasValue;
        }

        /// <summary>
        /// Gets value of node
        /// </summary>
        /// <returns>Value of node</returns>
        public double GetValue()
        {
            return this.value;
        }

        /// <summary>
        /// Sets value to node. If this is not possible, prints error message
        /// </summary>
        /// <param name="value">New value of node</param>
        public void SetValue(double value)
        {
            if (this.hasValue)
            {
                if (this.valueEditable)
                {
                    this.value = value;
                }
                else
                {
                    Parser.PrintError("Unauthorized attempt to set value to node with value editing forbidden!", this.token);
                }
            }
            else
            {
                Parser.PrintError("Unauthorized attempt to set value to node without value!", this.token);
            }
        }

        /// <summary>
        /// Flag, whether node has name
        /// </summary>
        /// <returns>TRUE if node has defined name, FALSE otherwise</returns>
        public bool HasName()
        {
            return this.hasName;
        }

        /// <summary>
        /// Gets name of node
        /// </summary>
        /// <returns>Name of node</returns>
        public string GetName()
        {
            return this.name;
        }
    }
}

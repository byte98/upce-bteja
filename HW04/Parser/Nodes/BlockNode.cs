using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HW04.Lexer;

namespace HW04.Parser.Nodes
{
    /// <summary>
    /// Class representing node containing some kind of block in AST
    /// </summary>
    abstract class BlockNode : Node
    {
        /// <summary>
        /// Block of code hold by node
        /// </summary>
        protected CodeBlock codeBlock;

        /// <summary>
        /// Parential node
        /// </summary>
        protected BlockNode parent;

        /// <summary>
        /// Creates new node containing block of code
        /// </summary>
        /// <param name="tokens">Stream of tokens from lexer</param>
        /// <param name="token">Token defining node in AST</param>
        /// <param name="block">Parential block of code</param>
        public BlockNode(TokenStream tokens, Token token, BlockNode block) : base(tokens, token)
        {
            if (block != null)
            {
                this.parent = block;
                this.codeBlock = new CodeBlock(block.codeBlock);
            }
        }

        /// <summary>
        /// Checks, whether block of code stores variable
        /// </summary>
        /// <param name="name">Name of variable which will be checked</param>
        /// <returns>TRUE if block of code stores variable, FALSE otherwise</returns>
        public bool HasVariable(string name)
        {
            bool reti = false;
            if (this.codeBlock != null)
            {
                reti = this.codeBlock.HasVariable(name);
            }
            if (this.parent != null && reti == false)
            {
                reti = this.parent.HasVariable(name);
            }
            return reti;
        }

        /// <summary>
        /// Sets value to variable
        /// </summary>
        /// <param name="name">Name of variable</param>
        /// <param name="value">Value of variable</param>
        public void SetVariable(string name, double value)
        {
            if (this.codeBlock != null)
            {
                this.codeBlock.SetVariable(name, value);
            }
            if (this.parent != null)
            {
                this.parent.SetVariable(name, value);
            }
        }

        /// <summary>
        /// Gets value of variable
        /// </summary>
        /// <param name="name">Name of variable</param>
        /// <returns>Value of variable or <c>double.NaN</c> if there is no such a variable</returns>
        public double GetVariable(string name)
        {
            double reti = double.NaN;
            if (this.codeBlock != null)
            {
                reti = this.codeBlock.GetVariable(name);
            }
            if (this.parent != null && reti == double.NaN)
            {
                reti = this.parent.GetVariable(name);
            }
            return reti;
        }

        /// <summary>
        /// Checks, whether there is constant with defined name
        /// </summary>
        /// <param name="name">Name of constant</param>
        /// <returns>TRUE, if there is constant with defined name, FALSE otherwise</returns>
        public bool HasConstant(string name)
        {
            bool reti = false;
            if (this.codeBlock != null)
            {
                reti = this.codeBlock.HasConstant(name);
            }
            if (this.parent != null && reti == false)
            {
                reti = this.parent.HasConstant(name);
            }
            return reti;
        }

        /// <summary>
        /// Adds constant to code block
        /// </summary>
        /// <param name="name">Name of constant</param>
        /// <param name="value">Value of constant</param>
        /// <returns>TRUE, if this operation is allowed, FALSE otherwise</returns>
        public bool AddConstant(string name, double value)
        {
            bool reti = false;
            if (this.codeBlock != null)
            {
                reti = this.codeBlock.AddConstant(name, value);
            }
            if (this.parent != null && reti == false)
            {
                reti = this.parent.AddConstant(name, value);
            }
            return reti;
        }

        /// <summary>
        /// Gets value of constant
        /// </summary>
        /// <param name="name">Name of constant</param>
        /// <returns>Value of variable or <c>double.NaN</c> if there is no such a constant</returns>
        public double GetConstant(string name)
        {
            double reti = double.NaN;
            if (this.codeBlock != null)
            {
                reti = this.codeBlock.GetConstant(name);
            }
            if (this.parent != null && reti == double.NaN)
            {
                reti = this.parent.GetConstant(name);
            }
            return reti;
        }

        /// <summary>
        /// Checks, whether there is procedure with defined name
        /// </summary>
        /// <param name="name">Name of procedure which will be checked</param>
        /// <returns>TRUE if there is procedure with defined name, FALSE otherwise</returns>
        public bool HasProcedure(string name)
        {
            bool reti = false;
            if (this.codeBlock != null)
            {
                reti = this.codeBlock.HasProcedure(name);
            }
            if (this.parent != null && reti == false)
            {
                reti = this.parent.HasProcedure(name);
            }
            return reti;
        }

        /// <summary>
        /// Adds procedure to code block
        /// </summary>
        /// <param name="name">Name of procedure</param>
        /// <param name="procedure">Procedure which will be added to code block</param>
        /// <returns>TRUE if this operation is allowed, FALSE otherwise</returns>
        public bool AddProcedure(string name, ExecutableNode procedure)
        {
            bool reti = false;
            if (this.codeBlock != null)
            {
                reti = this.codeBlock.AddProcedure(name, procedure);
            }
            if (this.parent != null && reti == false)
            {
                this.parent.AddProcedure(name, procedure);
            }
            return reti;
        }

        /// <summary>
        /// Gets procedure with defined name
        /// </summary>
        /// <param name="name">Name defining procedure</param>
        /// <returns>Procedure or NULL if there is no such a procedure</returns>
        public ExecutableNode GetProcedure(string name)
        {
            ExecutableNode reti = null;
            if (this.codeBlock != null)
            {
                reti = this.codeBlock.GetProcedure(name);
            }
            if (this.parent != null && reti == null)
            {
                reti = this.parent.GetProcedure(name);
            }
            return reti;
        }
    }
}

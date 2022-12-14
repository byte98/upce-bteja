using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SemestralProject.Lexer;

namespace SemestralProject.Parser.Nodes
{
    /// <summary>
    /// Class representing block of code
    /// </summary>
    public abstract class BlockNode: Node
    {
        /// <summary>
        /// Context holding actual state of program
        /// </summary>
        protected Context context;

        /// <summary>
        /// Stack with previous states of program
        /// </summary>
        protected Stack<Context> contextes;

        /// <summary>
        /// Flag, whether block should be executing or not (handles call of break)
        /// </summary>
        protected bool executing = true;

        /// <summary>
        /// Creates new block node
        /// </summary>
        /// <param name="tokens">Tokens from source code</param>
        /// <param name="token">Token representing node</param>
        /// <param name="parent">Parent block to which this block belongs to</param>
        public BlockNode(Lexer.TokenStream tokens, Lexer.Token token, BlockNode parent)
            : base(tokens, token, parent)
        {
            this.context = new Context();
            this.contextes = new Stack<Context>();
        }

        /// <summary>
        /// Checks, whether there is available variable with defined name
        /// </summary>
        /// <param name="name">Name of variable</param>
        /// <returns>TRUE, if there is variable with defined name, FALSE otherwise</returns>
        public bool HasVariable(string name)
        {
            bool reti = this.context.HasVariable(name);
            if (reti == false && this.block != null)
            {
                reti = this.block.HasVariable(name);
            }
            return reti;
        }

        /// <summary>
        /// Gets variable defined by its name
        /// </summary>
        /// <param name="name">Name of variable</param>
        /// <returns>Variable defined by its name or NULL, if there is no variable with defined name</returns>
        public VariableModel GetVariable(string name)
        {
            VariableModel reti = this.context.GetVariable(name);
            if (reti == null && this.block != null)
            {
                reti = this.block.GetVariable(name);
            }
            return reti;
        }

        /// <summary>
        /// Adds variable if there is no variable with same name
        /// </summary>
        /// <param name="variable">Variable which will be added</param>
        public void AddVariable(VariableModel variable)
        {
            this.context.AddVariable(variable);
        }

        /// <summary>
        /// Checks, whether there is function with defined name
        /// </summary>
        /// <param name="name">Name of function which will be checked</param>
        /// <returns>TRUE, if there is function with defined name, FALSE otherwise</returns>
        public bool HasFunction(string name)
        {
            bool reti = this.context.HasFunction(name);
            if (reti == false && this.block != null)
            {
                reti = this.block.HasFunction(name);
            }
            return reti;
        }

        /// <summary>
        /// Gets function by its name
        /// </summary>
        /// <param name="name">Name of function</param>
        /// <returns>Function defined by its name or NULL if there is no such function</returns>
        public FunctionModel GetFunction(string name)
        {
            FunctionModel reti = this.context.GetFunction(name);
            if (reti == null && this.block != null)
            {
                reti = this.block.GetFunction(name);
            }
            return reti;
        }

        /// <summary>
        /// Adds function if there is no function with same name
        /// </summary>
        /// <param name="function">Function which will be added</param>
        public void AddFunction(FunctionModel function)
        {
            this.context.AddFunction(function);
        }

        /// <summary>
        /// Breaks execution of block
        /// </summary>
        public virtual void Break()
        {
            this.executing = false;
            if (this.block != null)
            {
                this.block.Break();
            }
        }

        /// <summary>
        /// Resets function
        /// </summary>
        public void Reset()
        {
            this.context.Reset();
        }

        /// <summary>
        /// Creates new state of program
        /// </summary>
        public virtual void CreateState()
        {
            /*
            Context newState = this.context.CreateCopy();
            this.contextes.Push(this.context);
            this.context = newState;
            */
            IList<string> varnames = this.context.GetVariables();
            foreach (string varname in varnames)
            {
                this.context.GetVariable(varname).NewEvaluation();
            }
        }

        /// <summary>
        /// Creates new empty state
        /// </summary>
        public void CreateEmptyState()
        {
            Context newState = new Context();
            this.contextes.Push(this.context);
            this.context = newState;
        }

        /// <summary>
        /// Changes actual state of program to previous one
        /// </summary>
        public virtual void PreviousState()
        {
            /*
            if (this.contextes.Count > 0)
            {
                this.context = this.contextes.Pop();
            }
            */
            IList<string> varnames = this.context.GetVariables();
            foreach (string varname in varnames)
            {
                this.context.GetVariable(varname).PreviousEvaluation();
            }
        }

        /// <summary>
        /// Checks, whether there is set flag
        /// </summary>
        /// <param name="flag">Flag which will be checked</param>
        /// <returns>TRUE if flag is set, FALSE otherwise</returns>
        public bool HasFlag(Register.EFlags flag)
        {
            bool reti = this.context.RegRead(flag);
            if (reti == false && this.block != null)
            {
                reti = this.block.HasFlag(flag);
            }
            return reti;
        }

        /// <summary>
        /// Sets flag
        /// </summary>
        /// <param name="flag">Flag which will be set</param>
        public void SetFlag(Register.EFlags flag)
        {
            this.context.RegSet(flag);
        }

        /// <summary>
        /// Unsets flag
        /// </summary>
        /// <param name="flag">Flag which will be unset</param>
        public void UnsetFlag(Register.EFlags flag)
        {
            this.context.RegUnset(flag);
        }

        /// <summary>
        /// Gets all available variables
        /// </summary>
        /// <returns>List with names of all available variables</returns>
        public IList<string> GetVariables()
        {
            return this.context.GetVariables();
        }
    }
}

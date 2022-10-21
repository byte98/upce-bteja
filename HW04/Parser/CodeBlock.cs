using HW04.Parser.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW04.Parser
{
    /// <summary>
    /// Class representing block of code
    /// </summary>
    class CodeBlock
    {
        /// <summary>
        /// All stored variables
        /// </summary>
        private Dictionary<string, double> variables;

        /// <summary>
        /// All stored constants
        /// </summary>
        private Dictionary<string, double> constants;

        /// <summary>
        /// All stored procedures
        /// </summary>
        private Dictionary<string, ExecutableNode> procedures;

        /// <summary>
        /// Parential block of code
        /// </summary>
        private CodeBlock parent;

        /// <summary>
        /// Creates new block of code
        /// </summary>
        /// <param name="parent">Parential block of code</param>
        public CodeBlock(CodeBlock parent)
        {
            this.parent = parent;
            this.variables = new Dictionary<string, double>();
            this.constants = new Dictionary<string, double>();
            this.procedures = new Dictionary<string, ExecutableNode>();
        }

        /// <summary>
        /// Checks, whether block of code stores variable
        /// </summary>
        /// <param name="name">Name of variable which will be checked</param>
        /// <returns>TRUE if block of code stores variable, FALSE otherwise</returns>
        public bool HasVariable(string name)
        {
            bool reti = false;
            if (this.variables.ContainsKey(name))
            {
                reti = true;
            }
            else if (this.parent != null)
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
            if (this.variables.ContainsKey(name))
            {
                this.variables.Remove(name);
            }
            this.variables.Add(name, value);
        }

        /// <summary>
        /// Gets value of variable
        /// </summary>
        /// <param name="name">Name of variable</param>
        /// <returns>Value of variable or <c>double.NaN</c> if there is no such a variable</returns>
        public double GetVariable(string name)
        {
            double reti = double.NaN;
            if (this.variables.ContainsKey(name))
            {
                reti = this.variables[name];
            }
            else if (this.parent != null)
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
            if (this.constants.ContainsKey(name))
            {
                reti = true;
            }
            else if (this.parent != null)
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
            bool reti = this.HasConstant(name) == false;
            if (reti)
            {
                this.constants.Add(name, value);
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
            if (this.constants.ContainsKey(name))
            {
                reti = this.constants[name];
            }
            else if (this.parent != null)
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
            if (this.procedures.ContainsKey(name))
            {
                reti = true;
            }
            else if (this.parent != null)
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
            bool reti = this.procedures.ContainsKey(name) == false;
            if (reti)
            {
                this.procedures.Add(name, procedure);
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
            if (this.procedures.ContainsKey(name))
            {
                reti = this.procedures[name];
            }
            else if (this.parent != null)
            {
                reti = this.parent.GetProcedure(name);
            }
            return reti;
        }

        /// <summary>
        /// Gets parent of code block
        /// </summary>
        /// <returns>Parential code block</returns>
        public CodeBlock GetParent()
        {
            return this.parent;
        }

    }
}

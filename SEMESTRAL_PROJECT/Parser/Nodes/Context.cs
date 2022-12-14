using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralProject.Parser.Nodes
{
    /// <summary>
    /// Class representing programming context
    /// holding all informations about variables and functions
    /// </summary>
    public class Context
    {
        /// <summary>
        /// Flag, whether context should be case sensitive or not
        /// </summary>
        private const bool CASE_SENSITIVE = false;

        /// <summary>
        /// List with all variables available in the context
        /// </summary>
        private readonly IList<VariableModel> variables;

        /// <summary>
        /// List with all functions available in the context
        /// </summary>
        private readonly IList<FunctionModel> functions;

        /// <summary>
        /// Register with flags which applies to current context
        /// </summary>
        private readonly Register register;

        /// <summary>
        /// Creates new programming context holding variables and functions
        /// </summary>
        public Context()
        {
            this.variables = new List<VariableModel>();
            this.functions = new List<FunctionModel>();
            this.register = new Register();
        }

        /// <summary>
        /// Formats string according to defined rules
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string FormatString(string input)
        {
            string reti = input;
            reti = reti.Trim();
            if (Context.CASE_SENSITIVE == false)
            {
                reti = reti.ToUpper();
            }
            return reti;
        }

        /// <summary>
        /// Checks, whether context contains variable
        /// </summary>
        /// <param name="name">Name of variable which will be searched</param>
        /// <returns>TRUE if context contains variable, FALSE otherwise</returns>
        public bool HasVariable(string name)
        {
            bool reti = false;
            foreach (VariableModel var in this.variables)
            {
                if (this.FormatString(var.GetName()) == this.FormatString(name))
                {
                    reti = true;
                    break;
                }
            }
            return reti;
        }

        /// <summary>
        /// Gets variable by its name
        /// </summary>
        /// <param name="name">Name of variable</param>
        /// <returns>Variable defined by its name or NULL, if there is no such a variable</returns>
        public VariableModel GetVariable(string name)
        {
            VariableModel reti = null;
            foreach (VariableModel var in this.variables)
            {
                if (this.FormatString(var.GetName()) == this.FormatString(name))
                {
                    reti = var;
                    break;
                }
            }
            return reti;
        }

        /// <summary>
        /// Adds variable to context, if there is no variable with same name
        /// </summary>
        /// <param name="var">Variable which will be added to context</param>
        public void AddVariable(VariableModel var)
        {
            if (this.HasVariable(var.GetName()) == false)
            {
                this.variables.Add(var);
            }
        }

        /// <summary>
        /// Checks, whether context contains function
        /// </summary>
        /// <param name="name">Name of function which will be searched</param>
        /// <returns>TRUE if context contains function with defined name, FALSE otherwise</returns>
        public bool HasFunction(string name)
        {
            bool reti = false;
            foreach(FunctionModel func in this.functions)
            {
                if (this.FormatString(func.GetName()) == this.FormatString(name))
                {
                    reti = true;
                    break;
                }
            }
            return reti;
        }

        /// <summary>
        /// Gets function by its name
        /// </summary>
        /// <param name="name">Name of function</param>
        /// <returns>Function defined by its name or NULL, if there is no such a function</returns>
        public FunctionModel GetFunction(string name)
        {
            FunctionModel reti = null;
            foreach (FunctionModel func in this.functions)
            {
                if (this.FormatString(func.GetName()) == this.FormatString(name))
                {
                    reti = func;
                    break;
                }
            }
            return reti;
        }

        /// <summary>
        /// Adds function to context, if there is no function with same name
        /// </summary>
        /// <param name="function">Function which will be added to the context</param>
        public void AddFunction(FunctionModel function)
        {
            if (this.HasFunction(function.GetName()) == false)
            {
                this.functions.Add(function);
            }
        }

        /// <summary>
        /// Resets all evaluated variables
        /// </summary>
        public void Reset()
        {
            foreach (VariableModel var in this.variables)
            {
                var.Unevaluate();
            }
        }

        /// <summary>
        /// Sets flag in register
        /// </summary>
        /// <param name="flag">Flag which will be set</param>
        public void RegSet(Register.EFlags flag)
        {
            this.register.Set(flag);
        }

        /// <summary>
        /// Unsets flag in register
        /// </summary>
        /// <param name="flag">Flag which will be unset</param>
        public void RegUnset(Register.EFlags flag)
        {
            this.register.Unset(flag);
        }

        /// <summary>
        /// Reads flag from register
        /// </summary>
        /// <param name="flag">Flag which value will be read</param>
        /// <returns>TRUE if flag is set in register, FALSE otherwise</returns>
        public bool RegRead(Register.EFlags flag)
        {
            return this.register.Get(flag);
        }

        /// <summary>
        /// Creates copy of this context
        /// </summary>
        /// <returns>Copy of this context without any variable evaluated</returns>
        public Context CreateCopy()
        {
            Context reti = new Context();
            foreach (VariableModel var in this.variables)
            {
                VariableModel newVar = new VariableModel(var.GetName(), var.GetDataType());
                newVar.SetValue(var.GetExecutable());
                if (var.IsEvaluated())
                {
                    newVar.SetEvaluation(var.GetEvaluation());
                }
                //newVar.Unevaluate();
                reti.AddVariable(newVar);
            }
            foreach (FunctionModel func in this.functions)
            {
                reti.AddFunction(new FunctionModel(func.GetName(), func.GetExecutable(), func.GetBlock()));
            }
            return reti;
        }

        /// <summary>
        /// Gets name of all available variables
        /// </summary>
        /// <returns>List with names of all available variables</returns>
        public IList<string> GetVariables()
        {
            IList<string> reti = new List<string>();
            foreach(VariableModel var in this.variables)
            {
                reti.Add(var.GetName());
            }
            return reti;
        }
    }
}

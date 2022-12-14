using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SemestralProject.Parser.Nodes;
using SemestralProject.Parser.Nodes.Blocks;

namespace SemestralProject.Parser
{
    /// <summary>
    /// Model for function in source code
    /// </summary>
    public class FunctionModel
    {
        /// <summary>
        /// Counter of created function models
        /// </summary>
        private static long counter = 0;

        /// <summary>
        /// Name of function
        /// </summary>
        private readonly string name;

        /// <summary>
        /// List with arguments of function
        /// </summary>
        private readonly IList<VariableModel> arguments;

        /// <summary>
        /// Node representing body of function
        /// </summary>
        private readonly FunctionBody body;

        /// <summary>
        /// Placeholder for return value
        /// </summary>
        private Value reti;

        /// <summary>
        /// Node representing function itself
        /// </summary>
        private readonly Function function;

        /// <summary>
        /// Identifier of model of function
        /// </summary>
        private readonly long id;

        /// <summary>
        /// Creates new model of function
        /// </summary>
        /// <param name="name">Name of function</param>
        /// <param name="body">Executable node representing body of function</param>
        /// <param name="function">Node representing function itself</param>
        public FunctionModel(string name, FunctionBody body, Function function)
        {
            this.name = name;
            this.body = body;
            this.arguments = new List<VariableModel>();
            this.function = function;
            this.reti = null;
            this.id = FunctionModel.counter;
            FunctionModel.counter++;
        }

        /// <summary>
        /// Sets return value of function
        /// </summary>
        /// <param name="var">Return value of function</param>
        public void SetReturn(Value val)
        {
            this.reti = val;
        }

        /// <summary>
        /// Checks, whether function has return value
        /// </summary>
        /// <returns>TRUE if function has return value, FALSE otherwise</returns>
        public bool HasReturn()
        {
            return !(this.reti == null);
        }

        /// <summary>
        /// Gets return value of function
        /// </summary>
        /// <returns>Return value of function</returns>
        public Value GetReturn()
        {
            return this.reti;
        }

        /// <summary>
        /// Gets name of function
        /// </summary>
        /// <returns>Name of function</returns>
        public string GetName()
        {
            return this.name;
        }

        /// <summary>
        /// Gets body of function
        /// </summary>
        /// <returns>Executable node representing body of function</returns>
        public FunctionBody GetExecutable()
        {
            return this.body;
        }

        /// <summary>
        /// Gets count of arguments
        /// </summary>
        /// <returns>Count of arguments</returns>
        public int GetArguments()
        {
            return this.arguments.Count;
        }

        /// <summary>
        /// Gets argument defined by its position
        /// </summary>
        /// <param name="index">Position of argument</param>
        /// <returns>Argument defined by its position or NULL, if position is not valid</returns>
        public VariableModel GetArgument(int index)
        {
            VariableModel reti = null;
            if (index >= 0 && index < this.GetArguments())
            {
                reti = this.arguments.ElementAt(index);
            }
            return reti;
        }

        /// <summary>
        /// Adds argument to function
        /// </summary>
        /// <param name="argument">Argument which will be added</param>
        public void AddArgument(VariableModel argument)
        {
            this.arguments.Add(argument);
        }

        /// <summary>
        /// Initializes function before execution
        /// </summary>
        public void Initialize()
        {
            foreach(VariableModel argument in this.arguments)
            {
                //argument.Unevaluate();
                this.function.AddVariable(argument);
            }
        }

        /// <summary>
        /// Gets block representing function
        /// </summary>
        /// <returns>Block representing function</returns>
        public Function GetBlock()
        {
            return this.function;
        }

        public override string ToString()
        {
            return "FUNCTION#" + this.id + ":" + this.name + "(" + this.arguments.Count + ")";
        }
    }
}

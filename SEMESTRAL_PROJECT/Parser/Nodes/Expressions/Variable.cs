using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemestralProject.Lexer;
using SemestralProject.Interpreter;

namespace SemestralProject.Parser.Nodes.Expressions
{
    /// <summary>
    /// Class representing node used to reference some variable
    /// </summary>
    class Variable: Node, IValueNode
    {
        /// <summary>
        /// Name of variable
        /// </summary>
        private string variableName;

        /// <summary>
        /// Value of variable
        /// </summary>
        private Value value;

        /// <summary>
        /// Creates new reference to variable
        /// </summary>
        /// <param name="tokens">Tokens from source code</param>
        /// <param name="token">Token representing node</param>
        /// <param name="parent">Parent block to which this block belongs to</param>
        public Variable(TokenStream tokens, Token token, BlockNode block)
            : base(tokens, token, block) { }

        public override void Build()
        {
            this.StartBuild();
            if (this.token.GetTokenType() == TokenType.VARIABLE)
            {
                if (this.token.HasValue())
                {
                    this.variableName = this.token.GetValue();
                }
                else
                {
                    Parser.PrintError("Building " + this.ToString() + " failed: token with no value passed!", this.token);
                }
            }
            else
            {
                this.PrintUnexpectedToken("identifier of variable", this.token);
            }
            this.FinishBuild();
        }

        public void Execute()
        {
            if (this.block.HasFlag(Register.EFlags.FARG) == false)
            {
                if (this.block.HasVariable(this.variableName))
                {
                    VariableModel var = this.block.GetVariable(this.variableName);
                    if (var.IsEvaluated() == false)
                    {
                        var.Evaluate();
                    }
                    if (var.HasValue())
                    {
                        if (var.GetEvaluation() != null)
                        {
                            if (var.GetEvaluation().GetDataType() == EDataType.INTEGER && var.GetDataType() == EDataType.FLOAT)
                            {
                                this.value = new Value((float)var.GetEvaluation().GetInt());
                            }
                            else
                            {
                                this.value = var.GetEvaluation();
                            }
                        }
                        else
                        {
                            Interpreter.Interpreter.PrintError("Value of variable '" + this.variableName + "' has no definition!", this.token);
                        }
                    }
                    else
                    {
                        Interpreter.Interpreter.PrintError("Variable '" + this.variableName + "' has no defined value!", this.token);
                    }
                }
                else
                {
                    Interpreter.Interpreter.PrintError("Unknown variable '" + this.variableName + "'!", this.token);
                }
            }
            else
            {
                if (this.block.HasVariable(this.variableName))
                {
                    VariableModel var = this.block.GetVariable(this.variableName);
                    if (this.value != null)
                    {
                        var.SetEvaluation(this.value);
                    }
                    else
                    {
                        Interpreter.Interpreter.PrintError("Variable (argument of function) '" + this.variableName + "' has no defined value!", this.token);
                    }
                }
                else
                {
                    Interpreter.Interpreter.PrintError("Unknown variable '" + this.variableName + "'!", this.token);
                }
            }
        }

        public Value GetValue()
        {
            return this.value;
        }

        public bool HasValue()
        {
            return this.value != null;
        }
    }
}

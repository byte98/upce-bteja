using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralProject.Parser.Nodes.Commands
{
    /// <summary>
    /// Class representing decrementation of variable
    /// </summary>
    class Decrementation: Node, IExecutableNode
    {
        /// <summary>
        /// Name of variable which will be decrementation
        /// </summary>
        private readonly string variable;

        /// <summary>
        /// Creates new decrementation of variable
        /// </summary>
        /// <param name="tokens">Tokens from source code</param>
        /// <param name="token">Token representing node</param>
        /// <param name="block">Block of code to which this node belongs to</param>
        /// <param name="variable">Name of variable which will be decremented</param>
        public Decrementation(Lexer.TokenStream tokens, Lexer.Token token, BlockNode block, string variable)
            : base(tokens, token, block)
        {
            this.variable = variable;
        }

        public override void Build()
        {
            this.StartBuild();
            if (this.token.GetTokenType() != Lexer.TokenType.DECREMENT)
            {
                this.PrintUnexpectedToken("decrementation", this.token);
            }
            this.FinishBuild();
        }

        public void Execute()
        {
            if (this.block.HasVariable(this.variable))
            {
                VariableModel var = this.block.GetVariable(this.variable);
                if (var.IsEvaluated() == false)
                {
                    var.Evaluate();
                }
                if (var.GetDataType() == EDataType.FLOAT)
                {
                    var.SetEvaluation(new Value(var.GetEvaluation().GetFloat() - 1));
                }
                else if (var.GetDataType() == EDataType.INTEGER)
                {
                    var.SetEvaluation(new Value(var.GetEvaluation().GetInt() - 1));
                }
                else
                {
                    Interpreter.Interpreter.PrintError("Cannot decrement variable '" + this.variable + "': only floats and integers can be decremented!", this.token);
                }
            }
            else
            {
                Interpreter.Interpreter.PrintError("Cannot decrement variable '" + this.variable + "': unknown variable!", this.token);
            }
        }
    }
}

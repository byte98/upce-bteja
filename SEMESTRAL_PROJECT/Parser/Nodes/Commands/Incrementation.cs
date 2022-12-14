using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralProject.Parser.Nodes.Commands
{
    /// <summary>
    /// Class representing incrementation of variable
    /// </summary>
    class Incrementation: Node, IExecutableNode
    {
        /// <summary>
        /// Name of variable which will be incrementeed
        /// </summary>
        private readonly string variable;

        /// <summary>
        /// Creates new incrementation of variable
        /// </summary>
        /// <param name="tokens">Tokens from source code</param>
        /// <param name="token">Token representing node</param>
        /// <param name="block">Block of code to which this node belongs to</param>
        /// <param name="variable">Name of variable which will be incremented</param>
        public Incrementation(Lexer.TokenStream tokens, Lexer.Token token, BlockNode block, string variable)
            : base(tokens, token, block)
        {
            this.variable = variable;
        }

        public override void Build()
        {
            this.StartBuild();
            if (this.token.GetTokenType() != Lexer.TokenType.INCREMENT)
            {
                this.PrintUnexpectedToken("incrementation", this.token);
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
                    var.SetEvaluation(new Value(var.GetEvaluation().GetFloat() + 1));
                }
                else if (var.GetDataType() == EDataType.INTEGER)
                {
                    var.SetEvaluation(new Value(var.GetEvaluation().GetInt() + 1));
                }
                else
                {
                    Interpreter.Interpreter.PrintError("Cannot increment variable '" + this.variable + "': only floats and integers can be incremented!", this.token);
                }
            }
            else
            {
                Interpreter.Interpreter.PrintError("Cannot increment variable '" + this.variable + "': unknown variable!", this.token);
            }
        }
    }
}

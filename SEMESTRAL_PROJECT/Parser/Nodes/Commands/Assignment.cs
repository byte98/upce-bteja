using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralProject.Parser.Nodes.Commands
{
    /// <summary>
    /// Class representing assignment value to variable
    /// </summary>
    class Assignment: Node, IExecutableNode
    {
        /// <summary>
        /// New value of variable
        /// </summary>
        IValueNode newValue = null;

        /// <summary>
        /// Placeholder for incrementation and decrementation
        /// </summary>
        IExecutableNode exec = null;

        /// <summary>
        /// Name of variable to which value will be assigned
        /// </summary>
        private string variable;

        /// <summary>
        /// Creates new assignment of value to variable
        /// </summary>
        /// <param name="tokens">Tokens from source code</param>
        /// <param name="token">Token representing node</param>
        /// <param name="parent">Parent block to which this block belongs to</param>
        public Assignment(Lexer.TokenStream tokens, Lexer.Token token, BlockNode block)
            : base(tokens, token, block) { }

        public override void Build()
        {
            this.StartBuild();
            if (this.token.GetTokenType() == Lexer.TokenType.VARIABLE)
            {
                if (this.token.HasValue())
                {
                    if (this.tokens.HasNext())
                    {
                        Lexer.Token next = this.tokens.GetNext();
                        if (next.GetTokenType() == Lexer.TokenType.ASSIGNMENT)
                        {
                            if (this.tokens.HasNext())
                            {
                                Expressions.Expression expr = new Expressions.Expression(this.tokens, this.tokens.GetNext(), this.block);
                                expr.Build();
                                this.newValue = expr;
                            }
                            else
                            {
                                this.PrintUnexpectedEndOfProgram("any expression", next);
                            }
                        }
                        else if (next.GetTokenType() == Lexer.TokenType.INCREMENT)
                        {
                            Incrementation incr = new Incrementation(this.tokens, next, this.block, this.token.GetValue());
                            incr.Build();
                            this.exec = incr;
                        }
                        else if (next.GetTokenType() == Lexer.TokenType.DECREMENT)
                        {
                            Decrementation decr = new Decrementation(this.tokens, next, this.block, this.token.GetValue());
                            decr.Build();
                            this.exec = decr;
                        }
                        else
                        {
                            this.PrintUnexpectedToken("any expression", next);
                        }
                    }
                    else
                    {
                        this.PrintUnexpectedEndOfProgram("any expression");
                    }
                }
                else
                {
                    Parser.PrintError("Building " + this.ToString() + " failed: unknown value of variable identifier!", this.token);
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
            if (this.exec != null)
            {
                this.exec.Execute();
            }
            else if (this.newValue != null)
            {
                if (this.block.HasVariable(this.token.GetValue()))
                {
                    this.newValue.Execute();
                    if (this.newValue.HasValue())
                    {
                        if (this.newValue.GetValue().GetDataType() == this.block.GetVariable(this.token.GetValue()).GetDataType())
                        {
                            this.block.GetVariable(this.token.GetValue()).SetEvaluation(this.newValue.GetValue());
                        }
                        else
                        {
                            Interpreter.Interpreter.PrintError("Assignment of value to variable '" + this.token.GetValue() + "' failed: Cannot assign "
                                + this.newValue.GetValue().GetDataType() + " to " + this.block.GetVariable(this.token.GetValue()).GetDataType() + "!", this.token);
                        }
                    }
                    else
                    {
                        Interpreter.Interpreter.PrintError("Assignment of value to variable '" + this.token.GetValue() + "' failed: There is nothing to assign!", this.token);
                    }
                    
                }
                else
                {
                    Interpreter.Interpreter.PrintError("Execution of " + this.ToString() + " failed: unknown variable!", this.token);
                }
            }
            else
            {
                Interpreter.Interpreter.PrintError("Execution of " + this.ToString() + " failed: nothing to execute!", this.token);
            }
        }
    }
}

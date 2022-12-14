using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemestralProject.Lexer;
using SemestralProject.Parser.Nodes.Expressions;

namespace SemestralProject.Parser.Nodes.Commands
{
    /// <summary>
    /// Class representing node which prints something to standard output
    /// </summary>
    class Write : Node, IExecutableNode
    {
        /// <summary>
        /// Value which will be printed
        /// </summary>
        private IValueNode value;

        /// <summary>
        /// Creates new write node
        /// </summary>
        /// <param name="tokens">Tokens from source code</param>
        /// <param name="token">Token representing node</param>
        /// <param name="parent">Parent block to which this block belongs to</param>
        public Write(Lexer.TokenStream tokens, Lexer.Token token, BlockNode block)
            : base(tokens, token, block) { }

        public override void Build()
        {
            this.StartBuild();
            if (this.token.GetTokenType() == TokenType.WRITE)
            {
                if (this.tokens.HasNext())
                {
                    Token next = this.tokens.GetNext();
                    if (next.GetTokenType() == TokenType.OPEN_BRACKET)
                    {
                        if (this.tokens.HasNext())
                        {
                            Expression expr = new Expression(this.tokens, this.tokens.GetNext(), this.block);
                            expr.Build();
                            this.value = expr;
                            if (this.tokens.HasNext())
                            {
                                next = this.tokens.GetNext();
                                if (next.GetTokenType() != TokenType.CLOSE_BRACKET)
                                {
                                    this.PrintUnexpectedToken("close bracket", next);
                                }
                            }
                            else
                            {
                                this.PrintUnexpectedEndOfProgram("close bracket", next);
                            }
                        }
                        else
                        {
                            this.PrintUnexpectedEndOfProgram("any expression", next);
                        }
                    }
                    else
                    {
                        this.PrintUnexpectedToken("open bracket", next);
                    }
                }
                else
                {
                    this.PrintUnexpectedEndOfProgram("open bracket", this.token);
                }
            }
            else
            {
                this.PrintUnexpectedToken("write", this.token);
            }
            this.FinishBuild();
        }

        public void Execute()
        {
            this.value.Execute();
            if (this.value.HasValue())
            {
                if (this.value.GetValue().GetDataType() == EDataType.STRING)
                {
                    Console.WriteLine(this.value.GetValue().GetString());
                }
                else
                {
                    Interpreter.Interpreter.PrintError("Writing to standard output failed: Only strings are allowed to write (not " + this.value.GetValue().GetDataType() + ")!", this.token);
                }
            }
            else
            {
                Interpreter.Interpreter.PrintError("Writing to standard output failed: Nothing to write!", this.token);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemestralProject.Lexer;

namespace SemestralProject.Parser.Nodes.Expressions
{
    /// <summary>
    /// Class representing parsing value to string
    /// </summary>
    class StrVal: Node, IValueNode
    {
        /// <summary>
        /// Parsed value
        /// </summary>
        private Value value = null;

        /// <summary>
        /// Expression which will be parsed into string
        /// </summary>
        private Expression expression;

        /// <summary>
        /// Creates parsing value to string
        /// </summary>
        /// <param name="tokens">Tokens from source code</param>
        /// <param name="token">Token representing node</param>
        /// <param name="parent">Parent block to which this block belongs to</param>
        public StrVal(TokenStream tokens, Token token, BlockNode block)
            : base(tokens, token, block) { }

        public override void Build()
        {
            this.StartBuild();
            if (this.token.GetTokenType() != TokenType.STRVAL)
            {
                this.PrintUnexpectedToken("strval", this.token);
            }
            else
            {
                if (this.tokens.HasNext())
                {
                    Token next = this.tokens.GetNext();
                    if (next.GetTokenType() == TokenType.OPEN_BRACKET)
                    {
                        if (this.tokens.HasNext())
                        {
                            this.expression = new Expression(this.tokens, this.tokens.GetNext(), this.block);
                            this.expression.Build();
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
                    this.PrintUnexpectedEndOfProgram("open bracket");
                }
            }
            this.FinishBuild();
        }

        public void Execute()
        {
            this.expression.Execute();
            if (this.expression.HasValue())
            {
                this.value = new Value(this.expression.GetValue().GetData(), false);
            }
            else
            {
                Interpreter.Interpreter.PrintError("Execution of parsing value to string failed: nothing to parse!", this.token);
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

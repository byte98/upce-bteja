using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemestralProject.Lexer;

namespace SemestralProject.Parser.Nodes.Expressions
{
    /// <summary>
    /// Class representing parsing value to floating point number
    /// </summary>
    class FloatVal: Node, IValueNode
    {
        /// <summary>
        /// Parsed value
        /// </summary>
        private Value value = null;

        /// <summary>
        /// Expression which will be parsed into floating point number
        /// </summary>
        private Expression expression;

        /// <summary>
        /// Creates parsing value to floating point number
        /// </summary>
        /// <param name="tokens">Tokens from source code</param>
        /// <param name="token">Token representing node</param>
        /// <param name="parent">Parent block to which this block belongs to</param>
        public FloatVal(TokenStream tokens, Token token, BlockNode block)
            : base(tokens, token, block) { }

        public override void Build()
        {
            this.StartBuild();
            if (this.token.GetTokenType() != TokenType.FLOATVAL)
            {
                this.PrintUnexpectedToken("floatval", this.token);
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
                Value val = this.expression.GetValue();
                if (val.GetDataType() == EDataType.FLOAT)
                {
                    this.value = new Value(val.GetFloat());
                }
                else if (val.GetDataType() == EDataType.STRING)
                {
                    float fval;
                    if (float.TryParse(val.GetString(), out fval))
                    {
                        this.value = new Value(fval);
                    }
                    else
                    {
                        Interpreter.Interpreter.PrintError("Execution of parsing value to float failed: cannot parse '" + val.GetString() + "'!", this.token);
                    }
                }
                else if (val.GetDataType() == EDataType.INTEGER)
                {
                    this.value = new Value((float)val.GetInt());
                }
            }
            else
            {
                Interpreter.Interpreter.PrintError("Execution of parsing value to float failed: nothing to parse!", this.token);
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

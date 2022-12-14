using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemestralProject.Lexer;
using SemestralProject.Parser.Nodes.Blocks;

namespace SemestralProject.Parser.Nodes.Commands
{
    /// <summary>
    /// Class representing return call from function
    /// </summary>
    class Return: Node, IValueNode
    {
        /// <summary>
        /// Return value of function
        /// </summary>
        private Expressions.Expression reti = null;

        /// <summary>
        /// Value of return value
        /// </summary>
        private Value retiVal;

        /// <summary>
        /// Creates new return call from function
        /// </summary>
        /// <param name="tokens">Tokens from source code</param>
        /// <param name="token">Token representing node</param>
        /// <param name="block">Parent block to which this block belongs to</param>
        public Return(Lexer.TokenStream tokens, Lexer.Token token, BlockNode block)
            : base(tokens, token, block)
        { }

        public override void Build()
        {
            this.StartBuild();
            if (this.token.GetTokenType() == Lexer.TokenType.RETURN)
            {
                if (this.tokens.HasNext())
                {
                    Token next = this.tokens.ObserveNext();
                    if (next.GetTokenType() != TokenType.SEMICOLON)
                    {
                        this.reti = new Expressions.Expression(this.tokens, this.tokens.GetNext(), this.block);
                        this.reti.Build();
                        if (this.tokens.HasNext())
                        {
                            next = this.tokens.GetNext();
                            if (next.GetTokenType() != TokenType.SEMICOLON)
                            {
                                this.PrintUnexpectedToken("semicolon", next);
                            }
                        }
                        else
                        {
                            this.PrintUnexpectedEndOfProgram("semicolon");
                        }
                    }
                }
                else
                {
                    this.PrintUnexpectedEndOfProgram("semicolon or any expression");
                }
            }
            else
            {
                this.PrintUnexpectedToken("return", this.token);
            }
            this.FinishBuild();
        }

        public void Execute()
        {
            if (this.HasReturnValue())
            {
                this.reti.Execute();
                if (this.reti.HasValue())
                {
                    this.retiVal = this.reti.GetValue();
                }
            }
        }

        /// <summary>
        /// Checks, whether any value is returned
        /// </summary>
        /// <returns>TRUE if any value is returned, FALSE otherwise</returns>
        public bool HasReturnValue()
        {
            return this.reti != null;
        }

        public bool HasValue()
        {
            return this.retiVal != null;
        }

        public Value GetValue()
        {
            return this.retiVal;
        }
    }
}

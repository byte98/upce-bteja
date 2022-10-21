using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW03.Parser.Nodes
{
    /// <summary>
    /// Class representing multiply binary expression
    /// </summary>
    class MultiplyBinaryExpression: Node, Evaluatable
    {
        /// <summary>
        /// Left side of multiply binary expression
        /// </summary>
        private LiteralExpression leftSide;

        /// <summary>
        /// Expression which is member of multiply binary expression
        /// </summary>
        private Expression expression;

        /// <summary>
        /// Literal expression which is member of multiply binary expression
        /// </summary>
        private LiteralExpression literalExpression;

        /// <summary>
        /// Creates new binary expression
        /// </summary>
        /// <param name="tokens">Tokens browser</param>
        /// <param name="token">Token defining multiply binary expression</param>
        public MultiplyBinaryExpression(Lexer.Lexer.LexerResults tokens, Lexer.Token token) : base(tokens, token) { }

        public override void Build()
        {
            if (this.tokens.HasNext())
            {
                Lexer.Token next = this.tokens.GetNext();
                if (next.GetTokenType() == Lexer.TokenType.PLUS)
                {
                    if (this.token.GetTokenType() == Lexer.TokenType.NUMBER && this.token.HasValue())
                    {
                        this.BuildLeftSide(this.token);
                        next = this.tokens.GetNext();
                        if (next.GetTokenType() == Lexer.TokenType.NUMBER)
                        {
                            this.BuildLiteralExpression(next);
                        }
                        else if (next.GetTokenType() == Lexer.TokenType.OPEN_BRACKET)
                        {
                            this.BuildExpression(next);
                        }
                        else
                        {
                            Parser.Print("Unexpected state during parsing multiply binary expression! " + next.ToString());
                        }
                    }
                    else
                    {
                        Parser.Print("Unexpected left side of multiply binary expression! " + this.token.ToString());
                    }
                }
                else
                {
                    Parser.Print("Unexpected state during parsing multiply binary expression! " + next.ToString());
                }
            }
        }

        /// <summary>
        /// Builds left side of multiply binary expression
        /// </summary>
        /// <param name="token"></param>
        private void BuildLeftSide(Lexer.Token token)
        {
            this.leftSide = new LiteralExpression(this.tokens, token);
            this.leftSide.Build();
        }

        /// <summary>
        /// Builds literal expression
        /// </summary>
        /// <param name="token">Token defining literal expression</param>
        private void BuildLiteralExpression(Lexer.Token token)
        {
            this.literalExpression = new LiteralExpression(this.tokens, token);
            this.literalExpression.Build();
        }

        /// <summary>
        /// Builds expression
        /// </summary>
        /// <param name="token">Token defining expression</param>
        private void BuildExpression(Lexer.Token token)
        {
            this.expression = new Expression(this.tokens, token);
            this.expression.Build();
        }

        public double Evaluate()
        {
            double reti = double.NaN;
            if (this.literalExpression != null)
            {
                reti = this.leftSide.Evaluate() * this.literalExpression.Evaluate();
            }
            else if (this.expression != null)
            {
                reti = this.leftSide.Evaluate() * this.expression.Evaluate();
            }
            return reti;
        }
    }
}

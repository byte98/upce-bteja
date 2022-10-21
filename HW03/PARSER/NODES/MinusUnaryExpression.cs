using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW03.Parser.Nodes
{
    /// <summary>
    /// Class representing minus unary expression
    /// </summary>
    class MinusUnaryExpression : Node, Evaluatable
    {
        /// <summary>
        /// Expression to which this minus unary expression will be applied
        /// </summary>
        private Expression expression;

        /// <summary>
        /// Literal expression to which this minus unary expression will be applied
        /// </summary>
        private LiteralExpression literalExpression;

        /// <summary>
        /// Creates new plus unary expression
        /// </summary>
        /// <param name="tokens">Tokens browser</param>
        /// <param name="token">Token defining minus unary expression</param>
        public MinusUnaryExpression(Lexer.Lexer.LexerResults tokens, Lexer.Token token) : base(tokens, token) { }

        public override void Build()
        {
            if (this.tokens.HasNext())
            {
                Lexer.Token t = this.tokens.GetNext();
                if (t.GetTokenType() == Lexer.TokenType.OPEN_BRACKET)
                {
                    this.BuildExpression(t);
                }
                else if (t.GetTokenType() == Lexer.TokenType.NUMBER)
                {
                    this.BuildLiteralExpression(t);
                }
                else
                {
                    Parser.Print("Unexpected token during parsing minus unary expression! " + t.ToString());
                }
            }
        }

        /// <summary>
        /// Builds expression
        /// </summary>
        /// <param name="t">Token defining expression</param>
        private void BuildExpression(Lexer.Token t)
        {
            this.expression = new Expression(this.tokens, t);
            this.expression.Build();
        }

        /// <summary>
        /// Builds literal expression
        /// </summary>
        /// <param name="t">Token defining literal expression</param>
        private void BuildLiteralExpression(Lexer.Token t)
        {
            this.literalExpression = new LiteralExpression(this.tokens, t);
            this.literalExpression.Build();
        }

        public double Evaluate()
        {
            double reti = double.NaN;
            if (this.literalExpression != null)
            {
                reti = (-1) * this.literalExpression.Evaluate();
            }
            else if (this.expression != null)
            {
                reti = (-1) * this.expression.Evaluate();
            }
            return reti;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW03.Parser.Nodes
{
    /// <summary>
    /// Class representing unary expression
    /// </summary>
    class UnaryExpression: Node, Evaluatable
    {
        /// <summary>
        /// Plus unary expression defined in this unary expression
        /// </summary>
        private PlusUnaryExpression plusUnaryExpression;

        /// <summary>
        /// Minus unary expression defined in this unary expression
        /// </summary>
        private MinusUnaryExpression minusUnaryExpression;

        /// <summary>
        /// Creates new unary expression
        /// </summary>
        /// <param name="tokens">Tokens browser</param>
        /// <param name="token">Token defining unary expression</param>
        public UnaryExpression(Lexer.Lexer.LexerResults tokens, Lexer.Token token) : base(tokens, token) { }

        public override void Build()
        {
            if (this.token.GetTokenType() == Lexer.TokenType.PLUS)
            {
                this.BuildPlusUnaryExpression();
            }
            else if (this.token.GetTokenType() == Lexer.TokenType.MINUS)
            {
                this.BuildMinusUnaryExpression();
            }
            else
            {
                Parser.Print("Unexpected state during parsing unary expression! " + this.token.ToString());
            }
        }

        /// <summary>
        /// Builds plus unary expression
        /// </summary>
        private void BuildPlusUnaryExpression()
        {
            this.plusUnaryExpression = new PlusUnaryExpression(this.tokens, this.token);
            this.plusUnaryExpression.Build();
        }

        /// <summary>
        /// Builds minus unary expression
        /// </summary>
        private void BuildMinusUnaryExpression()
        {
            this.minusUnaryExpression = new MinusUnaryExpression(this.tokens, this.token);
            this.minusUnaryExpression.Build();
        }

        public double Evaluate()
        {
            double reti = double.NaN;
            if (this.plusUnaryExpression != null)
            {
                reti = this.plusUnaryExpression.Evaluate();
            }
            else if (this.minusUnaryExpression != null)
            {
                reti = this.minusUnaryExpression.Evaluate();
            }
            return reti;
        }
    }
}

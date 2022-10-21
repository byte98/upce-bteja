using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW03.Parser.Nodes
{
    /// <summary>
    /// Class representing binary expression
    /// </summary>
    class BinaryExpression : Node, Evaluatable
    {
        /// <summary>
        /// Plus binary expression defined by this expression
        /// </summary>
        private PlusBinaryExpression plusBinaryExpression;

        /// <summary>
        /// Minus binary expression defined by this expression
        /// </summary>
        private MinusBinaryExpression minusBinaryExpression;

        /// <summary>
        /// Multiply binary expression defined by this expression
        /// </summary>
        private MultiplyBinaryExpression multiplyBinaryExpression;

        /// <summary>
        /// Divide binary expression defined by this expression
        /// </summary>
        private DivideBinaryExpression divideBinaryExpression;

        /// <summary>
        /// Creates new binary expression
        /// </summary>
        /// <param name="tokens">Tokens browser</param>
        /// <param name="token">Token defining binary expression</param>
        public BinaryExpression(Lexer.Lexer.LexerResults tokens, Lexer.Token token) : base(tokens, token) { }

        public override void Build()
        {
            Lexer.Token next = this.tokens.ObserveNext();
            if (next.GetTokenType() == Lexer.TokenType.PLUS)
            {
                this.BuildPlusBinaryExpression();
            }
            else if (next.GetTokenType() == Lexer.TokenType.MINUS)
            {
                this.BuildMinusBinaryExpression();
            }
            else if (next.GetTokenType() == Lexer.TokenType.MULTIPLY)
            {
                this.BuildMultiplyBinaryExpression();
            }
            else if (next.GetTokenType() == Lexer.TokenType.DIVIDE)
            {
                this.BuildDivideBinaryExpression();
            }
            else
            {
                Parser.Print("Unexpected token during parsing binary expression! " + next.ToString());
            }
        }

        /// <summary>
        /// Builds plus binary expression
        /// </summary>
        private void BuildPlusBinaryExpression()
        {
            this.plusBinaryExpression = new PlusBinaryExpression(this.tokens, this.token);
            this.plusBinaryExpression.Build();
        }

        /// <summary>
        /// Builds minus binary expression
        /// </summary>
        private void BuildMinusBinaryExpression()
        {
            this.minusBinaryExpression = new MinusBinaryExpression(this.tokens, this.token);
            this.minusBinaryExpression.Build();
        }

        /// <summary>
        /// Builds multiply binary expression
        /// </summary>
        private void BuildMultiplyBinaryExpression()
        {
            this.multiplyBinaryExpression = new MultiplyBinaryExpression(this.tokens, this.token);
            this.multiplyBinaryExpression.Build();
        }

        /// <summary>
        /// Builds divide binary expression
        /// </summary>
        private void BuildDivideBinaryExpression()
        {
            this.divideBinaryExpression = new DivideBinaryExpression(this.tokens, this.token);
            this.divideBinaryExpression.Build();
        }

        public double Evaluate()
        {
            double reti = double.NaN;
            if (this.plusBinaryExpression != null)
            {
                reti = this.plusBinaryExpression.Evaluate();
            }
            else if (this.minusBinaryExpression != null)
            {
                reti = this.minusBinaryExpression.Evaluate();
            }
            else if (this.multiplyBinaryExpression != null)
            {
                reti = this.multiplyBinaryExpression.Evaluate();
            }
            else if (this.divideBinaryExpression != null)
            {
                reti = this.divideBinaryExpression.Evaluate();
            }
            return reti;
        }
    }
}

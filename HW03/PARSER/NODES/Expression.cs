using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW03.Parser.Nodes
{
    /// <summary>
    /// Class representing expression
    /// </summary>
    class Expression: Node, Evaluatable
    {
        /// <summary>
        /// Unary expression defined in this expression
        /// </summary>
        private UnaryExpression unaryExpression;

        /// <summary>
        /// Expression defined in this expression
        /// </summary>
        private Expression expression;

        /// <summary>
        /// Binary expression defined in this expression
        /// </summary>
        private BinaryExpression binaryExpression;

        /// <summary>
        /// Literal expression defined in this expression
        /// </summary>
        private LiteralExpression literalExpression;

        /// <summary>
        /// Creates new expression
        /// </summary>
        /// <param name="tokens">Tokens browser</param>
        /// <param name="token">Token which defines expression</param>
        public Expression(Lexer.Lexer.LexerResults tokens, Lexer.Token token) : base(tokens, token) { }

        public override void Build()
        {
            if (this.tokens.HasNext())
            {
                Lexer.Token t = this.tokens.GetNext();
                if (t.GetTokenType() == Lexer.TokenType.PLUS ||
                    t.GetTokenType() == Lexer.TokenType.MINUS)
                {
                    this.BuildUnaryExpression(t);
                }
                else if (t.GetTokenType() == Lexer.TokenType.OPEN_BRACKET)
                {
                    this.BuildExpression(t);
                }
                else if (t.GetTokenType() == Lexer.TokenType.NUMBER)
                {
                    Lexer.Token n = this.tokens.ObserveNext();
                    if (n.GetTokenType() == Lexer.TokenType.PLUS ||
                        n.GetTokenType() == Lexer.TokenType.MINUS ||
                        n.GetTokenType() == Lexer.TokenType.DIVIDE ||
                        n.GetTokenType() == Lexer.TokenType.MULTIPLY)
                    {
                        this.BuildBinaryExpression(t);
                    }
                    else
                    {
                        this.BuildLiteralExpression(t); 
                    }
                }
                else
                {
                    Parser.Print("Unexpected token while parsing expression! " + t.ToString());
                }
            }
        }

        /// <summary>
        /// Builds unary expression
        /// </summary>
        /// <param name="t">Token defining unary expression</param>
        private void BuildUnaryExpression(Lexer.Token t)
        {
            this.unaryExpression = new UnaryExpression(this.tokens, t);
            this.unaryExpression.Build();
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
        /// Builds binary expression
        /// </summary>
        /// <param name="t">Token defining binary expression</param>
        private void BuildBinaryExpression(Lexer.Token t)
        {
            this.binaryExpression = new BinaryExpression(this.tokens, t);
            this.binaryExpression.Build();
        }

        public double Evaluate()
        {
            double reti = double.NaN;
            if (this.expression != null)
            {
                reti = this.expression.Evaluate();
            }
            else if (this.binaryExpression != null)
            {
                reti = this.binaryExpression.Evaluate();
            }
            else if (this.unaryExpression != null)
            {
                reti = this.unaryExpression.Evaluate();
            }
            else if (this.literalExpression != null)
            {
                reti = this.literalExpression.Evaluate();
            }
            return reti;
        }
    }
}

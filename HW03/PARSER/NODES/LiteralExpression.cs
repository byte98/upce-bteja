using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW03.Parser.Nodes
{
    /// <summary>
    /// Class representing literal expression
    /// </summary>
    class LiteralExpression : Node, Evaluatable
    {
        /// <summary>
        /// Value which is hold by expression
        /// </summary>
        private double value;

        /// <summary>
        /// Creates new literal expression
        /// </summary>
        /// <param name="tokens">Tokens browser</param>
        /// <param name="token">Token defining literal expression</param>
        public LiteralExpression(Lexer.Lexer.LexerResults tokens, Lexer.Token token)
            : base(tokens, token) { }

        public override void Build()
        {
            if (this.token.GetTokenType() == Lexer.TokenType.NUMBER && this.token.HasValue())
            {
                float val = float.NaN;
                if (float.TryParse(this.token.GetValue(), out val))
                {
                    this.value = val;
                }
                else
                {
                    Parser.Print("Invalid number format! " + this.token.ToString());
                }
            }
            else
            {
                Parser.Print("Unexpected token during parsing literal expression! " + this.token.ToString()); 
            }
        }

        public double Evaluate()
        {
            return this.value;
        }

    }
}

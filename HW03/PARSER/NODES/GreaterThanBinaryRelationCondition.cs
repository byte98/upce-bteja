using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW03.Parser.Nodes
{
    /// <summary>
    /// Class representing greater than binary relation condition
    /// </summary>
    class GreaterThanBinaryRelationCondition : AbstrBinaryRelationCondition
    {
        /// <summary>
        /// Creates greater than binary relation condition
        /// </summary>
        /// <param name="tokens">Tokens browser</param>
        /// <param name="token">Token defining greater than binary condition</param>
        /// <param name="leftSide">Left side of condition</param>
        public GreaterThanBinaryRelationCondition(Lexer.Lexer.LexerResults tokens, Lexer.Token token, Expression leftSide)
            : base(tokens, token, leftSide)
        {
            this.name = "greater than binary relation condition";
        }

        public override bool CheckToken(Lexer.Token token)
        {
            return token.GetTokenType() == Lexer.TokenType.GREATER;
        }

        public override double Evaluate()
        {
            return this.leftSide.Evaluate() > this.rightSide.Evaluate() ? 1 : 0;
        }
    }
}

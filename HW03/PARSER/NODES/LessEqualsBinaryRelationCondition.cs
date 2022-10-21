using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW03.Parser.Nodes
{
    /// <summary>
    /// Class representing less equals binary relation condition
    /// </summary>
    class LessEqualsBinaryRelationCondition : AbstrBinaryRelationCondition
    {
        /// <summary>
        /// Creates less equals binary relation condition
        /// </summary>
        /// <param name="tokens">Tokens browser</param>
        /// <param name="token">Token defining less equals binary condition</param>
        /// <param name="leftSide">Left side of condition</param>
        public LessEqualsBinaryRelationCondition(Lexer.Lexer.LexerResults tokens, Lexer.Token token, Expression leftSide)
            : base(tokens, token, leftSide)
        {
            this.name = "less equals binary relation condition";
        }

        public override bool CheckToken(Lexer.Token token)
        {
            return token.GetTokenType() == Lexer.TokenType.LOWER_EQUAL;
        }

        public override double Evaluate()
        {
            return this.leftSide.Evaluate() <= this.rightSide.Evaluate() ? 1 : 0;
        }
    }
}

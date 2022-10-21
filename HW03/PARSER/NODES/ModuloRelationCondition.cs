using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW03.Parser.Nodes
{
    /// <summary>
    /// Class representing modulo binary relation condition
    /// </summary>
    class ModuloBinaryRelationCondition : AbstrBinaryRelationCondition
    {
        /// <summary>
        /// Creates modulo binary relation condition
        /// </summary>
        /// <param name="tokens">Tokens browser</param>
        /// <param name="token">Token defining modulo binary condition</param>
        /// <param name="leftSide">Left side of condition</param>
        public ModuloBinaryRelationCondition(Lexer.Lexer.LexerResults tokens, Lexer.Token token, Expression leftSide)
            : base(tokens, token, leftSide)
        {
            this.name = "modulo binary relation condition";
        }

        public override bool CheckToken(Lexer.Token token)
        {
            return token.GetTokenType() == Lexer.TokenType.HASH;
        }

        public override double Evaluate()
        {
            return (this.leftSide.Evaluate() % this.rightSide.Evaluate()) == 0 ? 1 : 0;
        }
    }
}

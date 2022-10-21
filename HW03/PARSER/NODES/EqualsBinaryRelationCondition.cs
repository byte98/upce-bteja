using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW03.Parser.Nodes
{
    /// <summary>
    /// Class representing equals binary relation condition
    /// </summary>
    class EqualsBinaryRelationCondition : AbstrBinaryRelationCondition
    {
        /// <summary>
        /// Creates equals binary relation condition
        /// </summary>
        /// <param name="tokens">Tokens browser</param>
        /// <param name="token">Token defining equals binary condition</param>
        /// <param name="leftSide">Left side of condition</param>
        public EqualsBinaryRelationCondition(Lexer.Lexer.LexerResults tokens, Lexer.Token token, Expression leftSide)
            : base(tokens, token, leftSide)
        {
            this.name = "equals binary relation condition";
        }

        public override bool CheckToken(Lexer.Token token)
        {
            return token.GetTokenType() == Lexer.TokenType.EQUALS;
        }

        public override double Evaluate()
        {
            return this.leftSide.Evaluate() == this.rightSide.Evaluate() ? 1 : 0;
        }
    }
}

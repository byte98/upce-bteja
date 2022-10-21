using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW03.Parser.Nodes
{
    /// <summary>
    /// Class representing condition
    /// </summary>
    class Condition: Node, Evaluatable
    {
        /// <summary>
        /// Odd condition hold by this condition
        /// </summary>
        private OddCondition oddCondition;

        /// <summary>
        /// Binary relation condition hold by this condition
        /// </summary>
        private BinaryRelationCondition binaryRelationCondition;

        /// <summary>
        /// Creates new condition
        /// </summary>
        /// <param name="tokens">Tokens browser</param>
        /// <param name="token">Token defining condition</param>
        public Condition(Lexer.Lexer.LexerResults tokens, Lexer.Token token): base(tokens, token){}

        public override void Build()
        {
            if (this.tokens.HasNext())
            {
                Lexer.Token t = this.tokens.GetNext();
                if (t.GetTokenType() == Lexer.TokenType.ODD)
                {
                    this.BuildOddCondition(t);
                }
                else
                {
                    this.BuildRelationCondition(t);
                }
            }
        }

        /// <summary>
        /// Builds odd condition
        /// </summary>
        /// <param name="t">Token defining condition</param>
        private void BuildOddCondition(Lexer.Token t)
        {
            this.oddCondition = new OddCondition(this.tokens, t);
            this.oddCondition.Build();
        }

        /// <summary>
        /// Builds relation condition
        /// </summary>
        /// <param name="t">Token dfeining condition</param>
        private void BuildRelationCondition(Lexer.Token t)
        {
            this.binaryRelationCondition = new BinaryRelationCondition(this.tokens, t);
            this.binaryRelationCondition.Build();
        }

        public double Evaluate()
        {
            double reti = double.NaN;
            if (this.oddCondition != null)
            {
                reti = this.oddCondition.Evaluate();
            }
            else if (this.binaryRelationCondition != null)
            {
                reti = this.binaryRelationCondition.Evaluate();
            }
            return reti;
        }
    }
}

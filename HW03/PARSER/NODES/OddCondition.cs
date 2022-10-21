using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW03.Parser.Nodes
{
    class OddCondition : Node, Evaluatable
    {
        /// <summary>
        /// Expression which will be evaluated
        /// </summary>
        private Expression expression;

        /// <summary>
        /// Creates new odd condition
        /// </summary>
        /// <param name="tokens">Tokens browser</param>
        /// <param name="token"></param>
        public OddCondition(Lexer.Lexer.LexerResults tokens, Lexer.Token token) : base(tokens, token) { }

        public override void Build()
        {
            this.expression = new Expression(this.tokens, token);
            this.expression.Build();
        }

        public double Evaluate()
        {
            double reti = double.NaN;
            if (this.expression != null)
            {
                reti = (this.expression.Evaluate() % 2 == 1) ? 1 : 0;
            }
            return reti;
        }
    }
}

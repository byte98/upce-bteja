using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW03.Parser.Nodes
{
    /// <summary>
    /// Class abstractiong common attributes and methods for all binary relation conditions
    /// </summary>
    abstract class AbstrBinaryRelationCondition : Node, Evaluatable
    {
        /// <summary>
        /// Left side of condition
        /// </summary>
        protected Expression leftSide;

        /// <summary>
        /// Right side of condition
        /// </summary>
        protected Expression rightSide;

        /// <summary>
        /// Name of binary relation condition
        /// </summary>
        protected string name = "binary relation condition";

        /// <summary>
        /// Creates new binary relation condition
        /// </summary>
        /// <param name="tokens">Tokens browser</param>
        /// <param name="token">Token defining binary relation condition</param>
        /// <param name="leftSide">Left side of condition</param>
        public AbstrBinaryRelationCondition(Lexer.Lexer.LexerResults tokens, Lexer.Token token, Expression leftSide)
            : base(tokens, token)
        {
            this.leftSide = leftSide;
        }

        public override void Build()
        {
            if (this.CheckToken(this.token))
            {
                if (this.tokens.HasNext())
                {
                    this.rightSide = new Expression(this.tokens, this.tokens.ObserveNext());
                    this.rightSide.Build();
                }
            }
            else
            {
                Parser.Print("Unexpected token during parsing " + this.name + "! " + this.token.ToString());
            }
        }

        /// <summary>
        /// Checks, whether token represents actual binary relation condition
        /// </summary>
        /// <param name="token">Token which will be checked</param>
        /// <returns>TRUE if token represents actual binary relation condition, FALSE otherwise</returns>
        public abstract bool CheckToken(Lexer.Token token);

        public abstract double Evaluate();
    }
}

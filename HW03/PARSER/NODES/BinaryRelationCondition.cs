using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW03.Parser.Nodes
{
    /// <summary>
    /// Class representing binary relation condition
    /// </summary>
    class BinaryRelationCondition: Node, Evaluatable
    {
        /// <summary>
        /// Equals binary relation condition hold by this binary relation condition
        /// </summary>
        private EqualsBinaryRelationCondition equalsBinaryRelationCondition;

        /// <summary>
        /// Modulo binary relation condition hold by this binary relation condition
        /// </summary>
        private ModuloBinaryRelationCondition moduloBinaryRelationCondition;

        /// <summary>
        /// Less than binary relation condition hold by this binary relation condition
        /// </summary>
        private LessThanBinaryRelationCondition lessThanBinaryRelationCondition;

        /// <summary>
        /// Less than or equals binary relation condition hold by this binary relation condition
        /// </summary>
        private LessEqualsBinaryRelationCondition lessEqualsBinaryRelationCondition;

        /// <summary>
        /// Greater than binary relation condition hold by this binary relation condition
        /// </summary>
        private GreaterThanBinaryRelationCondition greaterThanBinaryRelationCondition;

        /// <summary>
        /// Greater than or equals binary relation condition hold by this binary relation condition
        /// </summary>
        private GreaterEqualsBinaryRelationCondition greaterEqualsBinaryRelationCondition;

        /// <summary>
        /// Left side of expression
        /// </summary>
        private Expression leftSide;

        /// <summary>
        /// Creates new binary relation condition
        /// </summary>
        /// <param name="tokens">Tokens browser</param>
        /// <param name="token">Token defining binary relation condition</param>
        public BinaryRelationCondition(Lexer.Lexer.LexerResults tokens, Lexer.Token token) : base(tokens, token) { }

        public override void Build()
        {
            this.leftSide = new Expression(this.tokens, this.token);
            this.leftSide.Build();
            if (this.tokens.HasNext())
            {
                Lexer.Token t = this.tokens.GetNext();
                if (t.GetTokenType() == Lexer.TokenType.EQUALS)
                {
                    this.BuildEqualsBinaryRelationCondition(t);
                }
                else if (t.GetTokenType() == Lexer.TokenType.HASH)
                {
                    this.BuildModuloBinaryRelationCondition(t);
                }
                else if (t.GetTokenType() == Lexer.TokenType.LOWER)
                {
                    this.BuildLessThanBinaryRelationCondition(t);
                }
                else if (t.GetTokenType() == Lexer.TokenType.LOWER_EQUAL)
                {
                    this.BuildLessThanEqualsBinaryRelationCondition(t);
                }
                else if (t.GetTokenType() == Lexer.TokenType.GREATER)
                {
                    this.BuildGreaterThanBinaryRelationCondition(t);
                }
                else if (t.GetTokenType() == Lexer.TokenType.GREATER_EQUAL)
                {
                    this.BuildGreaterThanEqualsBinaryRelationCondition(t);
                }
                else
                {
                    Parser.Print("Unexpected token during parsing binary relation condition! " + t.ToString());
                }
            }
        }

        /// <summary>
        /// Builds equals binary relation condition
        /// </summary>
        /// <param name="token">Tokend defining binary relation condition</param>
        private void BuildEqualsBinaryRelationCondition(Lexer.Token token)
        {
            this.equalsBinaryRelationCondition = new EqualsBinaryRelationCondition(this.tokens, token, this.leftSide);
            this.equalsBinaryRelationCondition.Build();
        }

        /// <summary>
        /// Builds modulo binary relation condition
        /// </summary>
        /// <param name="token">Tokend defining binary relation condition</param>
        private void BuildModuloBinaryRelationCondition(Lexer.Token token)
        {
            this.moduloBinaryRelationCondition = new ModuloBinaryRelationCondition(this.tokens, token, this.leftSide);
            this.moduloBinaryRelationCondition.Build();
        }

        /// <summary>
        /// Builds less than binary relation condition
        /// </summary>
        /// <param name="token">Tokend defining binary relation condition</param>
        private void BuildLessThanBinaryRelationCondition(Lexer.Token token)
        {
            this.lessThanBinaryRelationCondition = new LessThanBinaryRelationCondition(this.tokens, token, this.leftSide);
            this.lessThanBinaryRelationCondition.Build();
        }

        /// <summary>
        /// Builds less or equals than binary relation condition
        /// </summary>
        /// <param name="token">Tokend defining binary relation condition</param>
        private void BuildLessThanEqualsBinaryRelationCondition(Lexer.Token token)
        {
            this.lessEqualsBinaryRelationCondition = new LessEqualsBinaryRelationCondition(this.tokens, token, this.leftSide);
            this.lessEqualsBinaryRelationCondition.Build();
        }

        /// <summary>
        /// Builds greater than binary relation condition
        /// </summary>
        /// <param name="token">Tokend defining binary relation condition</param>
        private void BuildGreaterThanBinaryRelationCondition(Lexer.Token token)
        {
            this.greaterThanBinaryRelationCondition = new GreaterThanBinaryRelationCondition(this.tokens, token, this.leftSide);
            this.greaterThanBinaryRelationCondition.Build();
        }

        /// <summary>
        /// Builds greater or equals than binary relation condition
        /// </summary>
        /// <param name="token">Tokend defining binary relation condition</param>
        private void BuildGreaterThanEqualsBinaryRelationCondition(Lexer.Token token)
        {
            this.greaterEqualsBinaryRelationCondition = new GreaterEqualsBinaryRelationCondition(this.tokens, token, this.leftSide);
            this.greaterEqualsBinaryRelationCondition.Build();
        }

        public double Evaluate()
        {
            double reti = double.NaN;
            if (this.equalsBinaryRelationCondition != null)
            {
                reti = this.equalsBinaryRelationCondition.Evaluate();
            }
            else if (this.moduloBinaryRelationCondition != null)
            {
                reti = this.moduloBinaryRelationCondition.Evaluate();
            }
            else if (this.lessThanBinaryRelationCondition != null)
            {
                reti = this.lessThanBinaryRelationCondition.Evaluate();
            }
            else if (this.lessEqualsBinaryRelationCondition != null)
            {
                reti = this.lessEqualsBinaryRelationCondition.Evaluate();
            }
            else if (this.greaterThanBinaryRelationCondition != null)
            {
                reti = this.greaterThanBinaryRelationCondition.Evaluate();
            }
            else if (this.greaterEqualsBinaryRelationCondition != null)
            {
                reti = this.greaterEqualsBinaryRelationCondition.Evaluate();
            }
            return reti;
        }

    }
}

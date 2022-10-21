using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW03.Parser.Nodes
{
    /// <summary>
    /// Class defining program node in AST
    /// </summary>
    class Program: Node
    {
        /// <summary>
        /// Expression defined from program node
        /// </summary>
        private Expression expression;

        /// <summary>
        /// Condition defined from program node
        /// </summary>
        private Condition condition;

        /// <summary>
        /// Creates new program node
        /// </summary>
        /// <param name="tokens">Tokens browser</param>
        public Program(Lexer.Lexer.LexerResults tokens) : base(tokens, null) { }

        public override void Build()
        {
            if (this.tokens.HasNext())
            {
                Lexer.Token token = this.tokens.GetNext();
                if (token.GetTokenType() == Lexer.TokenType.CONDITION)
                {
                    this.BuildCondition(token);
                }
                else if (token.GetTokenType() == Lexer.TokenType.EXPRESSION)
                {
                    this.BuildExpression(token);
                }
                else
                {
                    Parser.Print("Unexpected token during parsing program! " + token.ToString());
                }
            }
        }

        /// <summary>
        /// Builds expression
        /// </summary>
        /// <param name="token">Token defining expression</param>
        private void BuildExpression(Lexer.Token token)
        {
            this.expression = new Expression(this.tokens, token);
            this.expression.Build();
        }

        /// <summary>
        /// Builds condition
        /// </summary>
        /// <param name="token">Token defining condition</param>
        private void BuildCondition(Lexer.Token token)
        {
            this.condition = new Condition(this.tokens, token);
            this.condition.Build();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemestralProject.Lexer;

namespace SemestralProject.Parser.Nodes.Expressions
{
    /// <summary>
    /// Class factor used in expressions
    /// </summary>
    class Factor: Node, IValueNode
    {
        /// <summary>
        /// Value of factor
        /// </summary>
        private IValueNode value;

        /// <summary>
        /// Creates new factor used in expressions
        /// </summary>
        /// <param name="tokens">Tokens from source code</param>
        /// <param name="token">Token representing node</param>
        /// <param name="parent">Parent block to which this block belongs to</param>
        public Factor(TokenStream tokens, Token token, BlockNode block)
            : base(tokens, token, block) { }

        public override void Build()
        {
            this.StartBuild();
            if (this.token.GetTokenType() == TokenType.LITERAL_FLOAT ||
                this.token.GetTokenType() == TokenType.LITERAL_INTEGER ||
                this.token.GetTokenType() == TokenType.LITERAL_STRING)
            {
                Literal literal = new Literal(this.tokens, this.token, this.block);
                literal.Build();
                this.value = literal;
            }
            else if (this.token.GetTokenType() == TokenType.VARIABLE)
            {
                Variable variable = new Variable(this.tokens, this.token, this.block);
                variable.Build();
                this.value = variable;
            }
            else if (this.token.GetTokenType() == TokenType.FUNCTION_IDENTIFIER)
            {
                Call call = new Call(this.tokens, this.token, this.block);
                call.Build();
                this.value = call;
            }
            else if (this.token.GetTokenType() == TokenType.READ)
            {
                Read read = new Read(this.tokens, this.token, this.block);
                read.Build();
                this.value = read;
            }
            else if (this.token.GetTokenType() == TokenType.STRVAL)
            {
                StrVal strVal = new StrVal(this.tokens, this.token, this.block);
                strVal.Build();
                this.value = strVal;
            }
            else if (this.token.GetTokenType() == TokenType.FLOATVAL)
            {
                FloatVal floatVal = new FloatVal(this.tokens, this.token, this.block);
                floatVal.Build();
                this.value = floatVal;
            }
            else if (this.token.GetTokenType() == TokenType.INTVAL)
            {
                IntVal intVal = new IntVal(this.tokens, this.token, this.block);
                intVal.Build();
                this.value = intVal;
            }
            else
            {
                this.PrintUnexpectedToken("literal, variable or function call", this.token);
            }
            this.FinishBuild();
        }

        public void Execute()
        {
            this.value.Execute();
        }

        public Value GetValue()
        {
            return this.value.GetValue();
        }

        public bool HasValue()
        {
            return this.value.HasValue();
        }
    }
}

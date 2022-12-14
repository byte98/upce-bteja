using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemestralProject.Lexer;

namespace SemestralProject.Parser.Nodes.Commands
{
    /// <summary>
    /// Class representing some literal
    /// </summary>
    class Literal: Node, IValueNode
    {
        /// <summary>
        /// Data type of literal
        /// </summary>
        private EDataType dataType;

        /// <summary>
        /// Value of literal
        /// </summary>
        private Value value = null;

        /// <summary>
        /// Creates new literal
        /// </summary>
        /// <param name="tokens">Tokens from source code</param>
        /// <param name="token">Token representing node</param>
        /// <param name="parent">Parent block to which this block belongs to</param>
        public Literal(TokenStream tokens, Token token, BlockNode block)
            : base(tokens, token, block) { }

        public override void Build()
        {
            this.StartBuild();
            if (this.token.HasValue())
            {
                if (this.token.GetTokenType() == TokenType.LITERAL_STRING)
                {
                    this.dataType = EDataType.STRING;
                }
                else if (this.token.GetTokenType() == TokenType.LITERAL_INTEGER)
                {
                    this.dataType = EDataType.INTEGER;
                }
                else if (this.token.GetTokenType() == TokenType.LITERAL_FLOAT)
                {
                    this.dataType = EDataType.FLOAT;
                }
            }
            else
            {
                Parser.PrintError("Building literal " + this.ToString() + " failed: literal with no value passed!", this.token);
            }
            
            this.FinishBuild();
        }

        public void Execute()
        {
            this.value = new Value(this.token.GetValue(), this.dataType != EDataType.STRING);
        }

        public Value GetValue()
        {
            return this.value;
        }

        public bool HasValue()
        {
            return this.value != null;
        }
    }
}

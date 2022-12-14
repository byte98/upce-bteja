using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemestralProject.Lexer;

namespace SemestralProject.Parser.Nodes.Expressions
{
    /// <summary>
    /// Class representing reading from standard input
    /// </summary>
    class Read: Node, IValueNode
    {
        /// <summary>
        /// Value of standard input
        /// </summary>
        private Value value = null;

        /// <summary>
        /// Creates new reading from standard input
        /// </summary>
        /// <param name="tokens">Tokens from source code</param>
        /// <param name="token">Token representing node</param>
        /// <param name="parent">Parent block to which this block belongs to</param>
        public Read(TokenStream tokens, Token token, BlockNode block)
            : base(tokens, token, block) { }

        public override void Build()
        {
            this.StartBuild();
            if (this.token.GetTokenType() != TokenType.READ)
            {
                this.PrintUnexpectedToken("read", this.token);
            }
            this.FinishBuild();
        }

        public void Execute()
        {
            string input = Console.ReadLine();
            this.value = new Value(input, false);
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

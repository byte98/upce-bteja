using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralProject.Parser.Nodes.Commands
{
    /// <summary>
    /// Class representing break of cycle
    /// </summary>
    class Break: Node, IExecutableNode
    {
        /// <summary>
        /// Creates new break of cycle
        /// </summary>
        /// <param name="tokens">Tokens from source code</param>
        /// <param name="token">Token representing node</param>
        /// <param name="parent">Parent block to which this block belongs to</param>
        public Break(Lexer.TokenStream tokens, Lexer.Token token, BlockNode block)
            : base(tokens, token, block) { }

        public override void Build()
        {
            this.StartBuild();
            if (this.token.GetTokenType() != Lexer.TokenType.BREAK)
            {
                this.PrintUnexpectedToken("break", this.token);
            }
            this.FinishBuild();
        }

        public void Execute()
        {
            this.block.Break();
        }
    }
}

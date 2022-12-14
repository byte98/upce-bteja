using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralProject.Parser.Nodes.Commands
{
    /// <summary>
    /// Class representing no operation
    /// </summary>
    class Nop: Node, IExecutableNode
    {
        /// <summary>
        /// Creates new break of cycle
        /// </summary>
        /// <param name="tokens">Tokens from source code</param>
        /// <param name="token">Token representing node</param>
        /// <param name="parent">Parent block to which this block belongs to</param>
        public Nop(Lexer.TokenStream tokens, Lexer.Token token, BlockNode block)
            : base(tokens, token, block) { }

        public override void Build()
        {
            this.StartBuild();
            this.FinishBuild();
        }

        public void Execute()
        {
            // Do nothing
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralProject.Parser.Nodes.Commands
{
    /// <summary>
    /// Class representing declaration of variable
    /// </summary>
    class Declaration: Node, IExecutableNode
    {
        /// <summary>
        /// Representation of variable which has been declared
        /// </summary>
        private VariableModel variable;

        /// <summary>
        /// Creates new declaration node
        /// </summary>
        /// <param name="tokens">Tokens from source code</param>
        /// <param name="token">Token representing node</param>
        /// <param name="parent">Parent block to which this block belongs to</param>
        public Declaration(Lexer.TokenStream tokens, Lexer.Token token, BlockNode block)
            : base(tokens, token, block) { }

        public override void Build()
        {
            this.StartBuild();
        }

        public void Execute()
        {

        }
    }
}

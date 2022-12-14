using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemestralProject.Lexer;
using SemestralProject.Parser.Nodes.Commands;

namespace SemestralProject.Parser.Nodes.Blocks
{
    /// <summary>
    /// Class representing generic block of code
    /// </summary>
    class ExecutableBlock: ExecutableBlockNode
    {
        /// <summary>
        /// Nodes contained in block
        /// </summary>
        private readonly IList<IExecutableNode> nodes;

        /// <summary>
        /// Creates new generic block of code
        /// </summary>
        /// <param name="tokens">Results of lexer</param>
        /// <param name="token">Token representing block</param>
        /// <param name="parent">Block to which this block belongs to</param>
        public ExecutableBlock(Lexer.TokenStream tokens, Lexer.Token token, BlockNode parent)
            : base(tokens, token, parent)
        {
            this.nodes = new List<IExecutableNode>();
        }

        public override void Build()
        {
            this.StartBuild();
            this.BuildContent(this.token);
            this.FinishBuild();
        }

        /// <summary>
        /// Builds content of block
        /// </summary>
        /// <param name="t">Token representing start of block</param>
        private void BuildContent(Token t)
        {
            if (t.GetTokenType() == TokenType.IF)
            {
                If nodeIf = new If(this.tokens, t, this);
                nodeIf.Build();
                this.nodes.Add(nodeIf);
                if (this.tokens.HasNext() && (this.tokens.ObserveNext().GetTokenType() != TokenType.CLOSE_CURLY &&
                                              this.tokens.ObserveNext().GetTokenType() != TokenType.PROG_END))
                {
                    this.BuildContent(this.tokens.GetNext());
                }
            }
            else if (t.GetTokenType() == TokenType.WHILE)
            {
                While nodeWhile = new While(this.tokens, t, this);
                nodeWhile.Build();
                this.nodes.Add(nodeWhile);
                if (this.tokens.HasNext() && (this.tokens.ObserveNext().GetTokenType() != TokenType.CLOSE_CURLY &&
                                              this.tokens.ObserveNext().GetTokenType() != TokenType.PROG_END))
                {
                    this.BuildContent(this.tokens.GetNext());
                }
            }
            else if (t.GetTokenType() != TokenType.PROG_END &&
                t.GetTokenType() != TokenType.CLOSE_CURLY)
            {
                Command cmd = new Command(this.tokens, t, this);
                cmd.Build();
                this.nodes.Add(cmd);
                if (this.tokens.HasNext() && (this.tokens.ObserveNext().GetTokenType() != TokenType.CLOSE_CURLY &&
                                              this.tokens.ObserveNext().GetTokenType() != TokenType.PROG_END))
                {
                    this.BuildContent(this.tokens.GetNext());
                }
            }
        }

        public override void Execute()
        {
            this.Reset();
            foreach (IExecutableNode exec in this.nodes)
            {
                if (this.executing == true)
                {
                    exec.Execute();
                }
                else
                {
                    break;
                }
            }
        }
    }
}

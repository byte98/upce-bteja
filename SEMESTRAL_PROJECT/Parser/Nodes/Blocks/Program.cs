using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemestralProject.Lexer;

namespace SemestralProject.Parser.Nodes.Blocks
{
    /// <summary>
    /// Class representing program node
    /// </summary>
    class Program: BlockNode, IExecutableNode
    {
        /// <summary>
        /// Content of program
        /// </summary>
        private ExecutableBlock content;

        /// <summary>
        /// Creates new program node
        /// </summary>
        /// <param name="tokens">Tokens from source code</param>
        /// <param name="token">Token representing node</param>
        /// <param name="block">Block to which thid node belongs to</param>
        public Program(Lexer.TokenStream tokens, Lexer.Token token, BlockNode block)
            : base(tokens, token, block) { }

        public override void Build()
        {
            this.StartBuild();
            if (this.tokens.HasNext())
            {
                Token t = this.tokens.GetNext();
                if (t.GetTokenType() == TokenType.FUNCTION)
                {
                    this.BuildFunction(t);
                }
                else
                {
                    this.BuildBlock(t);
                }
            }
            else
            {
                Parser.PrintError("Unexpected end of program! No more tokens found!", this.token);
            }
            this.FinishBuild();
        }

        /// <summary>
        /// Builds function node
        /// </summary>
        /// <param name="token">Token representing function node</param>
        private void BuildFunction(Token token)
        {
            Function func = new Function(this.tokens, token, this);
            func.Build();
            if (this.tokens.HasNext())
            {
                Token next = this.tokens.GetNext();
                if (next.GetTokenType() == TokenType.FUNCTION)
                {
                    this.BuildFunction(next);
                }
                else if (next.GetTokenType() == TokenType.PROG_END)
                {
                    this.FinishBuild();
                }
                else
                {
                    this.BuildBlock(next);
                }
            }
            else
            {
                this.PrintUnexpectedEndOfProgram("mark of end of program, any command or function definition");
            }
        }

        /// <summary>
        /// Builds content of program
        /// </summary>
        /// <param name="token">Token representing first command in block</param>
        private void BuildBlock(Token token)
        {
            this.content = new ExecutableBlock(this.tokens, token, this);
            this.content.Build();
        }

        public void Execute()
        {
            this.Reset();
            if (this.content != null)
            {
                this.content.Execute();
            }
            else
            {
                Interpreter.Interpreter.PrintError("Execution of program failed: Nothing to execute!", this.token);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemestralProject.Lexer;
using SemestralProject.Parser.Nodes.Blocks;
using SemestralProject.Parser.Nodes.Expressions;

namespace SemestralProject.Parser.Nodes.Commands
{
    /// <summary>
    /// Class representing any general command
    /// </summary>
    class Command: Node, IExecutableNode
    {
        /// <summary>
        /// Node represetnting specific command itself
        /// </summary>
        IExecutableNode executable = null;

        /// <summary>
        /// Creates new command node
        /// </summary>
        /// <param name="tokens">Tokens from source code</param>
        /// <param name="token">Token representing node</param>
        /// <param name="parent">Parent block to which this block belongs to</param>
        public Command(TokenStream tokens, Token token, BlockNode block)
            : base(tokens, token, block) { }
        public override void Build()
        {
            this.StartBuild();
            if (this.token.GetTokenType() == TokenType.FLOAT ||
                this.token.GetTokenType() == TokenType.INTEGER ||
                this.token.GetTokenType() == TokenType.STRING)
            {
                Declaration declaration = new Declaration(this.tokens, this.token, this.block);
                declaration.Build();
                this.executable = declaration;
            }
            else if (this.token.GetTokenType() == TokenType.VARIABLE)
            {
                Assignment assignment = new Assignment(this.tokens, this.token, this.block);
                assignment.Build();
                this.executable = assignment;
            }
            else if (this.token.GetTokenType() == TokenType.WRITE)
            {
                Write write = new Write(this.tokens, this.token, this.block);
                write.Build();
                this.executable = write;
            }
            else if (this.token.GetTokenType() == TokenType.FUNCTION_IDENTIFIER)
            {
                Call call = new Call(this.tokens, this.token, this.block);
                call.Build();
                this.executable = call;
            }
            else if (this.token.GetTokenType() == TokenType.BREAK)
            {
                Break br = new Break(this.tokens, this.token, this.block);
                br.Build();
                this.executable = br;
            }
            if (this.tokens.HasNext())
            {
                Token next = this.tokens.GetNext();
                if (next.GetTokenType() != TokenType.SEMICOLON)
                {
                    this.PrintUnexpectedToken("semicolon", next);
                }
            }
            else
            {
                this.PrintUnexpectedEndOfProgram("semicolon");
            }
            this.FinishBuild();
        }

        public void Execute()
        {
            if (this.executable != null)
            {
                this.executable.Execute();
            }
            else
            {
                Interpreter.Interpreter.PrintError("Failed to execute " + this.ToString() + ": Nothing to execute!", this.token);
            }
        }
    }
}

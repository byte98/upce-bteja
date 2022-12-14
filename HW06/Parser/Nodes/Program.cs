using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HW06.Lexer;

namespace HW06.Parser.Nodes
{
    /// <summary>
    /// Class representing program node
    /// </summary>
    class Program : ExecutableBlockNode
    {
        /// <summary>
        /// Dot node
        /// </summary>
        private Dot dot;

        /// <summary>
        /// Block node
        /// </summary>
        private Block block;


        /// <summary>
        /// Creates new program node in AST
        /// </summary>
        /// <param name="tokens">Stream of tokens from lexer</param>
        /// <param name="token">Token representing node in AST</param>
        public Program(TokenStream tokens, Token token) : base(tokens, token, null)
        {
        }

        public override void Build()
        {
            Parser.Print("Building program...", this.token);
            if (this.tokens.HasNext())
            {
                if (this.tokens.ObserveNext().GetTokenType() == TokenType.DOT)
                {
                    this.BuildDot();
                }
                else
                {
                    this.BuildBlock();
                }
            }
            else
            {
                Parser.Print("Unexpected end of program!", null);
            }
        }

        /// <summary>
        /// Builds dot node
        /// </summary>
        private void BuildDot()
        {
            Token dotToken = this.tokens.GetNext();
            if (dotToken == null)
            {
                Parser.PrintError("Unexpected token during building end of program! Dot expected!", dotToken);
            }
            else
            {
                if (dotToken.GetTokenType() == TokenType.DOT)
                {
                    this.dot = new Dot(this.tokens, dotToken);
                    Parser.Print("Found end of program.", dotToken);
                }
                else
                {
                    Parser.PrintError("Unexpected token during building end of program! Dot expected!", dotToken);
                }
            }
            

        }

        /// <summary>
        /// Builds block node
        /// </summary>
        private void BuildBlock()
        {
            this.block = new Block(this.tokens, null, this);
            this.block.Build();
            this.BuildDot();
        }

        public override void Execute()
        {
            ExecutableNode exec = this.block.GetExecutable();
            if (exec != null)
            {
                exec.Execute();
            }
            else
            {
                Interpreter.PrintError("Cannot execute program (there is nothing to execute)!", this.token);
            }
        }
    }
}

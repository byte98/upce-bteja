using HW05.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW05.Parser.Nodes
{
    /// <summary>
    /// Class representing statement node in AST
    /// </summary>
    class Statement : ExecutableNode
    {
        /// <summary>
        /// Assignment operation
        /// </summary>
        private Assignment assignment;

        /// <summary>
        /// Begin node
        /// </summary>
        private Begin begin;

        /// <summary>
        /// While node
        /// </summary>
        private While nodeWhile;

        /// <summary>
        /// Call node
        /// </summary>
        private Call call;

        /// <summary>
        /// Print node
        /// </summary>
        private Print print;

        /// <summary>
        /// Read node
        /// </summary>
        private Read read;

        /// <summary>
        /// If node
        /// </summary>
        private If nodeIf;

        /// <summary>
        /// Creates new statement node in AST
        /// </summary>
        /// <param name="tokens">Stream of tokens from lexer</param>
        /// <param name="token">Token defining node in AST</param>
        /// <param name="block">Block to which node belongs to</param>
        public Statement(TokenStream tokens, Token token, BlockNode block) : base(tokens, token, block)
        {
        }

        public override void Build()
        {
            Parser.Print("Building statement#" + this.id + "...", this.token);
            if (this.token.GetTokenType() == TokenType.IDENT && this.token.HasValue())
            {
                this.BuildAssignment();
            }
            else if (this.token.GetTokenType() == TokenType.BEGIN)
            {
                this.BuildBegin();
            }
            else if (this.token.GetTokenType() == TokenType.WHILE)
            {
                this.BuildWhile();
            }
            else if (this.token.GetTokenType() == TokenType.CALL)
            {
                this.BuildCall();
            }
            else if (this.token.GetTokenType() == TokenType.EXCLAMATION)
            {
                this.BuildPrint();
            }
            else if (this.token.GetTokenType() == TokenType.QUESTION)
            {
                this.BuildRead();
            }
            else if (this.token.GetTokenType() == TokenType.IF)
            {
                this.BuildIf();
            }
            else
            {
                Parser.PrintError("Unexpected token during building statement! Expected identifier, begin, call, exclamation, question mark, if or while!", this.token);
            }
        }

        /// <summary>
        /// Builds assignment
        /// </summary>
        private void BuildAssignment()
        {
            this.assignment = new Assignment(this.tokens, this.token, this.block);
            this.assignment.Build();
        }

        /// <summary>
        /// Builds begin node
        /// </summary>
        private void BuildBegin()
        {
            this.begin = new Begin(this.tokens, this.token, this.block);
            this.begin.Build();
        }

        /// <summary>
        /// Builds while node
        /// </summary>
        private void BuildWhile()
        {
            this.nodeWhile = new While(this.tokens, this.token, this.block);
            this.nodeWhile.Build();
        }

        /// <summary>
        /// Builds call node
        /// </summary>
        private void BuildCall()
        {
            this.call = new Call(this.tokens, this.token, this.block);
            this.call.Build();
        }

        /// <summary>
        /// Builds print node
        /// </summary>
        private void BuildPrint()
        {
            this.print = new Print(this.tokens, this.token, this.block);
            this.print.Build();
        }

        /// <summary>
        /// Builds read node
        /// </summary>
        private void BuildRead()
        {
            this.read = new Read(this.tokens, this.token, this.block);
            this.read.Build();
        }

        /// <summary>
        /// Builds if node
        /// </summary>
        private void BuildIf()
        {
            this.nodeIf = new If(this.tokens, this.token, this.block);
            this.nodeIf.Build();
        }

        public override void Execute()
        {
            if (this.assignment != null) this.assignment.Execute();
            else if (this.begin != null) this.begin.Execute();
            else if (this.nodeWhile != null) this.nodeWhile.Execute();
            else if (this.call != null) this.call.Execute();
            else if (this.print != null) this.print.Execute();
        }
    }
}

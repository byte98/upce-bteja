using HW05.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW05.Parser.Nodes
{
    /// <summary>
    /// Class representing begin node in AST
    /// </summary>
    class Begin : ExecutableBlockNode
    {
        /// <summary>
        /// List of all statements stored to block
        /// </summary>
        private List<Statement> statements;

        /// <summary>
        /// Creates new begin node in AST
        /// </summary>
        /// <param name="tokens">Stream of tokens from lexer</param>
        /// <param name="token">Token defining begin node in AST</param>
        /// <param name="block">Parential block of code</param>
        public Begin(TokenStream tokens, Token token, BlockNode block) : base(tokens, token, block)
        {
            this.statements = new List<Statement>();
        }

        public override void Build()
        {
            Parser.Print("Building begin#" + this.id +"...", this.token);
            if (this.tokens.HasNext())
            {
                this.BuildBegin();
            }
            else
            {
                Parser.PrintError("Unexpected end of program during building begin#" + this.id + " block!", this.token);
            }
        }

        /// <summary>
        /// Builds begin block
        /// </summary>
        private void BuildBegin()
        {
            if (this.tokens.HasNext())
            {
                Statement s = new Statement(this.tokens, this.tokens.GetNext(), this);
                s.Build();
                this.statements.Add(s);
                if (this.tokens.HasNext())
                {
                    Token t = this.tokens.ObserveNext();
                    if (t.GetTokenType() == TokenType.END)
                    {
                        t = this.tokens.GetNext();
                        Parser.Print("Found end of begin#" + this.id, t);
                    }
                    else
                    {
                        this.BuildBegin();
                    }
                }
                else
                {
                    Parser.PrintError("Unexpected end of program during building begin#" + this.id + " block!", this.token);
                }
            }
        }

        public override void Execute()
        {
            foreach(Statement s in this.statements)
            {
                s.Execute();
            }
        }
    }
}

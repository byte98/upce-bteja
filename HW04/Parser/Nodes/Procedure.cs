using HW04.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW04.Parser.Nodes
{
    /// <summary>
    /// Class representing procedure node in AST
    /// </summary>
    class Procedure : BlockNode
    {
        /// <summary>
        /// Block of procedure
        /// </summary>
        private Block block;

        /// <summary>
        /// Creates new procedure node in AST
        /// </summary>
        /// <param name="tokens">Stream of tokens from lexer</param>
        /// <param name="token">Token defining procedure node in AST</param>
        /// <param name="block">Parential block of code</param>
        public Procedure(TokenStream tokens, Token token, BlockNode block) : base(tokens, token, block)
        {
        }

        public override void Build()
        {
            Parser.Print("Building procedure...", this.token);
            if (this.token.GetTokenType() == TokenType.IDENT && this.token.HasValue())
            {
                this.hasName = true;
                this.name = this.token.GetValue();
                if (this.tokens.HasNext())
                {
                    Token t = this.tokens.GetNext();
                    if (t.GetTokenType() == TokenType.SEMICOLON)
                    {
                        this.block = new Block(this.tokens, null, this.block);
                        this.block.Build();
                        this.InsertProcedure(this.name, this.block.GetExecutable());
                    }
                    else
                    {
                        Parser.PrintError("Unexpected token during building procedure! End or semicolon expected!", t);
                    }
                }
                else
                {
                    Parser.PrintError("Unexpected end of program during building procedure!", this.token);
                }
            }
            else
            {
                Parser.PrintError("Unexpected token during building procedure! Identifier expected!", this.token);
            }
        }

        /// <summary>
        /// Adds procedure to code block
        /// </summary>
        /// <param name="name">Name of procedure</param>
        /// <param name="procedure">Procedure itself</param>
        private void InsertProcedure(string name, ExecutableNode procedure)
        {
            CodeBlock block = this.codeBlock;
            if (block != null)
            {
                while (block.GetParent() != null)
                {
                    block = block.GetParent();
                }
            }            
            if (block.AddProcedure(name, procedure) == false)
            {
                Parser.PrintError("Cannot define procedure '" + name + "' (procedure already defined)!", this.token);
            }
        }
    }
}

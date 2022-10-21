using HW04.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW04.Parser.Nodes
{
    /// <summary>
    /// Class representing call node in AST
    /// </summary>
    class Call : ExecutableNode
    {
        /// <summary>
        /// Name of procedure which will be called
        /// </summary>
        private string name;

        /// <summary>
        /// Creates new call node in AST
        /// </summary>
        /// <param name="tokens">Stream of tokens from lexer</param>
        /// <param name="token">Token defining call node in AST</param>
        /// <param name="block">Block to which call node belongs to</param>
        public Call(TokenStream tokens, Token token, BlockNode block) : base(tokens, token, block)
        {
        }

        public override void Build()
        {
            Parser.Print("Building call...", this.token);
            if (this.token.GetTokenType() == TokenType.CALL)
            {
                if (this.tokens.HasNext())
                {
                    Token t = this.tokens.GetNext();
                    if (t.GetTokenType() == TokenType.IDENT && t.HasValue())
                    {
                        this.name = t.GetValue();
                        Parser.Print("Found call [" + this.name + "]", this.token);
                    }
                    else
                    {
                        Parser.PrintError("Unexpected token during building call! Identifier expected!", t);
                    }
                }
                else
                {
                    Parser.PrintError("Unexpected end of program during building call!", this.token);
                }
            }
            else
            {
                Parser.PrintError("Unexpected token during building call! Call expected!", this.token);
            }
        }

        public override void Execute()
        {
            ExecutableNode procedure = this.block.GetProcedure(this.name);
            if (this.block.HasProcedure(this.name) || procedure == null)
            {
                Parser.PrintError("Cannot execute procedure '" + this.name + "' (unknown procedure)!", this.token);
            }
            else
            {
                procedure.Execute();
            }
        }
    }
}

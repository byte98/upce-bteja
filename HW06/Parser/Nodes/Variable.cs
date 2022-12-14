using HW06.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW06.Parser.Nodes
{
    /// <summary>
    /// Class representing variable node in AST
    /// </summary>
    class Variable : Node
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokens">Stream of tokens from lexer</param>
        /// <param name="token">Token defining variable node in AST</param>
        public Variable(TokenStream tokens, Token token) : base(tokens, token)
        {
        }

        public override void Build()
        {
            Parser.Print("Building variable...", this.token);
            if (this.token.HasValue() && this.token.GetTokenType() == TokenType.IDENT)
            {
                this.name = this.token.GetValue();
                this.hasName = true;
                this.hasValue = true;
                this.valueEditable = true;
                Parser.Print("Found variable [" + this.name + "]", this.token);
                if (this.tokens.HasNext())
                {
                    Token t = this.tokens.GetNext();
                    if (t.GetTokenType() == TokenType.COLON)
                    {
                        if (this.tokens.HasNext())
                        {
                            t = this.tokens.GetNext();
                            if (t.GetTokenType() == TokenType.DT_NUMBER || t.GetTokenType() == TokenType.DT_STRING)
                            {
                                this.hasDefinition = true;
                                this.definition = t;
                            }
                            else
                            {
                                Parser.PrintError("Unexpected token during building variable! Data type expected!", this.token);
                            }
                        }
                        else
                        {
                            Parser.PrintError("Unexpected end of program during building variable! Data type expected!", this.token);
                        }
                    }
                    else
                    {
                        Parser.PrintError("Unexpected token during builidng variable! Colon expected!", t);
                    }
                }
                else
                {
                    Parser.PrintError("Unexpected end of program during building variable! Colon expected!", this.token);
                }
            }
            else
            {
                Parser.PrintError("Unexpected token during builidng variable! Identifier expected!", this.token);
            }
        }
    }
}

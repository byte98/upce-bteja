using HW04.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW04.Parser.Nodes
{
    /// <summary>
    /// Class representing constant in program
    /// </summary>
    class Constant : Node
    {
        /// <summary>
        /// Creates new constant in AST
        /// </summary>
        /// <param name="tokens">Stream of tokens from lexer</param>
        /// <param name="token">Token defining constant node in AST</param>
        public Constant(TokenStream tokens, Token token) : base(tokens, token)
        {
        }

        public override void Build()
        {
            Parser.Print("Building constant...", this.token);
            if (this.token.GetTokenType() == TokenType.IDENT && this.token.HasValue())
            {
                this.name = this.token.GetValue();
                if (this.tokens.HasNext())
                {
                    Token t = this.tokens.GetNext();
                    if (t.GetTokenType() == TokenType.EQUALS)
                    {
                        if (this.tokens.HasNext())
                        {
                            t = this.tokens.GetNext();
                            if (t.GetTokenType() == TokenType.NUMBER && t.HasValue())
                            { 
                                double val = double.NaN;
                                if (double.TryParse(t.GetValue(), out val))
                                {
                                    this.hasName = true;
                                    this.name = this.token.GetValue();
                                    this.hasValue = true;
                                    this.value = val;
                                    Parser.Print("Found constant [" + this.name + " = " + this.value + "]", t);
                                }
                                else
                                {
                                    Parser.PrintError("Unexpected value of constant! Number expected!", t);
                                }
                            }
                            else
                            {
                                Parser.PrintError("Unexpected token during building constant! Number expected!", t);
                            }
                        }
                        else
                        {
                            Parser.PrintError("Unexpected end of program during building constant!", this.token);
                        }
                    }
                    else
                    {
                        Parser.PrintError("Unexpected token during building constant! Equals expected!", t);
                    }
                }
                else
                {
                    Parser.PrintError("Unexpected end of program during building constant!", this.token);
                }
            }
            else
            {
                Parser.PrintError("Unexpected token during building constant! Identifier expected!", this.token);
            }
        }
    }
}

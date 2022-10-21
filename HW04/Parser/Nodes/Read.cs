using HW04.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW04.Parser.Nodes
{
    /// <summary>
    /// Class representing read node in AST
    /// </summary>
    class Read : ExecutableNode
    {
        /// <summary>
        /// Name of variable to which value will be saved into
        /// </summary>
        private string varName;

        /// <summary>
        /// Creates new read node in AST
        /// </summary>
        /// <param name="tokens">Stream of tokens from lexer</param>
        /// <param name="token">Token defining read node in AST</param>
        /// <param name="block">Block to which read node belongs to</param>
        public Read(TokenStream tokens, Token token, BlockNode block) : base(tokens, token, block)
        {
        }

        public override void Build()
        {
            Parser.Print("Building read...", this.token);
            if (this.token.GetTokenType() == TokenType.QUESTION)
            {
                if (this.tokens.HasNext())
                {
                    Token t = this.tokens.GetNext();
                    if (t.GetTokenType() == TokenType.IDENT && t.HasValue())
                    {
                        this.varName = t.GetValue();
                    }
                    else
                    {
                        Parser.PrintError("Unexpected token during building read! Question mark expected!", t);
                    }
                }
                else
                {
                    Parser.PrintError("Unexpected end of program during building read!", this.token);
                }
            }
            else
            {
                Parser.PrintError("Unexpected token during building read! Question mark expected!", this.token);
            }
        }

        public override void Execute()
        {
            if (this.varName != null)
            {
                if (this.block.HasVariable(this.varName))
                {
                    double val = double.NaN;
                    if (double.TryParse(Console.ReadLine(), out val))
                    {
                        this.block.SetVariable(this.varName, val);
                    }
                    else
                    {
                        Parser.PrintError("Cannot execute read (invalid value)! Expected number!", this.token);
                    }
                }
                else
                {
                    Parser.PrintError("Cannot execute read (unknown variable '" + this.varName + "')!", this.token);
                }
            }
            else
            {
                Parser.PrintError("Cannot execute read (variable not defined)!", this.token);
            }
        }
    }
}

using HW04.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW04.Parser.Nodes
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
                this.hasValue = true;
                this.valueEditable = true;
                Parser.Print("Found variable [" + this.name + "]", this.token);
            }
            else
            {
                Parser.PrintError("Unexpected token during builidng variable! Identifier expected!", this.token);
            }
        }
    }
}

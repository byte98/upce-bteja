using HW04.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW04.Parser.Nodes
{
    /// <summary>
    /// Class representing print node in AST
    /// </summary>
    class Print : ExecutableNode
    {
        /// <summary>
        /// Expression which will be printed
        /// </summary>
        private Expression expression;

        /// <summary>
        /// Creates new print node in AST
        /// </summary>
        /// <param name="tokens">Stream of tokens from lexer</param>
        /// <param name="token">Token defining print node in AST</param>
        /// <param name="block">Block to which print node belongs to</param>
        public Print(TokenStream tokens, Token token, BlockNode block) : base(tokens, token, block)
        {
        }

        public override void Build()
        {
            Parser.Print("Building print...", this.token);
            if (this.token.GetTokenType() == TokenType.EXCLAMATION)
            {
                if (this.tokens.HasNext())
                {
                    this.expression = new Expression(this.tokens, this.tokens.GetNext(), this.block);
                    this.expression.Build();
                }
                else
                {
                    Parser.PrintError("Unexpected end of program during building print!", this.token);
                }
            }
            else
            {
                Parser.PrintError("Unexpected token during building print! Exclamation expected!", this.token);
            }
        }

        public override void Execute()
        {
            this.expression.Execute();
            if (this.expression.HasValue())
            {
                Console.WriteLine(this.expression.GetValue());
            }
            else
            {
                Parser.PrintError("Cannot execute print (expression has no value)!", this.token);
            }
        }
    }
}

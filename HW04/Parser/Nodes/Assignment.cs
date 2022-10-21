using HW04.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW04.Parser.Nodes
{
    /// <summary>
    /// Class representing operation of assignment in AST
    /// </summary>
    class Assignment : ExecutableNode
    {
        /// <summary>
        /// Name of variable to which value will be assigned into
        /// </summary>
        private string varName;

        /// <summary>
        /// Expression which value will be assigned into variable
        /// </summary>
        private Expression expression;

        /// <summary>
        /// Creates new assignment node in AST
        /// </summary>
        /// <param name="tokens">Stream of tokens from lexer</param>
        /// <param name="token">Token defining node in AST</param>
        /// <param name="block">Block to which node belongs to</param>
        public Assignment(TokenStream tokens, Token token, BlockNode block) : base(tokens, token, block)
        {
        }

        public override void Build()
        {
            Parser.Print("Building assignment...", this.token);
            if (this.token.GetTokenType() == TokenType.IDENT && this.token.HasValue())
            {
                if (this.tokens.HasNext())
                {
                    Token t = this.tokens.GetNext();
                    if (t.GetTokenType() == TokenType.ASSIGNMENT)
                    {
                        if (this.tokens.HasNext())
                        {
                            this.BuildExpression();
                            this.varName = this.token.GetValue();
                        }
                        else
                        {
                            Parser.PrintError("Unexpected end of program during building assignment!", this.token);
                        }
                    }
                    else
                    {
                        Parser.PrintError("Unexpected token during building assignment! Assignment expected!", t);
                    }
                }
                else
                {
                    Parser.PrintError("Unexpected end of program during building assignment!", this.token);
                }
            }
            else
            {
                Parser.PrintError("Unexpected token during building assignment! Identifier expected!", this.token);
            }
        }

        /// <summary>
        /// Function which builds expression
        /// </summary>
        private void BuildExpression()
        {
            if(this.tokens.HasNext())
            {
                this.expression = new Expression(this.tokens, this.tokens.GetNext(), this.block);
                this.expression.Build();
            }
            else
            {
                Parser.PrintError("Unexpected end of program during building assignment expression!", this.token);
            }
        }

        public override void Execute()
        {
            this.expression.Execute();
            if (this.expression.HasValue())
            {
                if (this.block.HasVariable(this.varName))
                {
                    this.block.SetVariable(this.varName, this.expression.GetValue());
                }
                else
                {
                    Parser.PrintError("Unknown variable '" + this.varName + "' found in assignment execution!", this.token);
                }
            }
            else
            {
                Parser.PrintError("Failed to process expression for assignment!", this.token);
            }
        }
    }
}

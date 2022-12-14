using HW06.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW06.Parser.Nodes
{
    /// <summary>
    /// Class representing factor in AST
    /// </summary>
    class Factor : ExecutableNode
    {
        /// <summary>
        /// Data saved as factor is identifier
        /// </summary>
        private bool isIdentifier = false;

        /// <summary>
        /// Identifier saved as factor
        /// </summary>
        private string identifier;

        /// <summary>
        /// Data saved as factor is number
        /// </summary>
        private bool isNumber = false;

        /// <summary>
        /// Numeric value of factor
        /// </summary>
        private double number = double.NaN;

        /// <summary>
        /// Data saved as factor is expression
        /// </summary>
        private bool isExpression;

        /// <summary>
        /// Expression saved as factor
        /// </summary>
        private Expression expression;

        /// <summary>
        /// Creates new factor node in AST
        /// </summary>
        /// <param name="tokens">Stream of tokens from lexer</param>
        /// <param name="token">Token defining factor node in AST</param>
        /// <param name="block">Block to which factor node belongs to</param>
        public Factor(TokenStream tokens, Token token, BlockNode block) : base(tokens, token, block)
        {
        }

        public override void Build()
        {
            Parser.Print("Building factor...", this.token);
            if (this.token.GetTokenType() == TokenType.IDENT && this.token.HasValue())
            {
                this.isIdentifier = true;
                this.identifier = this.token.GetValue();
                Parser.Print("Found factor [identifier: " + this.identifier + "]", this.token);
            }
            else if (this.token.GetTokenType() == TokenType.NUMBER && this.token.HasValue())
            {
                double val = double.NaN;
                if (double.TryParse(this.token.GetValue(), out val))
                {
                    this.isNumber = true;
                    this.number = val;
                    Parser.Print("Found factor [value: " + this.number + "]", this.token);
                }
                else
                {
                    Parser.PrintError("Unexpected value of factor! Number expected!", this.token);
                }
            }
            else if (this.token.GetTokenType() == TokenType.OPEN_BRACKET)
            {
                Parser.Print("Found factor [expression]", this.token);
                this.BuildExpression();
            }
            else
            {
                Parser.PrintError("Unexpected token during building factor! Expected identifier, number or expression!", this.token);
            }

        }

        /// <summary>
        /// Builds expression
        /// </summary>
        private void BuildExpression()
        {
            if (this.tokens.HasNext())
            {
                this.isExpression = true;
                this.expression = new Expression(this.tokens, this.token, this.block);
                this.expression.Build();
                if (this.tokens.HasNext() && this.tokens.ObserveNext().GetTokenType() == TokenType.CLOSE_BRACKET) // If there is remaininh ')', move to next token
                {
                    this.tokens.GetNext();
                }
            }
            else
            {
                Parser.PrintError("Unexpected end of program during building factor expression!", this.token);
            }
        }

        public override void Execute()
        {
            if (this.isIdentifier)
            {
                if (this.block.HasVariable(this.identifier))
                {
                    if (this.block.GetBlock().HasDefinition(this.identifier) && this.block.GetBlock().GetDefinition(this.identifier).GetTokenType() == TokenType.DT_NUMBER)
                    {
                        this.hasValue = true;
                        this.value = this.block.GetVariable(this.identifier);
                    }
                    else
                    {
                        Interpreter.PrintError("Wrong data type of variable found in factor execution!", this.block.GetBlock().GetDefinition(this.identifier));
                    }
                    
                }
                else
                {
                    Interpreter.PrintError("Unknown variable '" + this.identifier + "' found in factor execution!", this.token);
                }
            }
            else if (this.isNumber)
            {
                this.hasValue = true;
                this.value = this.number;
            }
            else if (this.isExpression)
            {
                this.expression.Execute();
                if (this.expression.HasValue())
                {
                    this.hasValue = true;
                    this.value = this.expression.GetValue();
                }
                else
                {
                    Interpreter.PrintError("Cannot execute factor! Unknown expression value!", this.token);
                }
            }
            else
            {
                Interpreter.PrintError("Cannot execute factor! Unknown value!", this.token);
            }
        }
    }
}

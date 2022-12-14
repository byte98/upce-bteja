using HW06.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW06.Parser.Nodes
{
    /// <summary>
    /// Class representing while node in AST
    /// </summary>
    class While : ExecutableBlockNode
    {
        /// <summary>
        /// Condition controlling cycle
        /// </summary>
        private Condition condition;

        /// <summary>
        /// Statement done during cycle running
        /// </summary>
        private Statement statement;

        public While(TokenStream tokens, Token token, BlockNode block) : base(tokens, token, block)
        {
        }

        public override void Build()
        {
            Parser.Print("Building while...", this.token);
            if (this.token.GetTokenType() == TokenType.WHILE)
            {
                if (this.tokens.HasNext())
                {
                    Token t = this.tokens.GetNext();
                    this.condition = new Condition(this.tokens, t, this);
                    this.condition.Build();
                    if (this.tokens.HasNext())
                    {
                        t = tokens.GetNext();
                        if (t.GetTokenType() == TokenType.DO)
                        {
                            if (this.tokens.HasNext())
                            {
                                this.statement = new Statement(this.tokens, this.tokens.GetNext(), this);
                                this.statement.Build();
                            }
                            else
                            {
                                Parser.PrintError("Unexpected end of program during building while!", t);
                            }
                        }
                        else
                        {
                            Parser.PrintError("Unexpected token during building while! Expected do!", t);
                        }
                    }
                    else
                    {
                        Parser.PrintError("Unexpected end of program during building while!", t);
                    }
                }
                else
                {
                    Parser.PrintError("Unexpected end of program during building while!", this.token);
                }
            }
            else
            {
                Parser.PrintError("Unexpected token during building while! Expected while!", this.token);
            }
        }

        public override void Execute()
        {
            this.condition.Execute();
            if (this.condition.HasValue())
            {
                if (this.condition.GetValue() == Condition.TRUE)
                {
                    this.statement.Execute();
                    this.Execute();
                }
            }
            else
            {
                Interpreter.PrintError("Cannot execute while (condition execution failed)!", this.token);
            }
        }
    }
}

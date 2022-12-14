using HW05.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW05.Parser.Nodes
{
    /// <summary>
    /// Class representing if node in AST
    /// </summary>
    class If : ExecutableBlockNode
    {
        /// <summary>
        /// Condition controlling condition
        /// </summary>
        private Condition condition;

        /// <summary>
        /// Statement done for positive condition
        /// </summary>
        private Statement statement;

        public If(TokenStream tokens, Token token, BlockNode block) : base(tokens, token, block)
        {
        }

        public override void Build()
        {
            Parser.Print("Building if...", this.token);
            if (this.token.GetTokenType() == TokenType.IF)
            {
                if (this.tokens.HasNext())
                {
                    Token t = this.tokens.GetNext();
                    this.condition = new Condition(this.tokens, t, this);
                    this.condition.Build();
                    if (this.tokens.HasNext())
                    {
                        t = tokens.GetNext();
                        if (t.GetTokenType() == TokenType.THEN)
                        {
                            if (this.tokens.HasNext())
                            {
                                this.statement = new Statement(this.tokens, this.tokens.GetNext(), this);
                                this.statement.Build();
                            }
                            else
                            {
                                Parser.PrintError("Unexpected end of program during building if!", t);
                            }
                        }
                        else
                        {
                            Parser.PrintError("Unexpected token during building if! Expected then!", t);
                        }
                    }
                    else
                    {
                        Parser.PrintError("Unexpected end of program during building if!", t);
                    }
                }
                else
                {
                    Parser.PrintError("Unexpected end of program during building if!", this.token);
                }
            }
            else
            {
                Parser.PrintError("Unexpected token during building if! Expected if!", this.token);
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
                }
            }
            else
            {
                Interpreter.PrintError("Cannot execute if (condition execution failed)!", this.token);
            }
        }
    }
}

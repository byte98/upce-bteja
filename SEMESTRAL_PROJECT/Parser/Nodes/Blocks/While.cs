using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemestralProject.Lexer;
using SemestralProject.Parser.Nodes.Expressions;

namespace SemestralProject.Parser.Nodes.Blocks
{
    /// <summary>
    /// Class representing conditional cycle
    /// </summary>
    class While: ExecutableBlockNode
    {
        /// <summary>
        /// Predicate deciding, whether cylce will be repeated
        /// </summary>
        private IPredicate predicate;

        /// <summary>
        /// Body of cycle
        /// </summary>
        private ExecutableBlockNode body;


        /// <summary>
        /// Creates new generic conditional cycle
        /// </summary>
        /// <param name="tokens">Results of lexer</param>
        /// <param name="token">Token representing block</param>
        /// <param name="parent">Block to which this block belongs to</param>
        public While(Lexer.TokenStream tokens, Lexer.Token token, BlockNode parent)
            : base(tokens, token, parent) { }

        public override void Build()
        {
            this.StartBuild();
            if (this.token.GetTokenType() == Lexer.TokenType.WHILE)
            {
                if (this.tokens.HasNext())
                {
                    Token next = this.tokens.GetNext();
                    if (next.GetTokenType() == TokenType.OPEN_BRACKET)
                    {
                        if (this.tokens.HasNext())
                        {
                            Condition cond = new Condition(this.tokens, this.tokens.GetNext(), this);
                            cond.Build();
                            this.predicate = cond;
                            if (this.tokens.HasNext())
                            {
                                next = this.tokens.GetNext();
                                if (next.GetTokenType() == TokenType.CLOSE_BRACKET)
                                {
                                    if (this.tokens.HasNext())
                                    {
                                        next = this.tokens.GetNext();
                                        if (next.GetTokenType() == TokenType.OPEN_CURLY)
                                        {
                                            if (this.tokens.HasNext())
                                            {
                                                this.body = new ExecutableBlock(this.tokens, this.tokens.GetNext(), this);
                                                this.body.Build();
                                                if (this.tokens.HasNext())
                                                {
                                                    next = this.tokens.GetNext();
                                                    if (next.GetTokenType() != TokenType.CLOSE_CURLY)
                                                    {
                                                        this.PrintUnexpectedToken("close curly bracket", next);
                                                    }
                                                }
                                                else
                                                {
                                                    this.PrintUnexpectedEndOfProgram("close curly bracket", next);
                                                }
                                            }
                                            else
                                            {
                                                this.PrintUnexpectedEndOfProgram("any command", next);
                                            }
                                        }
                                        else
                                        {
                                            this.PrintUnexpectedToken("open curly bracket", next);
                                        }
                                    }
                                    else
                                    {
                                        this.PrintUnexpectedEndOfProgram("open curly bracket", next);
                                    }
                                }
                                else
                                {
                                    this.PrintUnexpectedToken("close bracket", next);
                                }
                            }
                            else
                            {
                                this.PrintUnexpectedEndOfProgram("close bracket");
                            }
                        }
                        else
                        {
                            this.PrintUnexpectedEndOfProgram("any condition", next);
                        }
                    }
                    else
                    {
                        this.PrintUnexpectedToken("open bracket", next);
                    }
                }
                else
                {
                    this.PrintUnexpectedEndOfProgram("open bracket");
                }
            }
            else
            {
                this.PrintUnexpectedToken("while", this.token);
            }
            this.FinishBuild();
        }

        public override void Execute()
        {
            this.Reset();
            this.predicate.Execute();
            while (this.predicate.IsTrue() && this.executing == true)
            {
                this.body.Execute();
                this.predicate.Execute();
            }
        }

        public override void Break()
        {
            this.executing = false;
        }

    }
}

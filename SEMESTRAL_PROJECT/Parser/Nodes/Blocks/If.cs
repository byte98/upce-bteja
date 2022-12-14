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
    /// Class representing conditional jump
    /// </summary>
    class If: ExecutableBlockNode
    {
        /// <summary>
        /// Predicate deciding, whether program will jump or not
        /// </summary>
        private IPredicate predicate;

        /// <summary>
        /// Body of conditional jump
        /// </summary>
        private ExecutableBlockNode body;

        /// <summary>
        /// Body which will be executed when not jumped
        /// </summary>
        private ExecutableBlockNode elseBody;

        /// <summary>
        /// Creates new generic conditional jump
        /// </summary>
        /// <param name="tokens">Results of lexer</param>
        /// <param name="token">Token representing block</param>
        /// <param name="parent">Block to which this block belongs to</param>
        public If(Lexer.TokenStream tokens, Lexer.Token token, BlockNode parent)
            : base(tokens, token, parent) { }

        public override void Build()
        {
            this.StartBuild();
            if (this.token.GetTokenType() == Lexer.TokenType.IF)
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
                                                    if (next.GetTokenType() == TokenType.CLOSE_CURLY)
                                                    {
                                                        if (this.tokens.HasNext() && this.tokens.ObserveNext().GetTokenType() == TokenType.ELSE)
                                                        {
                                                            next = this.tokens.GetNext();
                                                            if (this.tokens.HasNext())
                                                            {
                                                                next = this.tokens.GetNext();
                                                                if (next.GetTokenType() == TokenType.OPEN_CURLY)
                                                                {
                                                                    if (this.tokens.HasNext())
                                                                    {
                                                                        this.elseBody = new ExecutableBlock(this.tokens, this.tokens.GetNext(), this);
                                                                        this.elseBody.Build();
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
                                                    }
                                                    else
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
                this.PrintUnexpectedToken("if", this.token);
            }
            this.FinishBuild();
        }

        public override void Execute()
        {
            this.Reset();
            this.predicate.Execute();
            if (this.predicate.IsTrue())
            {
                this.body.Execute();
            }
            else if (this.elseBody != null)
            {
                this.elseBody.Execute();
            }
        }
    }
}

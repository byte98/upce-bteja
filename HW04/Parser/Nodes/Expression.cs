using HW04.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW04.Parser.Nodes
{
    /// <summary>
    /// Class representing expression in AST
    /// </summary>
    class Expression : ExecutableNode
    {
        /// <summary>
        /// Structure saving term with its sign
        /// </summary>
        private struct SignedTerm
        {
            /// <summary>
            /// Enumeration of all term signs
            /// </summary>
            public enum TermSign
            {
                /// <summary>
                /// Plus term sign
                /// </summary>
                PLUS,

                /// <summary>
                /// Minus term sign
                /// </summary>
                MINUS
            }

            /// <summary>
            /// Sign of term
            /// </summary>
            public TermSign sign { get; }

            /// <summary>
            /// Term itself
            /// </summary>
            public Term term { get; }

            /// <summary>
            /// Creates new signed term
            /// </summary>
            /// <param name="sign">Sign of term</param>
            /// <param name="term">Term itself</param>
            public SignedTerm(TermSign sign, Term term)
            {
                this.sign = sign;
                this.term = term;
            }
        }
        /// <summary>
        /// Terms stored in this expression
        /// </summary>
        private List<SignedTerm> terms;

        /// <summary>
        /// Creates new expression node in AST
        /// </summary>
        /// <param name="tokens">Stream of tokens from lexer</param>
        /// <param name="token">Token defining expression node in AST</param>
        /// <param name="block">Block to which expression node belongs to</param>
        public Expression(TokenStream tokens, Token token, BlockNode block) : base(tokens, token, block)
        {
            this.terms = new List<SignedTerm>();
        }

        public override void Build()
        {
            Parser.Print("Building expression...", this.token);
            if (this.token.GetTokenType() == TokenType.PLUS)
            {
                if (this.tokens.HasNext())
                {
                    Term t = new Term(this.tokens, this.tokens.GetNext(), this.block);
                    t.Build();
                    this.terms.Add(new SignedTerm(SignedTerm.TermSign.PLUS, t));
                }
                else
                {
                    Parser.PrintError("Unexpected end of program during building expression!", this.token);
                }
            }
            else if (this.token.GetTokenType() == TokenType.MINUS)
            {
                if (this.tokens.HasNext())
                {
                    Term t = new Term(this.tokens, this.tokens.GetNext(), this.block);
                    t.Build();
                    this.terms.Add(new SignedTerm(SignedTerm.TermSign.MINUS, t));
                }
                else
                {
                    Parser.PrintError("Unexpected end of program during building expression!", this.token);
                }
            }
            else
            {
                Term t = new Term(this.tokens, this.token, this.block);
                t.Build();
                this.terms.Add(new SignedTerm(SignedTerm.TermSign.PLUS, t));
            }
            if (this.tokens.HasNext() && (this.tokens.ObserveNext().GetTokenType() == TokenType.PLUS || this.tokens.ObserveNext().GetTokenType() == TokenType.MINUS))
            {
                this.BuildTerm();
            }
        }

        /// <summary>
        /// Builds term
        /// </summary>
        private void BuildTerm()
        {
            Token token = this.tokens.GetNext();
            if (token.GetTokenType() == TokenType.PLUS)
            {
                if (this.tokens.HasNext())
                {
                    Term t = new Term(this.tokens, this.tokens.GetNext(), this.block);
                    this.terms.Add(new SignedTerm(SignedTerm.TermSign.PLUS, t));
                    if (this.tokens.HasNext() && (this.tokens.ObserveNext().GetTokenType() == TokenType.PLUS || this.tokens.ObserveNext().GetTokenType() == TokenType.MINUS))
                    {
                        this.BuildTerm();
                    }
                }
                else
                {
                    Parser.PrintError("Unexpected end of program during building expression term!", token);
                }
            }
            else if (token.GetTokenType() == TokenType.MINUS)
            {
                if (this.tokens.HasNext())
                {
                    Term t = new Term(this.tokens, this.tokens.GetNext(), this.block);
                    this.terms.Add(new SignedTerm(SignedTerm.TermSign.MINUS, t));
                    if (this.tokens.HasNext() && (this.tokens.ObserveNext().GetTokenType() == TokenType.PLUS || this.tokens.ObserveNext().GetTokenType() == TokenType.MINUS))
                    {
                        this.BuildTerm();
                    }
                }
                else
                {
                    Parser.PrintError("Unexpected end of program during building expression term!", token);
                }
            }
            else
            {
                Parser.PrintError("Unexpected token during building expression term! Expected plus or minus!", token);
            }
        }

        public override void Execute()
        {
            if (this.terms.Count > 0)
            {
                double val = 0;
                foreach(SignedTerm t in this.terms)
                {
                    t.term.Execute();
                    if (t.term.HasValue())
                    {
                        if (t.sign == SignedTerm.TermSign.PLUS)
                        {
                            val = val + t.term.GetValue();
                            this.hasValue = true;
                        }
                        else if (t.sign == SignedTerm.TermSign.MINUS)
                        {
                            val = val - t.term.GetValue();
                            this.hasValue = true;
                        }
                        else
                        {
                            Parser.PrintError("Cannot execute expression (term sign not defined)!", this.token);
                        }
                    }
                    else
                    {
                        Parser.PrintError("Cannot execute expression (term execution failed)!", this.token);
                    }
                }
                if (this.hasValue)
                {
                    this.value = val;
                }
            }
            else
            {
                Parser.PrintError("Cannot execute expression (terms not defined)!", this.token);
            }
        }
    }
}
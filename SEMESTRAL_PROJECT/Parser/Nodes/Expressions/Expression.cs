using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemestralProject.Lexer;

namespace SemestralProject.Parser.Nodes.Expressions
{
    /// <summary>
    /// Class representing expression
    /// </summary>
    class Expression: Node, IValueNode
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
            public TermSign sign { get; set; }

            /// <summary>
            /// Term itself
            /// </summary>
            public Term term { get; set; }

            /// <summary>
            /// Creates new signed term
            /// </summary>
            /// <param name="sign">Sign of term</param>
            /// <param name="term">Term itself</param>
            public SignedTerm(TermSign sign, Term term) : this()
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
        /// First term with no defined sign
        /// </summary>
        private Term term;

        /// <summary>
        /// Value of expression
        /// </summary>
        private Value value;

        /// <summary>
        /// Creates new expression
        /// </summary>
        /// <param name="tokens">Tokens from source code</param>
        /// <param name="token">Token representing node</param>
        /// <param name="parent">Parent block to which this block belongs to</param>
        public Expression(TokenStream tokens, Token token, BlockNode block)
            : base(tokens, token, block) { }

        public override void Build()
        {
            this.StartBuild();
            this.term = new Term(this.tokens, this.token, this.block);
            this.terms = new List<SignedTerm>();
            this.term.Build();
            this.BuildContent();
            this.FinishBuild();
        }

        /// <summary>
        /// Builds content of expression
        /// </summary>
        private void BuildContent()
        {
            if (this.tokens.HasNext())
            {
                Token next = this.tokens.ObserveNext();
                if (next.GetTokenType() == TokenType.PLUS)
                {
                    this.tokens.GetNext();
                    if (this.tokens.HasNext())
                    {
                        Term term = new Term(this.tokens, this.tokens.GetNext(), this.block);
                        term.Build();
                        this.terms.Add(new SignedTerm(SignedTerm.TermSign.PLUS, term));
                        this.BuildContent();
                    }
                    else
                    {
                        this.PrintUnexpectedEndOfProgram("any term");
                    }
                }
                else if (next.GetTokenType() == TokenType.MINUS)
                {
                    this.tokens.GetNext();
                    if (this.tokens.HasNext())
                    {
                        Term term = new Term(this.tokens, this.tokens.GetNext(), this.block);
                        term.Build();
                        this.terms.Add(new SignedTerm(SignedTerm.TermSign.MINUS, term));
                        this.BuildContent();
                    }
                    else
                    {
                        this.PrintUnexpectedEndOfProgram("any term");
                    }
                }
            }
        }

        public void Execute()
        {
            this.term.Execute();
            if (this.term.HasValue())
            {
                this.value = this.term.GetValue();
                foreach (SignedTerm term in this.terms)
                {
                    term.term.Execute();
                    if (term.term.HasValue())
                    {
                        if (term.sign == SignedTerm.TermSign.PLUS)
                        {
                            this.value = this.value + term.term.GetValue();
                        }
                        else if (term.sign == SignedTerm.TermSign.MINUS)
                        {
                            this.value = this.value - term.term.GetValue();
                        }
                    }
                    else
                    {
                        Interpreter.Interpreter.PrintError("Execution of expression failed: term has no value!", this.token);
                    }
                }
            }
            else
            {
                Interpreter.Interpreter.PrintError("Execution of expression failed: term has no value!", this.token);
            }
        }

        public Value GetValue()
        {
            return this.value;
        }

        public bool HasValue()
        {
            return this.value != null;
        }
    }
}

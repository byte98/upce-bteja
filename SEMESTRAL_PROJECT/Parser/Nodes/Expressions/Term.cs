using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemestralProject.Lexer;

namespace SemestralProject.Parser.Nodes.Expressions
{
    /// <summary>
    /// Class term used in expressions
    /// </summary>
    class Term: Node, IValueNode
    {
        /// <summary>
        /// Stores factor with its sign
        /// </summary>
        private struct SignedFactor
        {
            /// <summary>
            /// Sing connected with factor
            /// </summary>
            public enum FactorSign
            {
                /// <summary>
                /// Factor will be multiplied
                /// </summary>
                MULTIPLY,

                /// <summary>
                /// Factor will be divided
                /// </summary>
                DIVIDE
            }

            /// <summary>
            /// Sign of factor
            /// </summary>
            public FactorSign sign { get; set; }

            /// <summary>
            /// Factor itself
            /// </summary>
            public Factor factor { get; set; }

            /// <summary>
            /// Creates new factor with its sign
            /// </summary>
            /// <param name="sign">Sign of factor</param>
            /// <param name="factor">Factor itself</param>
            public SignedFactor(FactorSign sign, Factor factor) : this()
            {
                this.sign = sign;
                this.factor = factor;
            }
        }

        /// <summary>
        /// List of factors with its signs
        /// </summary>
        private List<SignedFactor> factors;

        /// <summary>
        /// First factor with no defined sign
        /// </summary>
        private Factor factor;

        /// <summary>
        /// Value of term
        /// </summary>
        private Value value;

        /// <summary>
        /// Creates new term
        /// </summary>
        /// <param name="tokens">Tokens from source code</param>
        /// <param name="token">Token representing node</param>
        /// <param name="parent">Parent block to which this block belongs to</param>
        public Term(TokenStream tokens, Token token, BlockNode block)
            : base(tokens, token, block)
        {
            this.factors = new List<SignedFactor>();
        }

        public override void Build()
        {
            this.StartBuild();
            this.factor = new Factor(this.tokens, this.token, this.block);
            this.factor.Build();
            this.BuildContent();
            this.FinishBuild();
        }

        /// <summary>
        /// Builds content of term
        /// </summary>
        private void BuildContent()
        {
            if (this.tokens.HasNext())
            {
                Token token = this.tokens.ObserveNext();
                if (token.GetTokenType() == TokenType.MULTIPLY)
                {
                    this.tokens.GetNext();
                    if (this.tokens.HasNext())
                    {
                        Factor f = new Factor(this.tokens, this.tokens.GetNext(), this.block);
                        f.Build();
                        this.factors.Add(new SignedFactor(SignedFactor.FactorSign.MULTIPLY, f));
                        this.BuildContent();
                    }
                    else
                    {
                        this.PrintUnexpectedEndOfProgram("any factor");
                    }
                }
                else if (token.GetTokenType() == TokenType.DIVIDE)
                {
                    this.tokens.GetNext();
                    if (this.tokens.HasNext())
                    {
                        Factor f = new Factor(this.tokens, this.tokens.GetNext(), this.block);
                        f.Build();
                        this.factors.Add(new SignedFactor(SignedFactor.FactorSign.DIVIDE, f));
                        this.BuildContent();
                    }
                    else
                    {
                        this.PrintUnexpectedEndOfProgram("any factor");
                    }
                }
            }
        }

        public void Execute()
        {
            this.factor.Execute();
            if (this.factor.HasValue())
            {
                this.value = this.factor.GetValue();
                foreach(SignedFactor f in this.factors)
                {
                    f.factor.Execute();
                    if (f.factor.HasValue())
                    {
                        if (f.sign == SignedFactor.FactorSign.MULTIPLY)
                        {
                            this.value = this.value * f.factor.GetValue();
                        }
                        else if (f.sign == SignedFactor.FactorSign.DIVIDE)
                        {
                            this.value = this.value / f.factor.GetValue();
                        }
                    }
                    else
                    {
                        Interpreter.Interpreter.PrintError("Execution of term failed: factor has no value!", this.token);
                        break;
                    }
                }
            }
            else
            {
                Interpreter.Interpreter.PrintError("Execution of term failed: factor has no value!", this.token);
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

using HW05.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW05.Parser.Nodes
{
    /// <summary>
    /// Class representing term node in AST
    /// </summary>
    class Term : ExecutableNode
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

            public SignedFactor(FactorSign sign, Factor factor): this()
            {
                this.sign = sign;
                this.factor = factor;
            }
        }

        /// <summary>
        /// Factor storing value of term
        /// </summary>
        private Factor factor;

        /// <summary>
        /// List of all factors which will be evaluated
        /// </summary>
        private List<SignedFactor> factors;

        /// <summary>
        /// Creates new term node in AST
        /// </summary>
        /// <param name="tokens">Stream of tokens from lexer</param>
        /// <param name="token">Token defining term node in AST</param>
        /// <param name="block">Block to which term node belongs to</param>
        public Term(TokenStream tokens, Token token, BlockNode block) : base(tokens, token, block)
        {
            this.factors = new List<SignedFactor>();
        }

        public override void Build()
        {
            Parser.Print("Building term...", this.token);
            this.factor = new Factor(this.tokens, this.token, this.block);
            this.factor.Build();
            if (this.tokens.HasNext())
            {
                Token t = this.tokens.ObserveNext();
                if (t.GetTokenType() == TokenType.DIVIDE || t.GetTokenType() == TokenType.MULTIPLY)
                {
                    this.BuildFactor();
                }
            }
        }

        /// <summary>
        /// Builds factor
        /// </summary>
        private void BuildFactor()
        {
            Token t = this.tokens.GetNext();
            SignedFactor.FactorSign sign = SignedFactor.FactorSign.MULTIPLY;
            bool signDefined = false;
            if (t.GetTokenType() == TokenType.DIVIDE)
            {
                sign = SignedFactor.FactorSign.DIVIDE;
                signDefined = true;
            }
            else if (t.GetTokenType() == TokenType.MULTIPLY)
            {
                sign = SignedFactor.FactorSign.MULTIPLY;
                signDefined = true;
            }
            else
            {
                Parser.PrintError("Unexpected token during building term factors! Multiply or divide expected!", t);
            }
            if (signDefined)
            {
                if (this.tokens.HasNext())
                {
                    t = this.tokens.GetNext();
                    Factor f = new Factor(this.tokens, t, this.block);
                    f.Build();
                    this.factors.Add(new SignedFactor(sign, f));
                    if (this.tokens.HasNext())
                    {
                        t = this.tokens.ObserveNext();
                        if (t.GetTokenType() == TokenType.MULTIPLY || t.GetTokenType() == TokenType.DIVIDE)
                        {
                            this.BuildFactor();
                        }
                    }
                }
                else
                {
                    Parser.PrintError("Unexpected end of program during building term factors!", t);
                }
            }
        }

        public override void Execute()
        {
            if (this.factor != null)
            {
                this.factor.Execute();
                if (this.factor.HasValue())
                {
                    double val = this.factor.GetValue();
                    if (this.factors.Count > 0)
                    {
                        foreach(SignedFactor f in this.factors)
                        {
                            f.factor.Execute();
                            if (f.factor.HasValue())
                            {
                                if (f.sign == SignedFactor.FactorSign.MULTIPLY)
                                {
                                    val = val * f.factor.GetValue();
                                }
                                else if (f.sign == SignedFactor.FactorSign.DIVIDE)
                                {
                                    if (f.factor.GetValue() != 0)
                                    {
                                        val = val / f.factor.GetValue();
                                    }
                                    else
                                    {
                                        Interpreter.PrintError("Cannot execute term (division by factor with zero value)!", this.token);
                                    }
                                }
                                else
                                {
                                    Interpreter.PrintError("Cannot execute term (unknown factor sign)!", this.token);
                                }
                            }
                            else
                            {
                                Interpreter.PrintError("Cannot execute term (factor has no value)!", this.token);
                            }
                        }
                    }
                    this.hasValue = true;
                    this.value = val;
                }
                else
                {
                    Interpreter.PrintError("Cannot execute term (factor has no value)!", this.token);
                }
            }
            else
            {
                Interpreter.PrintError("Cannot execute term (factor not defined)!", this.token);
            }
        }
    }
}

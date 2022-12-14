using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemestralProject.Lexer;

namespace SemestralProject.Parser.Nodes.Expressions
{
    /// <summary>
    /// Class representign some kind of a condition
    /// </summary>
    class Condition: Node, IPredicate
    {
        /// <summary>
        /// Left side of condition
        /// </summary>
        private Expression leftSide;

        /// <summary>
        /// Right side of expression
        /// </summary>
        private Expression rightSide;

        /// <summary>
        /// Enumeration of all available relational operators
        /// </summary>
        private enum ERelations
        {
            /// <summary>
            /// Left side equals to right side
            /// </summary>
            EQUALS,

            /// <summary>
            /// Left side does not equals to right side
            /// </summary>
            NOT_EQUALS,

            /// <summary>
            /// Left side is greater than right side
            /// </summary>
            GREATER,

            /// <summary>
            /// Left side is greater or equals to right side
            /// </summary>
            GREATER_EQUALS,

            /// <summary>
            /// Left side is lower than right side
            /// </summary>
            LOWER,

            /// <summary>
            /// Left side is lower or equals to right side
            /// </summary>
            LOWER_EQUALS
        }

        /// <summary>
        /// Relation of sides in this condition
        /// </summary>
        private ERelations relation;

        /// <summary>
        /// Creates new condition
        /// </summary>
        /// <param name="tokens">Tokens from source code</param>
        /// <param name="token">Token representing node</param>
        /// <param name="parent">Parent block to which this block belongs to</param>
        public Condition(TokenStream tokens, Token token, BlockNode block)
            : base(tokens, token, block) { }

        public override void Build()
        {
            this.StartBuild();
            this.leftSide = new Expression(this.tokens, this.token, this.block);
            this.leftSide.Build();
            if (this.tokens.HasNext())
            {
                Token next = this.tokens.GetNext();
                if (next.GetTokenType() == TokenType.EQUALS)
                {
                    this.relation = ERelations.EQUALS;
                }
                else if (next.GetTokenType() == TokenType.NOT_EQUALS)
                {
                    this.relation = ERelations.NOT_EQUALS;
                }
                else if (next.GetTokenType() == TokenType.GREATER)
                {
                    this.relation = ERelations.GREATER;
                }
                else if (next.GetTokenType() == TokenType.GREATER_EQUALS)
                {
                    this.relation = ERelations.GREATER_EQUALS;
                }
                else if (next.GetTokenType() == TokenType.LOWER)
                {
                    this.relation = ERelations.LOWER;
                }
                else if (next.GetTokenType() == TokenType.LOWER_EQUALS)
                {
                    this.relation = ERelations.LOWER_EQUALS;
                }
                else
                {
                    this.PrintUnexpectedToken("==, !=, <, <=, > or >=", next);
                }
                if (this.tokens.HasNext())
                {
                    this.rightSide = new Expression(this.tokens, this.tokens.GetNext(), this.block);
                    this.rightSide.Build();
                }
                else
                {
                    this.PrintUnexpectedEndOfProgram("any expression", next);
                }
            }
            else
            {
                this.PrintUnexpectedEndOfProgram("==, !=, <, <=, > or >=");
            }
            this.FinishBuild();
        }

        public void Execute()
        {
            this.leftSide.Execute();
            this.rightSide.Execute();
        }

        public bool IsTrue()
        {
            bool reti = false;
            if (this.leftSide.HasValue() == false)
            {
                Interpreter.Interpreter.PrintError("Cannot decide whether predicate is true: left side has no value!", this.token);
            }
            else if (this.rightSide.HasValue() == false)
            {
                Interpreter.Interpreter.PrintError("Cannot decide whether predicate is true: left side has no value!", this.token);
            }
            else if (this.leftSide.GetValue().GetDataType() != this.rightSide.GetValue().GetDataType())
            {
                Interpreter.Interpreter.PrintError("Cannot decide whether predicate is true: incompatible data types (" + this.leftSide.GetValue().GetDataType().ToString() + " and " + this.leftSide.GetValue().GetDataType().ToString() + ")!", this.token);
            }
            else
            {
                Value left = this.leftSide.GetValue();
                Value right = this.rightSide.GetValue();
                if (left.GetDataType() == EDataType.INTEGER)
                {
                    if (this.relation == ERelations.EQUALS)
                    {
                        reti = left.GetInt() == right.GetInt();
                    }
                    else if (this.relation == ERelations.NOT_EQUALS)
                    {
                        reti = left.GetInt() != right.GetInt();
                    }
                    else if (this.relation == ERelations.GREATER)
                    {
                        reti = left.GetInt() > right.GetInt();
                    }
                    else if (this.relation == ERelations.GREATER_EQUALS)
                    {
                        reti = left.GetInt() >= right.GetInt();
                    }
                    else if (this.relation == ERelations.LOWER)
                    {
                        reti = left.GetInt() < right.GetInt();
                    }
                    else if (this.relation == ERelations.LOWER_EQUALS)
                    {
                        reti = left.GetInt() <= right.GetInt();
                    }
                }
                else if (left.GetDataType() == EDataType.FLOAT)
                {
                    if (this.relation == ERelations.EQUALS)
                    {
                        reti = left.GetFloat() == right.GetFloat();
                    }
                    else if (this.relation == ERelations.NOT_EQUALS)
                    {
                        reti = left.GetFloat() != right.GetFloat();
                    }
                    else if (this.relation == ERelations.GREATER)
                    {
                        reti = left.GetFloat() > right.GetFloat();
                    }
                    else if (this.relation == ERelations.GREATER_EQUALS)
                    {
                        reti = left.GetFloat() >= right.GetFloat();
                    }
                    else if (this.relation == ERelations.LOWER)
                    {
                        reti = left.GetFloat() < right.GetFloat();
                    }
                    else if (this.relation == ERelations.LOWER_EQUALS)
                    {
                        reti = left.GetFloat() <= right.GetFloat();
                    }
                }
                else if (left.GetDataType() == EDataType.STRING)
                {
                    if (this.relation == ERelations.EQUALS)
                    {
                        reti = left.GetString() == right.GetString();
                    }
                    else if (this.relation == ERelations.NOT_EQUALS)
                    {
                        reti = left.GetString() != right.GetString();
                    }
                    else if (this.relation == ERelations.GREATER)
                    {
                        reti = string.Compare(left.GetString(), right.GetString()) > 0;
                    }
                    else if (this.relation == ERelations.GREATER_EQUALS)
                    {
                        reti = string.Compare(left.GetString(), right.GetString()) >= 0;
                    }
                    else if (this.relation == ERelations.LOWER)
                    {
                        reti = string.Compare(left.GetString(), right.GetString()) < 0;
                    }
                    else if (this.relation == ERelations.LOWER_EQUALS)
                    {
                        reti = string.Compare(left.GetString(), right.GetString()) <= 0;
                    }
                }
            }
            return reti;
        }
    }
}

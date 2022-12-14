using HW06.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW06.Parser.Nodes
{
    /// <summary>
    /// Class representing condition node in AST
    /// </summary>
    class Condition : ExecutableNode
    {
        /// <summary>
        /// Value for true condition
        /// </summary>
        public static double TRUE = 0;

        /// <summary>
        /// Value for false condition
        /// </summary>
        public static double FALSE = 1;

        /// <summary>
        /// Expression on left side of condition
        /// </summary>
        private Expression leftExpression;

        /// <summary>
        /// Expression on right side of condition
        /// </summary>
        private Expression rightExpression;

        /// <summary>
        /// Enumeration of all available condition types
        /// </summary>
        private enum ConditionType
        {
            /// <summary>
            /// Boths sides equal
            /// </summary>
            EQUAL,

            /// <summary>
            /// Sides does not equal to each other
            /// </summary>
            NOT_EQUAL,

            /// <summary>
            /// One side is lower than other one
            /// </summary>
            LOWER,

            /// <summary>
            /// One side is lower or equals to other one
            /// </summary>
            LOWER_EQUAL,

            /// <summary>
            /// One side is greater than other one
            /// </summary>
            GREATER,

            /// <summary>
            /// One side is greater or equals to other one
            /// </summary>
            GREATER_EQUAL
        }

        /// <summary>
        /// Type of condition
        /// </summary>
        private ConditionType type;

        /// <summary>
        /// Flag, whether condition is type of 'is odd'
        /// </summary>
        private bool isOdd = false;

        /// <summary>
        /// Creates new condition in AST
        /// </summary>
        /// <param name="tokens">Stream of tokens from lexer</param>
        /// <param name="token">Token defining condition node in AST</param>
        /// <param name="block">Block to which condition node belongs to</param>
        public Condition(TokenStream tokens, Token token, BlockNode block) : base(tokens, token, block)
        {
        }

        public override void Build()
        {
            Parser.Print("Building condition...", this.token);
            if (this.token.GetTokenType() == TokenType.ODD)
            {
                this.isOdd = true;
                if (this.tokens.HasNext())
                {
                    this.rightExpression = new Expression(this.tokens, this.tokens.GetNext(), this.block);
                    this.rightExpression.Build();
                }
                else
                {
                    Parser.PrintError("Unexpected end of program during building condition!", this.token);
                }
            }
            else
            { 
                this.leftExpression = new Expression(this.tokens, this.token, this.block);
                this.leftExpression.Build();
                if (this.tokens.HasNext())
                {
                    Token t = this.tokens.GetNext();
                    bool typeSet = false;
                    if (t.GetTokenType() == TokenType.EQUALS)
                    {
                        this.type = ConditionType.EQUAL;
                        typeSet = true;
                    }
                    else if (t.GetTokenType() == TokenType.HASH)
                    {
                        this.type = ConditionType.NOT_EQUAL;
                        typeSet = true;
                    }
                    else if (t.GetTokenType() == TokenType.LOWER)
                    {
                        this.type = ConditionType.LOWER;
                        typeSet = true;
                    }
                    else if (t.GetTokenType() == TokenType.LOWER_EQUAL)
                    {
                        this.type = ConditionType.LOWER_EQUAL;
                        typeSet = true;
                    }
                    else if (t.GetTokenType() == TokenType.GREATER)
                    {
                        this.type = ConditionType.GREATER;
                        typeSet = true;
                    }
                    else if (t.GetTokenType() == TokenType.GREATER_EQUAL)
                    {
                        this.type = ConditionType.GREATER_EQUAL;
                        typeSet = true;
                    }
                    else
                    {
                        Parser.PrintError("Unexpected token during building condition! Equal, hash, lower, lower or equal, greater, greater or equal expected!", t);
                    }
                    if (typeSet)
                    {
                        if (this.tokens.HasNext())
                        {
                            this.rightExpression = new Expression(this.tokens, this.tokens.GetNext(), this.block);
                            this.rightExpression.Build();
                        }
                        else
                        {
                            Parser.PrintError("Unexpected end of program during building condition!", t);
                        }
                    }
                    
                }
                else
                {
                    Parser.PrintError("Unexpected end of program during building condition!", this.token);
                }
            }
        }

        public override void Execute()
        {
            if (this.isOdd)
            {
                this.rightExpression.Execute();
                if (this.rightExpression.HasValue())
                {
                    this.hasValue = true;
                    this.value = this.rightExpression.GetValue() % 2 == 1 ? Condition.TRUE : Condition.FALSE;
                }
                else
                {
                    Interpreter.PrintError("Cannot execute condition (right side has no value)!", this.token);
                }
            }
            else
            {
                this.leftExpression.Execute();
                if (this.leftExpression.HasValue())
                {
                    this.rightExpression.Execute();
                    if (this.rightExpression.HasValue())
                    {

                        this.hasValue = true;
                        switch (this.type)
                        {
                            case ConditionType.EQUAL: this.value = this.leftExpression.GetValue() == this.rightExpression.GetValue() ? Condition.TRUE : Condition.FALSE; break;
                            case ConditionType.NOT_EQUAL: this.value = this.leftExpression.GetValue() != this.rightExpression.GetValue() ? Condition.TRUE : Condition.FALSE; break;
                            case ConditionType.LOWER: this.value = this.leftExpression.GetValue() < this.rightExpression.GetValue() ? Condition.TRUE : Condition.FALSE; break;
                            case ConditionType.LOWER_EQUAL: this.value = this.leftExpression.GetValue() <= this.rightExpression.GetValue() ? Condition.TRUE : Condition.FALSE; break;
                            case ConditionType.GREATER: this.value = this.leftExpression.GetValue() > this.rightExpression.GetValue() ? Condition.TRUE : Condition.FALSE; break;
                            case ConditionType.GREATER_EQUAL: this.value = this.leftExpression.GetValue() >= this.rightExpression.GetValue() ? Condition.TRUE : Condition.FALSE; break;
                        }
                    }
                    else
                    {
                        Interpreter.PrintError("Cannot execute condition (right side has no value)!", this.token);
                    }
                }
                else
                {
                    Interpreter.PrintError("Cannot execute condition (left side has no value)!", this.token);
                }
            }
        }
    }
}

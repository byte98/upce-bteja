using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemestralProject.Lexer;

namespace SemestralProject.Parser.Nodes.Expressions
{
    /// <summary>
    /// Class representing call of the function
    /// </summary>
    class Call: Node, IValueNode
    {
        /// <summary>
        /// Name of function which will be called
        /// </summary>
        private string function;

        /// <summary>
        /// Arguments passed to function
        /// </summary>
        private List<Expression> arguments;

        /// <summary>
        /// Return value of function call
        /// </summary>
        private Value value;

        /// <summary>
        /// Creates new command node
        /// </summary>
        /// <param name="tokens">Tokens from source code</param>
        /// <param name="token">Token representing node</param>
        /// <param name="block">Parent block to which this block belongs to</param>
        public Call(TokenStream tokens, Token token, BlockNode block)
            : base(tokens, token, block)
        {
            this.arguments = new List<Expression>();
        }

        public override void Build()
        {
            this.StartBuild();
            if (this.token.GetTokenType() == TokenType.FUNCTION_IDENTIFIER)
            {
                if (this.token.HasValue())
                {
                    this.function = this.token.GetValue();
                    if (this.tokens.HasNext())
                    {
                        Token next = this.tokens.GetNext();
                        if (next.GetTokenType() == TokenType.OPEN_BRACKET)
                        {
                            if (this.tokens.HasNext())
                            {
                                next = this.tokens.ObserveNext();
                                if (next.GetTokenType() != TokenType.CLOSE_BRACKET)
                                {
                                    this.BuildArguments(this.tokens.GetNext());
                                }
                                if (this.tokens.HasNext())
                                {
                                    next = this.tokens.GetNext();
                                    if (next.GetTokenType() != TokenType.CLOSE_BRACKET)
                                    {
                                        this.PrintUnexpectedToken("close bracket", next);
                                    }
                                }
                                else
                                {
                                    this.PrintUnexpectedEndOfProgram("close bracket", next);
                                }
                            }
                            else
                            {
                                this.PrintUnexpectedEndOfProgram("close bracket or arguments of function", next);
                            }
                        }
                        else
                        {
                            this.PrintUnexpectedToken("open bracket", next);
                        }
                    }
                    else
                    {
                        this.PrintUnexpectedEndOfProgram("open bracket", this.token);
                    }
                }
                else
                {
                    this.PrintUnexpectedToken("identifier of function", this.token);
                }
            }
            else
            {
                this.PrintUnexpectedToken("identifier of function", this.token);
            }
            this.FinishBuild();
        }

        /// <summary>
        /// Builds arguments which will be passed to function
        /// </summary>
        /// <param name="t">Token representing start of arguments</param>
        private void BuildArguments(Token t)
        {
            Expression arg = new Expression(this.tokens, t, this.block);
            arg.Build();
            this.arguments.Add(arg);
            if (this.tokens.HasNext())
            {
                if (this.tokens.ObserveNext().GetTokenType() == TokenType.COMMA)
                {
                    Token next = this.tokens.GetNext();
                    if (this.tokens.HasNext())
                    {
                        next = this.tokens.GetNext();
                        this.BuildArguments(next);
                    }
                    else
                    {
                        this.PrintUnexpectedEndOfProgram("argument of function", next);
                    }
                }
            }
            else
            {
                this.PrintUnexpectedEndOfProgram("close bracket or comma", t);
            }
        }

        public void Execute()
        {
            if (this.block.HasFunction(this.function))
            {
                FunctionModel f = this.block.GetFunction(this.function);
                if (f.GetArguments() == this.arguments.Count)
                {
                    f.GetBlock().CreateState();
                    //f.GetBlock().Reset();
                    f.Initialize();
                    for (int i = 0; i < this.arguments.Count; i++)
                    {
                        this.arguments[i].Execute();
                        VariableModel arg = f.GetArgument(i);
                        arg.SetValue(this.arguments[i]);
                    }
                    f.GetExecutable().InitializeArguments();
                    f.GetBlock().SetFlag(Register.EFlags.FRUN);
                    f.GetExecutable().Execute();
                    f.GetBlock().UnsetFlag(Register.EFlags.FRUN);
                    if (f.HasReturn())
                    {
                        this.value = f.GetReturn();
                    }
                    f.GetBlock().PreviousState();
                }
                else
                {
                    Interpreter.Interpreter.PrintError("Execution of function '" + this.function + "' call failed: Arugments does not match (expected " + f.GetArguments() + ", but got " + this.arguments.Count + ")!", this.token);
                }
            }
            else
            {
                Interpreter.Interpreter.PrintError("Execution of function '" + this.function + "' call failed: Unknown function!", this.token);
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

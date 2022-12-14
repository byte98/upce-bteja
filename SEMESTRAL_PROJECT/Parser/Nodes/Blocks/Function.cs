using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SemestralProject.Lexer;

namespace SemestralProject.Parser.Nodes.Blocks
{
    /// <summary>
    /// Class representing function node
    /// </summary>
    public class Function: BlockNode
    {
        /// <summary>
        /// Arguments of function
        /// </summary>
        private readonly IList<VariableModel> arguments;

        /// <summary>
        /// Function model representing this function node
        /// </summary>
        private FunctionModel function;

        /// <summary>
        /// Creates new function node
        /// </summary>
        /// <param name="tokens">Tokens from source code</param>
        /// <param name="token">Token representing node</param>
        /// <param name="parent">Parent block to which this block belongs to</param>
        public Function(Lexer.TokenStream tokens, Lexer.Token token, BlockNode block)
            : base(tokens, token, block)
        {
            this.arguments = new List<VariableModel>();
        }

        public override void Build()
        {
            this.StartBuild();
            if (this.tokens.HasNext())
            {
                Token next = this.tokens.GetNext();
                if (next.GetTokenType() == TokenType.FUNCTION_IDENTIFIER)
                {
                    string identifier = next.GetValue();
                    if (this.tokens.HasNext())
                    {
                        next = this.tokens.GetNext();
                        if (next.GetTokenType() == TokenType.OPEN_BRACKET)
                        {
                            if (this.tokens.HasNext())
                            {
                                next = this.tokens.ObserveNext();
                                if (next.GetTokenType() != TokenType.CLOSE_BRACKET)
                                {
                                    this.BuildArguments();
                                }
                                else
                                {
                                    next = this.tokens.GetNext();
                                }
                                if (this.tokens.HasNext())
                                {
                                    next = this.tokens.GetNext();
                                    if (next.GetTokenType() == TokenType.OPEN_CURLY)
                                    {
                                        if (this.tokens.HasNext())
                                        {
                                            next = this.tokens.GetNext();
                                            if (next.GetTokenType() != TokenType.CLOSE_CURLY)
                                            {
                                                FunctionBody body = new FunctionBody(this.tokens, next, this);
                                                body.Build();
                                                if (this.tokens.HasNext())
                                                {
                                                    next = this.tokens.GetNext();
                                                    if (next.GetTokenType() != TokenType.CLOSE_CURLY)
                                                    {
                                                        this.PrintUnexpectedToken("close curly bracket", next);
                                                    }
                                                    else
                                                    {
                                                        this.function = new FunctionModel(identifier, body, this);
                                                        if (this.arguments.Count > 0)
                                                        {
                                                            foreach (VariableModel argument in this.arguments)
                                                            {
                                                                this.function.AddArgument(argument);
                                                            }
                                                        }
                                                        this.block.AddFunction(this.function);
                                                    }
                                                }
                                                else
                                                {
                                                    this.PrintUnexpectedEndOfProgram("close curly bracket");
                                                }
                                            }
                                            else
                                            {
                                                this.function = new FunctionModel(identifier, FunctionBody.Empty(this.tokens, next, this), this);
                                                if (this.arguments.Count > 0)
                                                {
                                                    foreach (VariableModel argument in this.arguments)
                                                    {
                                                        this.function.AddArgument(argument);
                                                    }
                                                }
                                                this.block.AddFunction(this.function);
                                            }
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
                                this.PrintUnexpectedEndOfProgram("arguments or close bracket");
                            }
                        }
                        else
                        {
                            this.PrintUnexpectedToken("open bracket", next);
                        }
                    }
                    else
                    {
                        this.PrintUnexpectedEndOfProgram("open bracket", next);
                    }
                }
                else
                {
                    this.PrintUnexpectedToken("name of function", next);
                }
            }
            else
            {
                this.PrintUnexpectedEndOfProgram("name of function");
            }
            this.FinishBuild();
        }

        /// <summary>
        /// Gets data representation of function
        /// </summary>
        /// <returns>Model of function</returns>
        public FunctionModel GetModel()
        {
            return this.function;
        }

        /// <summary>
        /// Checks, whether variable is argument
        /// </summary>
        /// <param name="variable">Name of variable</param>
        /// <returns>TRUE if variable is argument of function, FALSE otherwise</returns>
        public bool IsArgument(string variable)
        {
            bool reti = false;
            foreach(VariableModel v in this.arguments)
            {
                if (v.GetName().Equals(variable))
                {
                    reti = true;
                    break;
                }
            }
            return reti;
        }

        public override void CreateState()
        {
            IList<string> varnames = this.context.GetVariables();
            foreach (string varname in varnames)
            {
                VariableModel var = this.context.GetVariable(varname);
                var.NewEvaluation();
                if (this.IsArgument(varname) == false)
                {
                    var.Unevaluate();
                }
            }
        }

        /// <summary>
        /// Builds arguments of function
        /// </summary>
        private void BuildArguments()
        {
            if (this.tokens.HasNext())
            {
                Token next = this.tokens.ObserveNext();
                if (next.GetTokenType() == TokenType.INTEGER ||
                    next.GetTokenType() == TokenType.FLOAT   ||
                    next.GetTokenType() == TokenType.STRING)
                {
                    Token datatype = this.tokens.GetNext();
                    EDataType dt = EDataType.INTEGER;
                    if (datatype.GetTokenType() == TokenType.FLOAT)
                    {
                        dt = EDataType.FLOAT;
                    }
                    else if (datatype.GetTokenType() == TokenType.STRING)
                    {
                        dt = EDataType.STRING;
                    }
                    if (this.tokens.HasNext())
                    {
                        next = this.tokens.GetNext();
                        if (next.GetTokenType() == TokenType.VARIABLE)
                        {
                            this.arguments.Add(new VariableModel(next.GetValue(), dt));
                            if (this.tokens.HasNext())
                            {
                                next = this.tokens.GetNext();
                                if (next.GetTokenType() == TokenType.COMMA)
                                {
                                    this.BuildArguments();
                                }
                                else if (next.GetTokenType() != TokenType.CLOSE_BRACKET)
                                {
                                    this.PrintUnexpectedToken("close bracket", next);
                                }
                            }
                            else
                            {
                                this.PrintUnexpectedEndOfProgram("comma or close bracket", next);
                            }
                        }
                        else
                        {
                            this.PrintUnexpectedToken("identifier of variable", next);
                        }
                    }
                    else
                    {
                        this.PrintUnexpectedEndOfProgram("name of argument", next);
                    }
                }
            }
            else
            {
                this.PrintUnexpectedEndOfProgram("data type of argument");
            }
        }
    }
}

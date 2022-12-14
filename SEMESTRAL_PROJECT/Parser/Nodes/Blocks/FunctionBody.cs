using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemestralProject.Lexer;
using SemestralProject.Parser.Nodes.Commands;

namespace SemestralProject.Parser.Nodes.Blocks
{
    /// <summary>
    /// Class representing body of function
    /// </summary>
    public class FunctionBody: Node, IExecutableNode
    {
        /// <summary>
        /// List of nodes forming body of function
        /// </summary>
        private List<IExecutableNode> nodes;

        /// <summary>
        /// Function to which this body belongs to
        /// </summary>
        private readonly Function function;

        /// <summary>
        /// Flag, whether function body should be executed
        /// </summary>
        private bool running = true;

        /// <summary>
        /// Reference to return statement
        /// </summary>
        private Return reti;

        /// <summary>
        /// Creates new body of function
        /// </summary>
        /// <param name="tokens">Tokens from source code</param>
        /// <param name="token">Token representing node</param>
        /// <param name="function">Function to which this body belongs to</param>
        public FunctionBody(Lexer.TokenStream tokens, Lexer.Token token, Function function)
            : base(tokens, token, function)
        {
            this.nodes = new List<IExecutableNode>();
            this.function = function;
        }

        public override void Build()
        {
            this.BuildContent(this.token);
        }

        /// <summary>
        /// Builds content of function body
        /// </summary>
        /// <param name="t">Token representing start of body</param>
        private void BuildContent(Token t)
        {
            this.StartBuild();
            if (t.GetTokenType() == TokenType.IF)
            {
                If nodeIf = new If(this.tokens, t, this.function);
                nodeIf.Build();
                this.nodes.Add(nodeIf);
                if (this.tokens.HasNext() && (this.tokens.ObserveNext().GetTokenType() != TokenType.CLOSE_CURLY &&
                                              this.tokens.ObserveNext().GetTokenType() != TokenType.PROG_END))
                {
                    this.BuildContent(this.tokens.GetNext());
                }
            }
            else if (t.GetTokenType() == TokenType.WHILE)
            {
                While nodeWhile = new While(this.tokens, t, this.function);
                nodeWhile.Build();
                this.nodes.Add(nodeWhile);
                if (this.tokens.HasNext() && (this.tokens.ObserveNext().GetTokenType() != TokenType.CLOSE_CURLY &&
                                              this.tokens.ObserveNext().GetTokenType() != TokenType.PROG_END))
                {
                    this.BuildContent(this.tokens.GetNext());
                }
            }
            else if (t.GetTokenType() == TokenType.RETURN)
            {
                this.reti = new Return(this.tokens, t, this.function);
                this.reti.Build();
                this.nodes.Add(this.reti);
            }
            else if (t.GetTokenType() != TokenType.PROG_END &&
                t.GetTokenType() != TokenType.CLOSE_CURLY)
            {
                Command cmd = new Command(this.tokens, t, this.function);
                cmd.Build();
                this.nodes.Add(cmd);
                if (this.tokens.HasNext() && (this.tokens.ObserveNext().GetTokenType() != TokenType.CLOSE_CURLY &&
                                              this.tokens.ObserveNext().GetTokenType() != TokenType.PROG_END))
                {
                    this.BuildContent(this.tokens.GetNext());
                }
            }
            this.FinishBuild();
        }

        /// <summary>
        /// Initializes arguments of function
        /// </summary>
        public void InitializeArguments()
        {
            this.function.SetFlag(Register.EFlags.FARG);
            foreach(string varname in this.function.GetVariables())
            {
                if (this.function.IsArgument(varname))
                {
                    VariableModel var = this.function.GetVariable(varname);
                    if (var.IsEvaluated() == false || this.function.HasFlag(Register.EFlags.FRUN) == false)
                    {
                        var.Evaluate();
                    }
                }
            }
            this.function.UnsetFlag(Register.EFlags.FARG);
        }

        public void Execute()
        {
            this.running = true;
            foreach (IExecutableNode exec in this.nodes)
            {
                if (this.running == true)
                {
                    exec.Execute();
                    if (this.reti != null && this.reti.Equals(exec)) // Return called
                    {
                        if (this.reti.HasReturnValue())
                        {
                            if (this.reti.HasValue())
                            {
                                if (this.function.GetModel() != null)
                                {
                                    this.function.GetModel().SetReturn(this.reti.GetValue());
                                }
                                else
                                {
                                    Interpreter.Interpreter.PrintError("Execution of function " + this.function.ToString() + " failed: unknown function!", this.token);
                                }
                            }
                            else
                            {
                                Interpreter.Interpreter.PrintError("Execution of function '" + this.function.GetModel().GetName() + "' failed: return value expected!", this.token);
                            }
                        }
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Creates empty function body
        /// </summary>
        /// <param name="tokens">Tokens from source code</param>
        /// <param name="token">Token representing node</param>
        /// <param name="function">Function to which this body belongs to</param>
        /// <returns>Newly created empty function body</returns>
        public static FunctionBody Empty(Lexer.TokenStream tokens, Lexer.Token token, Function function)
        {
            FunctionBody reti = new FunctionBody(tokens, token, function);
            Nop nop = new Nop(tokens, token, function);
            nop.Build();
            reti.nodes.Add(nop);
            return reti;
        }
    }
}

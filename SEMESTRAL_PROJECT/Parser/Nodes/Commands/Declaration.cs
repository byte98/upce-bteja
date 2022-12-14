using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemestralProject.Lexer;

namespace SemestralProject.Parser.Nodes.Commands
{
    /// <summary>
    /// Class representing declaration of variable
    /// </summary>
    class Declaration: Node, IExecutableNode
    {
        /// <summary>
        /// Representation of variable
        /// </summary>
        private VariableModel variable;

        /// <summary>
        /// Creates new declaration node
        /// </summary>
        /// <param name="tokens">Tokens from source code</param>
        /// <param name="token">Token representing node</param>
        /// <param name="parent">Parent block to which this block belongs to</param>
        public Declaration(Lexer.TokenStream tokens, Lexer.Token token, BlockNode block)
            : base(tokens, token, block) { }

        public override void Build()
        {
            this.StartBuild();
            EDataType dataType = EDataType.STRING;
            if (this.token.GetTokenType() == TokenType.STRING)
            {
                dataType = EDataType.STRING;
            }
            else if (this.token.GetTokenType() == TokenType.FLOAT)
            {
                dataType = EDataType.FLOAT;
            }
            else if (this.token.GetTokenType() == TokenType.INTEGER)
            {
                dataType = EDataType.INTEGER;
            }
            else
            {
                this.PrintUnexpectedToken("any data type", this.token);
            }
            if (this.tokens.HasNext())
            {
                Token next = this.tokens.GetNext();
                if (next.GetTokenType() == TokenType.VARIABLE)
                {
                    if (next.HasValue())
                    {
                        string identifier = next.GetValue();
                        if (this.tokens.HasNext())
                        {
                            next = this.tokens.GetNext();
                            if (next.GetTokenType() == TokenType.ASSIGNMENT)
                            {
                                if (this.tokens.HasNext())
                                {
                                    Expressions.Expression expression = new Expressions.Expression(this.tokens, this.tokens.GetNext(), this.block);
                                    expression.Build();
                                    this.variable = new VariableModel(identifier, dataType);
                                    variable.SetValue(expression);
                                    if (this.block.HasVariable(identifier) == false)
                                    {
                                        this.block.AddVariable(this.variable);
                                        Parser.Print("Added new variable '" + identifier + "' with defined value", this.token);
                                    }
                                    else
                                    {
                                        Parser.PrintError("Unable to declare variable '" + identifier + "': variable already exists!", this.token);
                                    }
                                    
                                }
                                else
                                {
                                    this.PrintUnexpectedEndOfProgram("any expression");
                                }
                            }
                            else if (next.GetTokenType() != TokenType.SEMICOLON)
                            {
                                this.PrintUnexpectedToken("assignment or semicolon", token);
                            }
                        }
                        else
                        {
                            this.PrintUnexpectedEndOfProgram("assignment");
                        }
                    }
                    else
                    {
                        this.PrintUnexpectedToken("identifier of variable with set value", next);
                    }
                }
                else
                {
                    this.PrintUnexpectedToken("identifier of variable", next);
                }
            }
            else
            {
                this.PrintUnexpectedEndOfProgram("indetifier of variable");
            }
        }

        public void Execute()
        {
            this.variable.Evaluate();
            if (this.variable.IsEvaluated())
            {
                if (this.variable.GetEvaluation() != null && this.variable != null)
                {
                    if (this.variable.GetEvaluation().GetDataType() != this.variable.GetDataType())
                    {
                        Interpreter.Interpreter.PrintError("Initialization of variable failed: trying to set " +
                            this.variable.GetEvaluation().GetDataType() + " to " + this.variable.GetDataType() + "!", this.token);
                    }
                }
                else
                {
                    Interpreter.Interpreter.PrintError("Execution of variable declaration failed: unknown variable!", this.token);
                }
                
            }
        }
    }
}

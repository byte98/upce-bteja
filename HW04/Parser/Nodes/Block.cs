using HW04.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW04.Parser.Nodes
{
    /// <summary>
    /// Class representing block node in AST
    /// </summary>
    class Block : BlockNode
    {
        /// <summary>
        /// List of available constants
        /// </summary>
        private List<Constant> constants;

        /// <summary>
        /// List of available variables
        /// </summary>
        private List<Variable> variables;

        /// <summary>
        /// List of available procedures
        /// </summary>
        private List<Procedure> procedures;

        /// <summary>
        /// Statement hold by this block
        /// </summary>
        private Statement statement;


        /// <summary>
        /// Creates new block node in AST
        /// </summary>
        /// <param name="tokens">Stream of tokens from lexer</param>
        /// <param name="token">Token defining block node in AST</param>
        /// <param name="block">Parential code block</param>
        public Block(TokenStream tokens, Token token, BlockNode block) : base(tokens, token, block)
        {
            this.constants = new List<Constant>();
            this.variables = new List<Variable>();
            this.procedures = new List<Procedure>();
            this.codeBlock = new CodeBlock(codeBlock);
        }

        public override void Build()
        {
            Parser.Print("Building block#" + this.id + "...", this.token);
            this.BuildBlock();
        }

        /// <summary>
        /// Gets actual block of code
        /// </summary>
        /// <returns>Actual block of code</returns>
        public CodeBlock GetCodeBlock()
        {
            return this.codeBlock;
        }

        /// <summary>
        /// Build block node
        /// </summary>
        private void BuildBlock()
        {
            if (this.tokens.HasNext())
            {
                Token t = this.tokens.GetNext();
                if (t.GetTokenType() == TokenType.CONST)
                {
                    this.BuildConstants();
                }
                else if (t.GetTokenType() == TokenType.VAR)
                {
                    this.BuildVariables();
                }
                else if (t.GetTokenType() == TokenType.PROCEDURE)
                {
                    this.BuildProcedure();
                }
                else
                {
                    this.BuildStatement(t);
                }
            }
            else
            {
                Parser.PrintError("Unexpected end of program during building block#" + this.id + "!", this.token);
            }
            
        }

        /// <summary>
        /// Builds all available constants
        /// </summary>
        private void BuildConstants()
        {
            if (this.tokens.HasNext())
            {
                Token next = this.tokens.GetNext();
                Constant c = new Constant(this.tokens, next);
                c.Build();
                this.constants.Add(c);
                if (c.HasName() && c.HasValue())
                {
                    if (this.AddConstant(c.GetName(), c.GetValue()) == false)
                    {
                        Parser.PrintError("Failed to set constant (constant has been already set)!", next);
                    }
                }
                else
                {
                    Parser.PrintError("Failed to build constant (name and/or value not specified)!", next);
                }
                if (this.tokens.HasNext())
                {
                    Token t = this.tokens.GetNext();
                    if (t.GetTokenType() == TokenType.COMMA)
                    {
                        this.BuildConstants();
                    }
                    else if (t.GetTokenType() == TokenType.SEMICOLON)
                    {
                        this.BuildBlock();
                    }
                    else
                    {
                        Parser.PrintError("Unexpected token during building block#" + this.id + " constants!", t);
                    }
                }
                else
                {
                    Parser.PrintError("Unexpected end of program during building block#" + this.id + " constants!", this.token);
                }
            }
            else
            {
                Parser.PrintError("Unexpected end of program during building block#" + this.id + " constants!", this.token);
            }
        }

        /// <summary>
        /// Builds all available variables
        /// </summary>
        private void BuildVariables()
        {
            if (this.tokens.HasNext())
            {
                Token next = this.tokens.GetNext();
                Variable v = new Variable(this.tokens, next);
                v.Build();
                this.variables.Add(v);
                if (v.HasName())
                {
                    this.SetVariable(v.GetName(), double.NaN);
                }
                else
                {
                    Parser.PrintError("Unexpected state during building block#" + this.id + " variables! Variable must have a name!", next);
                }
                if (this.tokens.HasNext())
                {
                    Token t = this.tokens.GetNext();
                    if (t.GetTokenType() == TokenType.COMMA)
                    {
                        this.BuildVariables();
                    }
                    else if (t.GetTokenType() == TokenType.SEMICOLON)
                    {
                        this.BuildBlock();
                    }
                    else
                    {
                        Parser.PrintError("Unexpected token during building block#" + this.id + " variables! Comma or semicolon expected!", t);
                    }
                }
                else
                {
                    Parser.PrintError("Unexpected end of program during building block#" + this.id + " variables!", this.token);
                }
            }
            else
            {
                Parser.PrintError("Unexpected end of program during building block#" + this.id + " variables!", this.token);
            }
        }

        /// <summary>
        /// Builds procedure
        /// </summary>
        private void BuildProcedure()
        {
            if (this.tokens.HasNext())
            {
                Token next = this.tokens.GetNext();
                Procedure p = new Procedure(this.tokens, next, this);
                p.Build();
                this.procedures.Add(p);
                if (this.tokens.HasNext())
                {
                    Token t = this.tokens.GetNext();
                    if (t.GetTokenType() == TokenType.SEMICOLON)
                    {
                        this.BuildBlock();
                    }
                    else if (t.GetTokenType() == TokenType.END)
                    {
                        Parser.Print("Found end of block#" + this.id, this.token);
                        if (this.tokens.HasNext())
                        {
                            this.BuildBlock();
                        }
                    }
                    else
                    {
                        Parser.PrintError("Unexpected token during building block#" + this.id + " procedures! Semicolon or end expected!", t);
                    }
                }
                else
                {
                    Parser.PrintError("Unexpected end of program during building block#" + this.id + " procedures!", this.token);
                }
            }
            else
            {
                Parser.PrintError("Unexpected end of program during building block#" + this.id + " procedures!", this.token);
            }
        }

        /// <summary>
        /// Builds statement
        /// </summary>
        /// <param name="t"></param>
        private void BuildStatement(Token t)
        {
            this.statement = new Statement(this.tokens, t, this);
            this.statement.Build();
        }

        /// <summary>
        /// Gets executable node of this block
        /// </summary>
        /// <returns>Executable node of this block</returns>
        public ExecutableNode GetExecutable()
        {
            return this.statement;
        }
    }
}

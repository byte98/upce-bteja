using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HW01.ENUMS;

namespace HW01
{
    /// <summary>
    /// Class which interprets mathematical expressions
    /// </summary>
    class Interpreter
    {
        /// <summary>
        /// Stack holding all loaded numbers
        /// </summary>
        private Stack<double> stack;

        /// <summary>
        /// Actual position of cursor
        /// </summary>
        private int cursor;

        /// <summary>
        /// Current input of interpreter
        /// </summary>
        private char[] input;

        /// <summary>
        /// Temporary variable for parsed input
        /// </summary>
        private StringBuilder temporary;

        /// <summary>
        /// Creates new interpreter of mathematical expressions
        /// </summary>
        public Interpreter()
        {
            this.stack = new Stack<double>();
            this.temporary = new StringBuilder();
            this.cursor = 0;
        }

        /// <summary>
        /// Executes an input
        /// </summary>
        /// <param name="input">Input which will be executed</param>
        /// <returns>Result of execution</returns>
        public double Execute(String input)
        {
            double reti = Double.NaN;
            this.Init();
            this.input = input.ToCharArray();
            if (this.LoadExpression())
            {
                reti = this.stack.Pop();
            }
            return reti;
        }

        /// <summary>
        /// Initializes intepreter
        /// </summary>
        private void Init()
        {
            this.stack.Clear();
            this.temporary.Clear();
            this.cursor = 0;
        }

        /// <summary>
        /// Loads plus or minus sign
        /// </summary>
        /// <returns>Loaded sign</returns>
        private PlusMinus LoadPlusMinus()
        {
            PlusMinus reti = PlusMinus.NOTHING;
            if (this.cursor < this.input.Length)
            {
                if (this.input[this.cursor] == '+')
                {
                    reti = PlusMinus.PLUS;
                    this.cursor++;
                }
                else if (this.input[this.cursor] == '-')
                {
                    reti = PlusMinus.MINUS;
                    this.cursor++;
                }
            }
            return reti;
        }

        /// <summary>
        /// Loads multiply or divide sign
        /// </summary>
        /// <returns>Loaded sign</returns>
        private MultiplyDivide LoadMultiplyDivide()
        {
            MultiplyDivide reti = MultiplyDivide.NOTHING;
            if (this.cursor < this.input.Length)
            {
                if (this.input[this.cursor] == '*')
                {
                    reti = MultiplyDivide.MULTIPLY;
                    this.cursor++;
                }
                else if (this.input[this.cursor] == '/')
                {
                    reti = MultiplyDivide.DIVIDE;
                    this.cursor++;
                }
            }
            return reti;
        }

        /// <summary>
        /// Loads number from input
        /// </summary>
        /// <returns><c>TRUE</c> if any number has been loaded, <c>FALSE</c> otherwise</returns>
        private bool LoadNumber()
        {
            bool reti = false;
            this.temporary.Clear();
            while (this.cursor < this.input.Length && Char.IsDigit(this.input[this.cursor]))
            {
                this.temporary.Append(this.input[this.cursor]);
                this.cursor++;
            }
            if (this.temporary.Length == 0) // No number has been loaded
            {
                reti = false;
            }
            else
            {
                String result = this.temporary.ToString();
                this.stack.Push(Double.Parse(result));
                reti = true;
            }
            return reti;
        }

        /// <summary>
        /// Loads term from input
        /// </summary>
        /// <returns><c>TRUE</c> if any term has been loaded, <c>FALSE</c> otherwise</returns>
        private bool LoadTerm()
        {
            bool reti = false;
            if (this.LoadNumber())
            {
                while (true)
                {
                    MultiplyDivide op = this.LoadMultiplyDivide();
                    if (op == MultiplyDivide.NOTHING)
                    {
                        reti = true;
                        break;
                    }
                    else
                    {
                        this.LoadNumber();
                        double v1 = this.stack.Pop(), v2 = this.stack.Pop();
                        double result = (
                            op == MultiplyDivide.MULTIPLY
                            ? v2 * v1
                            : v2 / v1
                        );
                        this.stack.Push(result);
                    }
                }
                reti = true;
            }
            return reti;
        }

        /// <summary>
        /// Loads expression from input
        /// </summary>
        /// <returns><c>TRUE</c> if any expression has been loaded, <c>FALSE</c> otherwise</returns>
        private bool LoadExpression()
        {
            bool reti = false;
            if (this.LoadTerm())
            {
                while (true)
                {
                    PlusMinus op = this.LoadPlusMinus();
                    if (op == PlusMinus.NOTHING)
                    {
                        reti = true;
                        break;
                    }
                    else
                    {
                        this.LoadTerm();
                        double v1 = this.stack.Pop(), v2 = this.stack.Pop();
                        double result = (
                            op == PlusMinus.PLUS
                            ? v2 + v1
                            : v2 - v1
                        );
                        this.stack.Push(result);
                    }
                }
                reti = true;
            }
            return reti;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralProject.Parser
{
    /// <summary>
    /// Class representing value of expression
    /// </summary>
    public class Value
    {
        /// <summary>
        /// Raw value
        /// </summary>
        private readonly string value;

        /// <summary>
        /// Data type of value
        /// </summary>
        private readonly EDataType type;

        /// <summary>
        /// Creates new value
        /// </summary>
        /// <param name="data">Raw data for value</param>
        public Value(string data) : this(data, true) { }

        /// <summary>
        /// Creates new value
        /// </summary>
        /// <param name="data">Raw data for value</param>
        /// <param name="resolveType">Flag, whether data type of value should be resolved from its data</param>
        public Value(string data, bool resolveType)
        {
            this.value = data;
            if (resolveType == true)
            {
                this.type = Value.DetermineDataType(this.value);
            }
            else
            {
                this.type = EDataType.STRING;
            }
        }

        /// <summary>
        /// Creates new value
        /// </summary>
        /// <param name="data">Integer which will be hold in value</param>
        public Value(int data)
        {
            this.value = data.ToString();
            this.type = EDataType.INTEGER;
        }

        /// <summary>
        /// Creates new value
        /// </summary>
        /// <param name="data">Floating point number which will be hold in value</param>
        public Value(float data)
        {
            this.value = data.ToString();
            this.type = EDataType.FLOAT;
        }

        /// <summary>
        /// Determines data type from string
        /// </summary>
        /// <param name="data">String which content data type will be determined</param>
        /// <returns>Data type of content of string</returns>
        private static EDataType DetermineDataType(string data)
        {
            EDataType reti = EDataType.STRING;
            int ival;
            float fval;
            if (int.TryParse(data, out ival))
            {
                reti = EDataType.INTEGER;
            }
            else if (float.TryParse(data, out fval))
            {
                reti = EDataType.STRING;
            }
            return reti;
        }

        /// <summary>
        /// Gets string representation of value
        /// </summary>
        /// <returns>String representation of value or empty string if data type is different than string</returns>
        public string GetString()
        {
            string reti = "";
            if (this.type == EDataType.STRING)
            {
                reti = this.value;
            }
            return reti;
        }

        /// <summary>
        /// Gets integer representation of value
        /// </summary>
        /// <returns>Integer representation of value or <code>int.MinValue</code> if data type is different than integer</returns>
        public int GetInt()
        {
            int reti = int.MinValue;
            if (this.type == EDataType.INTEGER)
            {
                reti = int.Parse(this.value);
            }
            return reti;
        }

        /// <summary>
        /// Gets float representation of value
        /// </summary>
        /// <returns>Float representation of value or <code>float.NaN</code> if data type is different than float</returns>
        public float GetFloat()
        {
            float reti = float.NaN;
            if (this.type == EDataType.FLOAT)
            {
                reti = float.Parse(this.value);
            }
            return reti;
        }

        /// <summary>
        /// Gets data type of value
        /// </summary>
        /// <returns>Data type of value</returns>
        public EDataType GetDataType()
        {
            return this.type;
        }

        /// <summary>
        /// Gets raw data of value
        /// </summary>
        /// <returns>Raw data of value</returns>
        public string GetData()
        {
            return this.value;
        }

        /// <summary>
        /// Sums two values
        /// </summary>
        /// <param name="a">First member of operation</param>
        /// <param name="b">Second member of operation</param>
        /// <returns>Result of operation</returns>
        public static Value operator+(Value a, Value b)
        {
            Value reti = null;
            if (a.GetDataType() != b.GetDataType())
            {
                Interpreter.Interpreter.PrintError("Invalid data type for operation '+': cannot add " + a.GetDataType().ToString() + " to " + b.GetDataType().ToString() + "!", null);
            }
            else
            {
                if (a.GetDataType() == EDataType.STRING)
                {
                    reti = new Value(a.GetString() + b.GetString());
                }
                else if (a.GetDataType() == EDataType.INTEGER)
                {
                    reti = new Value((int)(a.GetInt() + b.GetInt()));
                }
                else if (a.GetDataType() == EDataType.FLOAT)
                {
                    reti = new Value((float)(a.GetFloat() + b.GetFloat()));
                }
            }
            return reti;
        }

        /// <summary>
        /// Subtracts two values
        /// </summary>
        /// <param name="a">First member of operation</param>
        /// <param name="b">Second member of operation</param>
        /// <returns>Result of operation</returns>
        public static Value operator -(Value a, Value b)
        {
            Value reti = null;
            if (a.GetDataType() != b.GetDataType())
            {
                Interpreter.Interpreter.PrintError("Invalid data type for operation '-': cannot retract " + a.GetDataType().ToString() + " from " + b.GetDataType().ToString() + "!", null);
            }
            else
            {
                if (a.GetDataType() == EDataType.STRING)
                {
                    reti = new Value(a.GetString().Replace(b.GetString(), ""));
                }
                else if (a.GetDataType() == EDataType.INTEGER)
                {
                    reti = new Value((int)(a.GetInt() - b.GetInt()));
                }
                else if (a.GetDataType() == EDataType.FLOAT)
                {
                    reti = new Value((float)(a.GetFloat() - b.GetFloat()));
                }
            }
            return reti;
        }

        /// <summary>
        /// Multiplies two values
        /// </summary>
        /// <param name="a">First member of operation</param>
        /// <param name="b">Second member of operation</param>
        /// <returns>Result of operation</returns>
        public static Value operator *(Value a, Value b)
        {
            Value reti = null;
            if (a.GetDataType() != b.GetDataType() || a.GetDataType() == EDataType.STRING)
            {
                Interpreter.Interpreter.PrintError("Invalid data type for operation '*': cannot multiply " + a.GetDataType().ToString() + " by " + b.GetDataType().ToString() + "!", null);
            }
            else
            {
                if (a.GetDataType() == EDataType.INTEGER)
                {
                    reti = new Value((int)(a.GetInt() * b.GetInt()));
                }
                else if (a.GetDataType() == EDataType.FLOAT)
                {
                    reti = new Value((float)(a.GetFloat() * b.GetFloat()));
                }
            }
            return reti;
        }

        /// <summary>
        /// Divides two values
        /// </summary>
        /// <param name="a">First member of operation</param>
        /// <param name="b">Second member of operation</param>
        /// <returns>Result of operation</returns>
        public static Value operator /(Value a, Value b)
        {
            Value reti = null;
            if (a.GetDataType() != b.GetDataType() || a.GetDataType() == EDataType.STRING)
            {
                Interpreter.Interpreter.PrintError("Invalid data type for operation '/': cannot divide " + a.GetDataType().ToString() + " by " + b.GetDataType().ToString() + "!", null);
            }
            else
            {
                if (a.GetDataType() == EDataType.INTEGER)
                {
                    if (b.GetInt() == 0)
                    {
                        Interpreter.Interpreter.PrintError("Invalid value for operation '/': cannot divide by zero!", null);
                    }
                    else
                    {
                        reti = new Value((int)(a.GetInt() / b.GetInt()));
                    }
                }
                else if (a.GetDataType() == EDataType.FLOAT)
                {
                    if (b.GetFloat() == 0)
                    {
                        Interpreter.Interpreter.PrintError("Invalid value for operation '/': cannot divide by zero!", null);
                    }
                    else
                    {
                        reti = new Value((float)(a.GetFloat() / b.GetFloat()));
                    }
                }
            }
            return reti;
        }

        public override string ToString()
        {
            string reti = this.type.ToString() + ":" + this.value;
            return reti;
        }
    }
}

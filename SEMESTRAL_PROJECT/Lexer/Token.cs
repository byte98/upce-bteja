using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralProject.Lexer
{
    /// <summary>
    /// Class representing one token in source code
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Type of token
        /// </summary>
        private TokenType type;

        /// <summary>
        /// Flag, whether tokens holds some value
        /// </summary>
        private bool hasValue;

        /// <summary>
        /// Value of token
        /// </summary>
        private string value;

        /// <summary>
        /// Number of row where token can be found in source code
        /// </summary>
        private int row;

        /// <summary>
        /// Number of column where token can be found in source code
        /// </summary>
        private int column;

        /// <summary>
        /// Line from source code containing token
        /// </summary>
        private string line;

        /// <summary>
        /// Textual representation of token
        /// </summary>
        private string text;

        /// <summary>
        /// Path to file with source code
        /// </summary>
        private string file;


        /// <summary>
        /// Creates new token
        /// </summary>
        /// <param name="type">Type of token</param>
        /// <param name="text">Textual representation of token</param>
        /// <param name="row">Number of row where token can be found in source code</param>
        /// <param name="column">Number of column where token can be found in source code</param>
        /// <param name="line">Line containing token</param>
        /// <param name="file">Path to file containing source code</param>
        public Token(TokenType type, string text, int row, int column, string line, string file)
        {
            this.type = type;
            this.row = row;
            this.column = column;
            this.line = line;
            this.file = file;
            this.text = text;
            this.hasValue = false;
            this.value = null;
        }

        /// <summary>
        /// Sets value to token
        /// </summary>
        /// <param name="value">New value of token</param>
        public void SetValue(string value)
        {
            this.hasValue = true;
            this.value = value;
            
        }

        /// <summary>
        /// Gets type of token
        /// </summary>
        /// <returns>Type of token</returns>
        public TokenType GetTokenType()
        {
            return this.type;
        }

        /// <summary>
        /// Checks, whether token has defined value
        /// </summary>
        /// <returns>TRUE, if token has defined value, FALSE otherwise</returns>
        public bool HasValue()
        {
            return this.hasValue;
        }

        /// <summary>
        /// Gets value of token
        /// </summary>
        /// <returns>Value of token</returns>
        public string GetValue()
        {
            string reti = null;
            if (this.HasValue())
            {
                reti = this.value;
            }
            return reti;
        }

        /// <summary>
        /// Checks, whether value stored in token is float data type
        /// </summary>
        /// <returns>TRUE if value stored in token is float data type, FALSE otherwise</returns>
        public bool IsFloat()
        {
            float temp;
            CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.CurrencyDecimalSeparator = ".";
            return this.HasValue() && this.GetTokenType() == TokenType.LITERAL_FLOAT && float.TryParse(this.value, NumberStyles.Any, ci, out temp);
        }

        /// <summary>
        /// Gets float value of token
        /// </summary>
        /// <returns>Float value of token or <code>float.NaN</code> if token has no float value</returns>
        public float GetFloat()
        {
            float reti = float.NaN;
            if (this.IsFloat())
            {
                reti = float.Parse(this.value);
            }
            return reti;
        }

        /// <summary>
        /// Checks, whether value stored in token is integer data type
        /// </summary>
        /// <returns>TRUE if value stored in token is integer data type, FALSE otherwise</returns>
        public bool IsInteger()
        {
            int temp;
            CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.CurrencyDecimalSeparator = ".";
            return this.HasValue() && this.GetTokenType() == TokenType.LITERAL_INTEGER && int.TryParse(this.GetValue(), NumberStyles.Any, ci, out temp);
        }

        /// <summary>
        /// Gets integer value of token
        /// </summary>
        /// <returns>Integer value of token or <code>int.MinValue</code> if token has no integer value</returns>
        public int GetInteger()
        {
            int reti = int.MinValue;
            if (this.IsInteger())
            {
                reti = int.Parse(this.value);
            }
            return reti;
        }

        /// <summary>
        /// Gets line containing token
        /// </summary>
        /// <returns>Line containing token</returns>
        public string GetLine()
        {
            return this.line;
        }

        /// <summary>
        /// Gets file which contains token
        /// </summary>
        /// <returns>File containing token</returns>
        public string GetFile()
        {
            return this.file;
        }

        /// <summary>
        /// Gets number of row containing token
        /// </summary>
        /// <returns>Number of row containing token</returns>
        public int GetRow()
        {
            return this.row;
        }

        /// <summary>
        /// Gets number of column containing token
        /// </summary>
        /// <returns>Number of column containing token</returns>
        public int GetColumn()
        {
            return this.column;
        }

        /// <summary>
        /// Gets textual content of token
        /// </summary>
        /// <returns>Textual content of token</returns>
        public String GetContent()
        {
            return this.text;
        }

        public override string ToString()
        {
            StringBuilder reti = new StringBuilder();
            reti.Append("TOKEN[");
            reti.Append(this.GetTokenType().ToString());
            reti.Append("]:");
            if (this.HasValue())
            {
                reti.Append(" (" + this.GetValue() + ")");
            }
            reti.Append(" line: ");
            reti.Append(this.row);
            reti.Append("; column:");
            reti.Append(this.column);
            reti.Append("; content: '");
            reti.Append(this.text);
            reti.Append("'; source: '");
            reti.Append(this.line);
            reti.Append("'");
            return reti.ToString();
        }
    }
}

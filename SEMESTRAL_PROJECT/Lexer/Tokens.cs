using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralProject.Lexer
{
    /// <summary>
    /// Class utilizing work with keywords
    /// </summary>
    public class Tokens
    {
        /// <summary>
        /// String defining comment in source code
        /// </summary>
        public static readonly string COMMENT = "//";

        /// <summary>
        /// String identifiing variable
        /// </summary>
        public static readonly string VARIABLE = "$";

        /// <summary>
        /// String identifiing string literal
        /// </summary>
        public static readonly string STRING = "\"";

        /// <summary>
        /// Array with all available tokens
        /// </summary>
        private static readonly string[] VALUES = {
            "<?php",
            "string",
            "float",
            "int",
            "fgets(STDIN)",
            "echo",
            "(",
            ")",
            "=",
            "==",
            "!=",
            ">=",
            "<=",
            ">",
            "<",
            "+",
            "-",
            "*",
            "/",
            "++",
            "--",
            "floatval",
            "intval",
            "strval",
            "if",
            "else",
            "while",
            "{",
            "}",
            ";",
            ",",
            "function",
            "return",
            "break",
            "?>"
        };

        /// <summary>
        /// Gets longest possible token
        /// </summary>
        /// <returns>Longest possible token</returns>
        public static string GetLongest()
        {
            return Tokens.VALUES.OrderByDescending(s => s.Length).First();
        }

        /// <summary>
        /// Checks, whether string contains token
        /// </summary>
        /// <param name="input">String which will be searched for token</param>
        /// <returns>TRUE if string contains token, FALSE otherwise</returns>
        public static bool IsToken(String input)
        {
            return Tokens.VALUES.Contains(input);
        }

        /// <summary>
        /// Gets type of token from string
        /// </summary>
        /// <param name="input">String containing token</param>
        /// <returns>Type of token or NULL if string does not contains any token</returns>
        public static TokenType GetTokenType(String input)
        {
            TokenType reti = TokenType.UNKNOWN;
            switch (input)
            {
                case "<?php":        reti = TokenType.PROG_START;      break;
                case "string":       reti = TokenType.STRING;          break;
                case "float":        reti = TokenType.FLOAT;           break;
                case "int":          reti = TokenType.INTEGER;         break;
                case "fgets(STDIN)": reti = TokenType.READ;            break;
                case "echo":         reti = TokenType.WRITE;           break;
                case "(":            reti = TokenType.OPEN_BRACKET;    break;
                case ")":            reti = TokenType.CLOSE_BRACKET;   break;
                case "=":            reti = TokenType.ASSIGNMENT;      break;
                case "==":           reti = TokenType.EQUALS;          break;
                case "!=":           reti = TokenType.NOT_EQUALS;      break;
                case ">=":           reti = TokenType.GREATER_EQUALS;  break;
                case "<=":           reti = TokenType.LOWER_EQUALS;    break;
                case ">":            reti = TokenType.GREATER;         break;
                case "<":            reti = TokenType.LOWER;           break;
                case "+":            reti = TokenType.PLUS;            break;
                case "-":            reti = TokenType.MINUS;           break;
                case "*":            reti = TokenType.MULTIPLY;        break;
                case "/":            reti = TokenType.DIVIDE;          break;
                case "++":           reti = TokenType.INCREMENT;       break;
                case "--":           reti = TokenType.DECREMENT;       break;
                case "floatval":     reti = TokenType.FLOATVAL;        break;
                case "intval":       reti = TokenType.INTVAL;          break;
                case "strval":       reti = TokenType.STRVAL;          break;
                case "if":           reti = TokenType.IF;              break;
                case "else":         reti = TokenType.ELSE;            break;
                case "while":        reti = TokenType.WHILE;           break;
                case "{":            reti = TokenType.OPEN_CURLY;      break;
                case "}":            reti = TokenType.CLOSE_CURLY;     break;
                case ";":            reti = TokenType.SEMICOLON;       break;
                case ",":            reti = TokenType.COMMA;           break;
                case "function":     reti = TokenType.FUNCTION;        break;
                case "return":       reti = TokenType.RETURN;          break;
                case "break":        reti = TokenType.BREAK;           break;
                case "?>":           reti = TokenType.PROG_END;        break;
                default:             reti = TokenType.UNKNOWN;         break;
            }
            return reti;
        }
    }

    /// <summary>
    /// Enumeration of all available tokens
    /// </summary>
    public enum TokenType
    {
        PROG_START,
        STRING,
        FLOAT,
        INTEGER,
        READ,
        WRITE,
        OPEN_BRACKET,
        CLOSE_BRACKET,
        ASSIGNMENT,
        EQUALS,
        NOT_EQUALS,
        LOWER,
        LOWER_EQUALS,
        GREATER,
        GREATER_EQUALS,
        PLUS,
        MINUS,
        MULTIPLY,
        DIVIDE,
        INCREMENT,
        DECREMENT,
        FLOATVAL,
        INTVAL,
        STRVAL,
        IF,
        ELSE,
        WHILE,
        OPEN_CURLY,
        CLOSE_CURLY,
        SEMICOLON,
        COMMA,
        FUNCTION,
        FUNCTION_IDENTIFIER,
        RETURN,
        BREAK,
        LITERAL_STRING,
        LITERAL_FLOAT,
        LITERAL_INTEGER,
        VARIABLE,
        PROG_END,
        UNKNOWN
    }
}

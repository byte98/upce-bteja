using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW06.Lexer
{
    /// <summary>
    /// Class utilizing work with keywords
    /// </summary>
    class Tokens
    {
        private static readonly string[] VALUES = {
            "ident",
            "number",
            "const",
            "var",
            "procedure",
            "call",
            "begin",
            "end",
            "if",
            "then",
            "while",
            "do",
            "odd",
            "?",
            "!",
            ",",
            ";",
            "=",
            ":=",
            "#",
            "<=",
            ">=",
            ">",
            "<",
            ">",                                                        
            "+",
            "-",
            "*",
            "/",
            "(",
            ")",
            ":",
            "num",
            "str",
            "."
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
                case "ident": reti = TokenType.IDENT; break;
                case "number": reti = TokenType.NUMBER; break;
                case "const": reti = TokenType.CONST; break;
                case "var": reti = TokenType.VAR; break;
                case "procedure": reti = TokenType.PROCEDURE; break;
                case "call": reti = TokenType.CALL; break;
                case "begin": reti = TokenType.BEGIN; break;
                case "end": reti = TokenType.END; break;
                case "if": reti = TokenType.IF; break;
                case "then": reti = TokenType.THEN; break;
                case "while": reti = TokenType.WHILE; break;
                case "do": reti = TokenType.DO; break;
                case "odd": reti = TokenType.ODD; break;
                case "?": reti = TokenType.QUESTION; break;
                case "!": reti = TokenType.EXCLAMATION; break;
                case ",": reti = TokenType.COMMA; break;
                case ";": reti = TokenType.SEMICOLON; break;
                case "=": reti = TokenType.EQUALS; break;
                case ":=": reti = TokenType.ASSIGNMENT; break;
                case "#": reti = TokenType.HASH; break;
                case "<=": reti = TokenType.LOWER_EQUAL; break;
                case ">=": reti = TokenType.GREATER_EQUAL; break;
                case ">": reti = TokenType.GREATER; break;
                case "<": reti = TokenType.LOWER; break;
                case "+": reti = TokenType.PLUS; break;
                case "-": reti = TokenType.MINUS; break;
                case "*": reti = TokenType.MULTIPLY; break;
                case "/": reti = TokenType.DIVIDE; break;
                case "(": reti = TokenType.OPEN_BRACKET; break;
                case ")": reti = TokenType.CLOSE_BRACKET; break;
                case ":": reti = TokenType.COLON; break;
                case "str": reti = TokenType.DT_STRING; break;
                case "num": reti = TokenType.DT_NUMBER; break;
                case ".": reti = TokenType.DOT; break;
                default: reti = TokenType.UNKNOWN; break;
            }
            return reti;
        }
    }

    /// <summary>
    /// Enumeration of all available tokens
    /// </summary>
    public enum TokenType
    {
        IDENT,
        NUMBER,
        CONST,
        VAR,
        PROCEDURE,
        CALL,
        BEGIN,
        END,
        IF,
        THEN,
        WHILE,
        DO,
        ODD,
        QUESTION,
        EXCLAMATION,
        COMMA,
        SEMICOLON,
        EQUALS,
        ASSIGNMENT,
        HASH,
        LOWER_EQUAL,
        GREATER_EQUAL,
        LOWER,
        GREATER,
        PLUS,
        MINUS,
        MULTIPLY,
        DIVIDE,
        OPEN_BRACKET,
        CLOSE_BRACKET,
        DOT,
        COLON,
        DT_NUMBER,
        DT_STRING,
        UNKNOWN
    }
}

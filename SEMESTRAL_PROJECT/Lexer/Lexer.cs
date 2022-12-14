using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SemestralProject.Lexer
{
    /// <summary>
    /// Structure holding position in file
    /// </summary>
    public struct Position
    {
        /// <summary>
        /// Number of line in file
        /// </summary>
        public int line { get; set; }

        /// <summary>
        /// Number of column in file
        /// </summary>
        public int column { get; set; }
    }

    /// <summary>
    /// Class representing lexer transforming input to tokens
    /// </summary>
    public class Lexer
    {
        /// <summary>
        /// Enumeration of all lexer states
        /// </summary>
        private enum LexerState
        {
            /// <summary>
            /// Initial state of lexer
            /// </summary>
            INITIAL,

            /// <summary>
            /// Standard lexer state
            /// </summary>
            STANDARD,

            /// <summary>
            /// Lexer is parsing comment
            /// </summary>
            COMMENT,

            /// <summary>
            /// State where lexer is parsing string
            /// </summary>
            STRING
        }

        /// <summary>
        /// Counter of printed messages
        /// </summary>
        private static int COUNTER = 0;

        /// <summary>
        /// Counter of printed error messages
        /// </summary>
        private static int ERR_COUNTER = 0;

        /// <summary>
        /// Limit for error messages
        /// </summary>
        private static readonly int ERR_MAX = 8;

        /// <summary>
        /// Content of file
        /// </summary>
        private char[] content;

        /// <summary>
        /// Cursor position in file
        /// </summary>
        private Position cursor;

        /// <summary>
        /// Real position of cursor
        /// </summary>
        private int position;

        /// <summary>
        /// Buffer for actually parsed token
        /// </summary>
        private StringBuilder buffer;

        /// <summary>
        /// Results of running lexer
        /// </summary>
        private IList<Token> results;

        /// <summary>
        /// Actually parsed line
        /// </summary>
        private string line;

        /// <summary>
        /// Path to file parsed by lexer
        /// </summary>
        private string file;

        /// <summary>
        /// State of lexer
        /// </summary>
        private LexerState state;

        /// <summary>
        /// Creates new parser from text input to tokens
        /// </summary>
        /// <param name="content">Content of file</param>
        /// <param name="file">Path to file (will be used only for messages)</param>
        public Lexer(string content, string file)
        {
            this.cursor = new Position();
            this.cursor.column = 1;
            this.cursor.line = 1;
            this.position = 0;
            this.file = file;
            this.content = this.ClearInput(content).ToCharArray();
            this.results = new System.Collections.Generic.List<Token>();
            this.buffer = new StringBuilder();
            this.state = LexerState.INITIAL;
        }

        

        /// <summary>
        /// Clears input from unexpected characters
        /// </summary>
        /// <param name="input">Input which will be cleared from unexpected characters</param>
        /// <returns>Input cleared from unexpected characters</returns>
        private string ClearInput(string input)
        {
            StringBuilder reti = new StringBuilder();
            foreach (char c in input.ToCharArray())
            {
                if (c < 128)
                {
                    reti.Append(c);
                }
            }
            return reti.ToString();
        }

        /// <summary>
        /// Runs lexer
        /// </summary>
        public void Run()
        {
            while (this.position < this.content.Length)
            {
                this.LoadLine();
                // Load string till whitespace to buffer
                while (this.position < this.content.Length && Char.IsWhiteSpace(this.content[this.position]) == false)
                {
                    if (Char.ToString(this.content[this.position]) == Tokens.STRING)
                    {
                        foreach (Token t in this.ParseElement(this.buffer.ToString()))
                        {
                            this.results.Add(t);
                        }
                        this.state = LexerState.STRING;
                        String str = this.LoadString();
                        if (str != null)
                        {
                            this.CursorLeft(str.Length + 1);
                            Token t = new Token(
                                                TokenType.LITERAL_STRING,
                                                Tokens.STRING + str + Tokens.STRING,
                                                this.cursor.line,
                                                this.cursor.column,
                                                this.line,
                                                this.file
                                                );
                            t.SetValue(str);
                            this.CursorRight(str.Length + 1);
                            this.results.Add(t);
                            this.buffer.Clear();
                        }
                    }
                    else
                    {
                        this.buffer.Append(this.content[this.position]);
                        if (this.buffer.ToString() == Tokens.COMMENT)
                        {
                            this.state = LexerState.COMMENT;
                            this.CursorLeft();
                            Lexer.Print("Found comment (line: " + this.cursor.line + ", column: " + this.cursor.column + ")", null);
                            break;
                        }
                    }
                    if (this.CursorRight() == false)
                    {
                        break;
                    }

                }
                if (this.state == LexerState.COMMENT)
                {
                    char previous = this.content[this.position];
                    while (this.position < this.content.Length)
                    {
                        string nlBuffer = new string(new char[] { previous, this.content[this.position] });
                        previous = this.content[this.position];
                        if (Lexer.isNewLine(nlBuffer) || Lexer.isNewLine(Char.ToString(previous)) || this.CursorRight() == false)
                        {
                            break;
                        }
                    }
                    this.state = LexerState.STANDARD;
                }
                else if (String.IsNullOrWhiteSpace(this.buffer.ToString()) == false)
                {
                    this.CursorLeft(this.buffer.ToString().Length);
                    IList<Token> element = this.ParseElement(this.buffer.ToString());
                    this.CursorRight(this.buffer.ToString().Length);
                    int i = 0;
                    foreach (Token t in element)
                    {
                        if (i == 0 && this.state == LexerState.INITIAL) // Make sure, that every file starts with '<?php'
                        {
                            if (t.GetTokenType() != TokenType.PROG_START)
                            {
                                Lexer.PrintError("Unexpected token found! Expected start of program ('<?php')!", t);
                            }
                            else
                            {
                                this.state = LexerState.STANDARD;
                            }
                        }
                        this.results.Add(t);
                        i++;
                    }
                }
                this.buffer.Clear();            
                if (this.CursorRight() == false)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Loads string from source code
        /// </summary>
        /// <returns>String loaded from source code</returns>
        private string LoadString()
        {
            this.buffer.Clear();
            int startCol = this.cursor.column;
            int startLine = this.cursor.line;
            if (this.state == LexerState.STRING)
            {
                if (Char.ToString(this.content[this.position]) == Tokens.STRING)
                {
                    while (this.CursorRight() && Char.ToString(this.content[this.position]) != Tokens.STRING)
                    {
                        this.buffer.Append(this.content[this.position]);
                    }
                    if (Char.ToString(this.content[this.position]) != Tokens.STRING)
                    {
                        Lexer.PrintError("Unexpected end of file during parsing string literal (started at line:" + startLine + ", column: " + startCol + ")!", null);
                    }
                }
            }
            return this.buffer.ToString();
        }

        /// <summary>
        /// Checks, whether string represents new line character
        /// </summary>
        /// <param name="s">String which will be checked</param>
        /// <returns>TRUE if string represents new line, FALSE otherwise</returns>
        private static bool isNewLine(string s)
        {
            return(
                s == Environment.NewLine ||
                s == Char.ToString('\r') ||
                s == Char.ToString('\n')
                );
        }

        /// <summary>
        /// Loads actually parsed line
        /// </summary>
        private void LoadLine()
        {
            int actualPosition = this.position;
            while (this.position > 0 && this.content[this.position] != '\n')
            {
                this.position--;
            }
            int start = this.position;
            if (this.content[start] == '\n')
            {
                start++;
            }
            this.position = actualPosition;
            while (this.position < this.content.Length && this.content[this.position] != '\n')
            {
                this.position++;
            }
            int end = this.position;
            if (end >= this.content.Length)
            {
                end = this.content.Length - 1;
            }
            if (this.content[end] == '\n')
            {
                end--;
                end--;
            }
            this.position = actualPosition;
            StringBuilder line = new StringBuilder();
            if (end - start >= 1)
            {
                for (int i = start; i <= end; i++)
                {
                    line.Append(this.content[i]);
                }
            }
            this.line = line.ToString();
        }


        /// <summary>
        /// Parses text element
        /// <param name="element">Text element from which tokens will be parsed</param>
        /// </summary>
        private IList<Token> ParseElement(string element)
        {
            IList<Token> reti = new System.Collections.Generic.List<Token>();
            if (element.Length > 0)
            {
                //Console.WriteLine("\nParsing element '" + element + "':");
                if (Tokens.IsToken(element))
                {
                    reti.Add(new Token(
                                        Tokens.GetTokenType(element),
                                        element,
                                        this.cursor.line,
                                        this.cursor.column,
                                        this.line,
                                        this.file
                                    ));
                    //Console.WriteLine(">>> Element is token itself (type " + Tokens.GetTokenType(element) + ")");
                }
                else
                {
                    if (element.StartsWith(Tokens.VARIABLE))
                    {
                        int startCol = this.cursor.column;
                        int startLine = this.cursor.line;
                        StringBuilder varBuffer = new StringBuilder();
                        int idx = 0;
                        char[] elemArray = element.ToCharArray();
                        idx++;
                        while (idx < elemArray.Length && Char.IsLetter(elemArray[idx]))
                        {
                            varBuffer.Append(elemArray[idx]);
                            idx++;
                        }
                        Token t = new Token(
                                                TokenType.VARIABLE,
                                                Tokens.VARIABLE + varBuffer.ToString(),
                                                startLine,
                                                startCol,
                                                this.line,
                                                this.file
                                );
                        t.SetValue(varBuffer.ToString());
                        reti.Add(t);
                        IList<Token> subreti = this.ParseElement(element.Substring(idx));
                        foreach (Token tok in subreti)
                        {
                            reti.Add(tok);
                        }
                    }
                    else
                    {

                        int start = this.position;
                        bool[] added = new bool[element.Length];
                        int retiLen = 0;
                        for (int i = 0; i < added.Length; i++)
                        {
                            added[i] = false;
                        }
                        // Search for substrings from longest possible token
                        for (int len = Math.Min(Tokens.GetLongest().Length, element.Length); len >= 1; len--)
                        {
                            //Console.WriteLine("> Checking for tokens length " + len);
                            if (len <= element.Length)
                            {
                                // Searching for substrings of defined lengths from beginning
                                for (int offset = 0; offset <= element.Length - len; offset++)
                                {
                                    StringBuilder attempt = new StringBuilder();
                                    for (int character = 0; character < len; character++)
                                    {
                                        attempt.Append(element.ToCharArray()[offset + character]);
                                    }

                                    if (Tokens.IsToken(attempt.ToString()))
                                    {
                                        this.CursorRight(offset);
                                        //Console.WriteLine(">> Found token '" + attempt.ToString() + "' (type " + Tokens.GetTokenType(attempt.ToString()) + ")");
                                        reti.Add(new Token(
                                                Tokens.GetTokenType(attempt.ToString()),
                                                attempt.ToString(),
                                                this.cursor.line,
                                                this.cursor.column,
                                                this.line,
                                                this.file
                                            ));
                                        this.CursorLeft(offset);
                                        retiLen = attempt.Length;
                                        for (int i = offset; i < offset + len; i++)
                                        {
                                            added[i] = true;
                                        }
                                        break;
                                    }
                                }
                                if (reti.Count > 0)
                                {
                                    break;
                                }
                            }
                        }
                        if (reti.Count > 0)
                        {
                            StringBuilder s1 = new StringBuilder();
                            bool _added = false;
                            for (int i = 0; i < added.Length; i++)
                            {
                                if (added[i] == false)
                                {
                                    s1.Append(element.ToCharArray()[i]);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            _added = false;
                            StringBuilder s2 = new StringBuilder();
                            for (int i = 0; i < added.Length; i++)
                            {
                                if (added[i] && _added == false)
                                {
                                    _added = true;
                                }
                                else if (_added && added[i] == false)
                                {
                                    s2.Append(element.ToCharArray()[i]);
                                }
                            }
                            IList<Token> l1 = this.ParseElement(s1.ToString());
                            for (int i = l1.Count - 1; i >= 0; i--)
                            {
                                reti.Insert(0, l1[i]);
                            }
                            this.CursorRight(element.Length - s2.Length);
                            IList<Token> l2 = this.ParseElement(s2.ToString());
                            this.CursorLeft(element.Length - s2.Length);
                            foreach (Token t in l2)
                            {
                                reti.Add(t);
                            }
                        }
                        else
                        {
                            float floatVal = float.NaN;
                            int intVal = int.MinValue;
                            CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                            ci.NumberFormat.CurrencyDecimalSeparator = ".";
                            if (int.TryParse(element, NumberStyles.Any, ci, out intVal))
                            {
                                Token t = new Token(
                                    TokenType.LITERAL_INTEGER,
                                    element,
                                    this.cursor.line,
                                    this.cursor.column,
                                    this.line,
                                    this.file
                                );
                                t.SetValue(element);
                                reti.Add(t);
                            }
                            else if (float.TryParse(element, NumberStyles.Any, ci, out floatVal))
                            {
                                Token t = new Token(
                                    TokenType.LITERAL_FLOAT,
                                    element,
                                    this.cursor.line,
                                    this.cursor.column,
                                    this.line,
                                    this.file
                                );
                                t.SetValue(element);
                                reti.Add(t);
                            }
                            else if (Regex.IsMatch(element, @"^[a-zA-Z]+$")) // Contains letters only
                            {
                                Token t = new Token(
                                    TokenType.FUNCTION_IDENTIFIER,
                                    element,
                                    this.cursor.line,
                                    this.cursor.column,
                                    this.line,
                                    this.file
                                );
                                t.SetValue(element);
                                reti.Add(t);
                            }
                            else
                            {
                                Token t = new Token(
                                        TokenType.UNKNOWN,
                                        element,
                                        this.cursor.line,
                                        this.cursor.column,
                                        this.line,
                                        this.file
                                    );
                                t.SetValue(element);
                                reti.Add(t);
                                Lexer.PrintError("Found unknown token!", t);
                                //Console.WriteLine(">>> Added unknown  '" + element + "'");
                            }
                        }
                    }
                }
            }
            return reti;
            
        }

        /// <summary>
        /// Prints results of lexer
        /// </summary>
        public void PrintResults()
        {
            foreach (Token t in this.results)
            {
                Lexer.Print(null, t);
            }
        }
        
        /// <summary>
        /// Moves cursor to right side
        /// </summary>
        /// <returns>TRUE, if cursor has been moved, FALSE otherwise</returns>
        private bool CursorRight()
        {
            bool reti = false;
            if (this.position < this.content.Length)
            {
                this.position++;
                this.cursor.column++;
                reti = true;
                if (this.position < this.content.Length)
                {
                    while (this.content[this.position] == '\n' && this.position < this.content.Length - 1)
                    {
                        this.cursor.column = 1;
                        this.cursor.line++;
                        this.position++;
                    }
                }
            }
            return reti;
        }

        /// <summary>
        /// Moves cursor to right side
        /// <param name="i">Number of moves</param>
        /// </summary>
        /// <returns>TRUE, if cursor has been moved, FALSE otherwise</returns>
        private bool CursorRight(int i)
        {
            int counter = 0;
            for (int c = 0; c < i; c++)
            {
                if (this.CursorRight())
                {
                    counter++;
                }
            }
            return counter == i;
        }

        /// <summary>
        /// Moves cursor to left side
        /// </summary>
        /// <returns>TRUE, if cursor has been moved, FALSE otherwose</returns>
        private bool CursorLeft()
        {
            bool reti = false;
            if (this.position > 0)
            {
                this.position--;
                this.cursor.column--;
                reti = true;
                while (this.content[this.position] == '\n' && this.position > 0)
                {
                    this.cursor.column = 1;
                    this.cursor.line--;
                    this.position--;
                }
            }
            return reti;
        }

        /// <summary>
        /// Moves cursor to left side
        /// <param name="i">Number of moves</param>
        /// </summary>
        /// <returns>TRUE, if cursor has been moved, FALSE otherwise</returns>
        private bool CursorLeft(int i)
        {
            int counter = 0;
            for (int c = 0; c < i; c++)
            {
                if (this.CursorLeft())
                {
                    counter++;
                }
            }
            return counter == i;
        }

        /// <summary>
        /// Gets results of lexer
        /// </summary>
        /// <returns>Stream of tokens</returns>
        public TokenStream GetResults()
        {
            return new TokenStream(this.results);
        }

        /// <summary>
        /// Prints message to console
        /// </summary>
        /// <param name="message">Message which will be printed to console</param>
        /// <param name="token">Token providing more information</param>
        private static void Print(string message, Token token)
        {
            if (Program.DEBUG)
            {
                Lexer.COUNTER++;
                Console.Write("(" + String.Format("{00:0#}", Lexer.COUNTER) + ") ");
                if (message != null)
                {
                    Console.WriteLine(message);
                    if (token != null)
                    {
                        Console.Write("      ");
                    }
                }
                if (token != null)
                {
                    Console.WriteLine(token.ToString());
                }
            }
        }

        /// <summary>
        /// Prints error message to console
        /// </summary>
        /// <param name="message">Message which will be printed to console</param>
        /// <param name="token">Token which caused error</param>
        private static void PrintError(string message, Token token)
        {
            if (Lexer.ERR_COUNTER < Lexer.ERR_MAX)
            {
                Lexer.COUNTER++;
                Lexer.ERR_COUNTER++;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("(" + String.Format("{00:0#}", Lexer.COUNTER) + ") " + message);
                if (token != null)
                {
                    string line = "     File " + token.GetFile() + " [line: " + token.GetRow() + "; column: " + token.GetColumn() + "]: ";
                    Console.WriteLine(line + token.GetLine());
                    for (int i = 0; i < line.Length; i++)
                    {
                        Console.Write(" ");
                    }
                    for (int i = 0; i < token.GetColumn() - 1; i++)
                    {
                        Console.Write(" ");
                    }
                    Console.Write("˄");
                    for (int i = 1; i < token.GetContent().Length; i++)
                    {
                        Console.Write("ˉ");
                    }
                    Console.WriteLine();
                }
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Maximum errors (" + Lexer.ERR_MAX + ") printed! Program exits!");
                Console.ForegroundColor = ConsoleColor.Gray;
                Program.Exit();
            }
        }
    }
}

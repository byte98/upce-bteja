using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace HW04.Lexer
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
        /// Handler of progress of lexer
        /// </summary>
        private IProgressHandler progressHandler;

        /// <summary>
        /// Actually parsed line
        /// </summary>
        private string line;

        /// <summary>
        /// Path to file parsed by lexer
        /// </summary>
        private string file;

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
        /// <param name="progressHandler">Handler of progress updates</param>
        public void Run(IProgressHandler progressHandler = null)
        {
            this.progressHandler = progressHandler;
            while (this.position < this.content.Length)
            {
                this.LoadLine();
                // Load string till whitespace to buffer
                while (this.position < this.content.Length && Char.IsWhiteSpace(this.content[this.position]) == false)
                {
                    this.buffer.Append(this.content[this.position]);
                    if (this.CursorRight() == false)
                    {
                        break;
                    }
                }
                if (String.IsNullOrWhiteSpace(this.buffer.ToString()) == false)
                {
                    this.CursorLeft(this.buffer.ToString().Length);
                    IList<Token> element = this.ParseElement(this.buffer.ToString());
                    this.CursorRight(this.buffer.ToString().Length);
                    foreach (Token t in element)
                    {
                        this.results.Add(t);
                    }
                }
                this.buffer.Clear();
                this.UpdateProgress();                
                if (this.CursorRight() == false)
                {
                    break;
                }
            }
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
        /// <param name="element">Text element from which tokens will be parsed</param
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
                        //Console.WriteLine(">>> Added unknown  '" + element + "'");
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
            int counter = 1;
            foreach (Token t in this.results)
            {
                Console.WriteLine("(" + String.Format("{00:0#}", counter) + ") " + t.ToString());
                counter++;
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
        /// Updates progress of lexer
        /// </summary>
        private void UpdateProgress()
        {
            if (this.progressHandler != null)
            {
                this.progressHandler.UpdateProgress(
                        (float)((float)this.position / (float)this.content.Length) * 100
                    );
            }
        }

        /// <summary>
        /// Gets results of lexer
        /// </summary>
        /// <returns>Stream of tokens</returns>
        public TokenStream GetResults()
        {
            return new TokenStream(this.results);
        }
    }
}

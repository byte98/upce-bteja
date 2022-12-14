using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW05.Lexer
{
    /// <summary>
    /// class representing stream of tokens from lexer
    /// </summary>
    public class TokenStream
    {
        /// <summary>
        /// Array with all tokens stored in stream
        /// </summary>
        private Token[] tokens;

        /// <summary>
        /// Index of actual token
        /// </summary>
        private int index = -1;

        /// <summary>
        /// Creates new stream of tokens
        /// </summary>
        /// <param name="tokens">Tokens which will be stored in stream</param>
        public TokenStream(Token[] tokens)
        {
            this.tokens = tokens;
        }

        /// <summary>
        /// Creates new stream of tokens
        /// </summary>
        /// <param name="tokens">Tokens which will be stored in stream</param>
        public TokenStream(IList<Token> tokens)
        {
            this.tokens = tokens.ToArray();
        }

        // <summary>
        /// Checks, whether there is another token
        /// </summary>
        /// <returns>TRUE, if there is another token, FALSE otherwise</returns>
        public bool HasNext()
        {
            bool reti = false;
            if (this.index < this.tokens.Length - 1)
            {
                reti = true;
            }
            return reti;
        }

        /// <summary>
        /// Gets next token without changing actual position
        /// </summary>
        /// <returns>Next token or NULL, if there is no such a token</returns>
        public Token ObserveNext()
        {
            Token reti = null;
            if (this.HasNext())
            {
                reti = this.tokens[this.index + 1];
            }
            return reti;
        }

        /// <summary>
        /// Gets next token and changes actual position to next one
        /// </summary>
        /// <returns>Next token or NULL, if there is no such a token</returns>
        public Token GetNext()
        {
            Token reti = this.ObserveNext();
            if (reti != null)
            {
                this.index++;
            }
            return reti;
        }
    }
}

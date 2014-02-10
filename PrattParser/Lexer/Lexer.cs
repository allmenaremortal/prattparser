using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrattParser
{

    /// <summary>
    /// The Lexer class implements IEnumerable<Token>, yielding a new Token from its
    /// mText string as it is iterated through.
    /// </summary>
    public class Lexer : IEnumerable<Token>
    {
        // This is the string from which tokens are lexed and returned through IEnumerable<Token>.
        private string mText;

        #region Constructors

        public Lexer(string text)
        {
            mText = text;
        }

        #endregion

        #region IEnumerator<Token> methods

        /// <summary>
        /// Return an IEnumerator<Token> which is capable of returning tokens from the string of the lexer.
        /// </summary>
        public IEnumerator<Token> GetEnumerator()
        {
            return new LexerEnumerator(mText);
        }

        #endregion

        #region IEnumerator methods

        /// <summary>
        /// The non-generic IEnumerator interface simply returns the enumerator from
        /// the generic IEnumerator interface.
        /// </summary>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

    }
}

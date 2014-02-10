using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PrattParser
{
    /// <summary>
    /// This class implements iteration through the tokens of a string. Its only
    /// exposure is through IEnumerator<Token>.
    /// </summary>
    public class LexerEnumerator : IEnumerator<Token>
    {

        #region Fields

        private string mText;                               // The text from which tokens are lexed.
        private int mIndex;                                 // The current index of the string from where tokens are lexed.
        private bool isBeforeFirstElement = true;           // The enumerator initially starts before the first token.

        // Array of all characters which count as whitespace.
        private static char[] mWhiteSpaceCharArray = { ' ', '\t', '\r' };

        // Reserved keywords. Note that some of these reserved keywords can be part
        // of larger tokens, in the sense that "else" can be part of the variable
        // identifier "elsewhereToBeFound".
        private static Dictionary<string, TokenType> reservedStringsToTokenTypes = new Dictionary<string, TokenType>
        {
            {"+", TokenType.PLUS},
            {"-", TokenType.MINUS},
            {"/", TokenType.SLASH},
            {"*", TokenType.ASTERISK},
            {"^", TokenType.CARET},

            {"!", TokenType.NOT},

            {"if", TokenType.IF},
            {"then", TokenType.THEN},
            {"else", TokenType.ELSE},

            {"==", TokenType.EQUAL},
            {"<", TokenType.LESS_THAN},
            {"<=", TokenType.LESS_THAN_OR_EQUAL},
            {">", TokenType.GREATER_THAN},
            {">=", TokenType.GREATER_THAN_OR_EQUAL},
            {"!=", TokenType.NOT_EQUAL},

            {"(", TokenType.LEFT_PAREN},
            {")", TokenType.RIGHT_PAREN}

        };

        #endregion

        #region Public interface

        /// <summary>
        ///  Retrieves all tokens from the string.
        /// </summary>
        public List<Token> getAllTokens()
        {
            List<Token> tokenList = new List<Token>();

            do
            {
                tokenList.Add(Current);
                MoveNext();
            }
            while (tokenList.Last().TypeOfToken != TokenType.EOF);

            return tokenList;
        }

        #endregion

        #region Constructors

        public LexerEnumerator(string text)
        {
            if (text == null)
            {
                throw new Exception("Cannot instantiate LexerEnumerator with null string");
            }

            mText = text;
        }

        #endregion

        #region Maximal type substring getters

        /// <summary>
        /// Retrieve token string candidate as a maximal length number from the string.
        /// </summary>
        private string getMaximalNumberString()
        {
            string numberString = "";
            int temporaryStringIndex = mIndex;

            // If the first character of the candidate number string does not match
            // [0-9], the maximal number string is "".
            if (!Regex.IsMatch(mText[temporaryStringIndex].ToString(), "[0-9]"))
            {
                return numberString;
            }

            numberString = mText[temporaryStringIndex].ToString();
            temporaryStringIndex++;

            // Add characters to the number string for as long as these characters match [0-9.]
            while (temporaryStringIndex < mText.Length && Regex.IsMatch(mText[temporaryStringIndex].ToString(), "[0-9.]"))
            {
                numberString = numberString + mText[temporaryStringIndex];
                temporaryStringIndex++;
            }

            return numberString;
        }

        /// <summary>
        /// Retrieve token string candidate as a maximal length name from the string.
        /// </summary>
        private string getMaximalNameString()
        {
            string nameString = "";
            int temporaryStringIndex = mIndex;

            // If the first character of the candidate name string does not match
            // [a-zA-Z_], the maximal name string is "".
            if (!Regex.IsMatch(mText[temporaryStringIndex].ToString(), "[a-zA-z_]"))
            {
                return nameString;
            }

            nameString = mText[temporaryStringIndex].ToString();
            temporaryStringIndex++;

            // Add characters to the number string for as long as these characters match [a-zA-z0-9_]
            while (temporaryStringIndex < mText.Length && Regex.IsMatch(mText[temporaryStringIndex].ToString(), "[a-zA-z0-9_]"))
            {
                nameString = nameString + mText[temporaryStringIndex];
                temporaryStringIndex++;
            }

            return nameString;
        }

        #endregion

        #region Token and string related bool functions

        /// <summary>
        /// Return true if the next token could is a name.
        /// </summary>
        private bool isNextTokenContiguousName()
        {
            char currentChar = mText[mIndex];

            // We identify something as a possible name if it begins with a letter or an underscore.
            Match match = Regex.Match(currentChar.ToString(), "[a-zA-Z_]");

            return match.Success;
        }

        /// <summary>
        /// Returns true if the given string matches a reserved keyword.
        /// </summary>
        private bool isReservedKeyword(string str)
        {
            return reservedStringsToTokenTypes.ContainsKey(str);
        }

        /// <summary>
        /// Return true if the next token is a number.
        /// </summary>
        private bool isNextTokenContiguousNumber()
        {
            char currentChar = mText[mIndex];

            // We identify something as a possible number if it begins with a single digit number.
            Match match = Regex.Match(currentChar.ToString(), "[0-9.]");

            return match.Success;
        }

        /// <summary>
        /// Return true if the string can be parsed to an integer.
        /// </summary>
        private bool isInteger(string str)
        {
            int dummy;
            return int.TryParse(str, out dummy);
        }


        /// <summary>
        /// Return true if the string can be parsed to a double.
        /// </summary>
        private bool isDouble(string str)
        {
            double dummy;
            return double.TryParse(str, out dummy);
        }

        #endregion

        #region tryGetToken type methods

        // Try to retrieve a token which is a reserved keyword by looking forward
        // from the current mIndex position, using the rule that the maximal (longest)
        // token has first priority (e.g., in "==2", "==" has priority over "=" as
        // the first token of the string).

        /// <summary>
        /// Try to retrieve a reserved keyword token.
        /// </summary>
        private bool tryGetReservedKeywordToken(out Token token)
        {
            string tokenCandidate = "";
            TokenType tokenType;

            foreach (string reservedString in reservedStringsToTokenTypes.Keys)
            {
                // If the length of the currently searched for reserved string is
                // longer than the remaining part of the text, the next token cannot
                // be this reserved string.
                if (reservedString.Length > mText.Length - mIndex)
                {
                    continue;
                }

                // If reservedString matches a substring from the current index of
                // the text, and the reserved string is longer than the previously
                // found token candidate (the empty string if no previous candidate),
                // we update the token candidate string.
                if (reservedString == mText.Substring(mIndex, reservedString.Length) && reservedString.Length > tokenCandidate.Length)
                {
                    tokenCandidate = reservedString;
                    continue;
                }
            }

            if (tokenCandidate == "")
            {
                token = null;
                return false;
            }

            // If we get to this point, tokenCandidate contains the maximal token candidate
            // matching a reserved keyword. Return the token corresponding to this keyword
            // and return true.
            reservedStringsToTokenTypes.TryGetValue(tokenCandidate, out tokenType);
            token = new Token(tokenType, tokenCandidate);
            return true;
        }


        /// <summary>
        /// Try to retrieve a variable identifier token.
        /// </summary>
        private bool tryGetIdentifierToken(out Token token)
        {
            string tokenCandidate;

            // A necessary requirement for being an identifier token is that the token
            // string is a contiguous name.
            if (!isNextTokenContiguousName())
            {
                token = null;
                return false;
            }

            // If we get to this point, the next token must be a contiguous name, which
            // leaves either a reserved keyword or an identifier as the possible tokens.
            // We first retrieve the token string.
            tokenCandidate = getMaximalNameString();

            // Reserved words have priority over variable identifiers when tokenizing.
            // If the token string is a reserved keyword, we cannot retrieve an identifier
            // type token.
            if (isReservedKeyword(tokenCandidate))
            {
                token = null;
                return false;
            }

            // If the token was not a reserved keyword, it must be a variable identifier.
            token = new Token(TokenType.IDENTIFIER, tokenCandidate);
            return true;
        }

        /// <summary>
        /// Try to retrieve an integer token.
        /// </summary>
        private bool tryGetIntegerToken(out Token token)
        {
            string tokenCandidate;

            // A necessary requirement for being an integer token is that the token
            // string is a contiguous numer.
            if (!isNextTokenContiguousNumber())
            {
                token = null;
                return false;
            }

            // If we get to this point, the next token must be a contiguous number, which
            // leaves either an integer or a double as the possible tokens.
            tokenCandidate = getMaximalNumberString();

            // If the token candidate is not an integer, return false.
            if (!isInteger(tokenCandidate))
            {
                token = null;
                return false;
            }

            // Otherwise, return the integer token.
            token = new Token(TokenType.CONST_INT, tokenCandidate);
            return true;
        }

        /// <summary>
        /// Try to retrieve a double token.
        /// </summary>
        private bool tryGetDoubleToken(out Token token)
        {
            string tokenCandidate;

            // A necessary requirement for being a double token is that the token
            // string is a contiguous numer.
            if (!isNextTokenContiguousNumber())
            {
                token = null;
                return false;
            }

            // If we get to this point, the next token must be a contiguous number, which
            // leaves either an integer or a double as the possible tokens.
            tokenCandidate = getMaximalNumberString();

            if (isInteger(tokenCandidate))
            {
                token = null;
                return false;
            }

            if (!isDouble(tokenCandidate))
            {
                token = null;
                return false;
            }

            token = new Token(TokenType.CONST_DOUBLE, tokenCandidate);
            return true;
        }

        #endregion

        #region Miscellaneous

        /// <summary>
        /// Returns true if _char is a whitespace character.
        /// </summary>
        private static bool isWhiteSpace(char _char)
        {
            // Loop through the array of whitespace chars and compare the given
            // character. If we find an equal char in the array, the given char
            // is a whitespace char and we return true.
            foreach (char whitespacechar in mWhiteSpaceCharArray)
            {
                if (_char == whitespacechar)
                {
                    return true;
                }
            }

            // If we get to this point, the char is not in the whitespace char array.
            return false;
        }

        /// <summary>
        /// Advance the string index past all whitespace characters.
        /// </summary>
        private void consumeWhiteSpace()
        {
            // Advance the string index for as long as we have not reached the end of the
            // string and for as long as the current character is a whitespace character.
            while (mIndex < mText.Length && isWhiteSpace(mText[mIndex]))
            {
                mIndex++;
            }
        }

        #endregion

        #region IEnumerator<Token> methods

        /// <summary>
        /// Move the index to the beginning of the next token and return true. We always return true,
        /// as we implement the Current property by returning an EOF token if no more tokens are left
        /// to be identified in the mText string.
        /// </summary>
        public bool MoveNext()
        {
            if (isBeforeFirstElement)
            {
                isBeforeFirstElement = false;
                return true;
            }
            else
            {
                mIndex += Current.Text.Length;
                return true;
            }
        }

        /// <summary>
        /// Return the current token in the enumerator. Note: No set method.
        /// </summary>
        public Token Current
        {
            get
            {
                if (isBeforeFirstElement)
                {
                    throw new InvalidOperationException("An attempt was made to obtain Current element from a LexerEnumerator " +
                                                        "while positioned before the beginning of the sequence.");
                }

                Token token;
                int origIndex = mIndex;

                // At this point, we know that mIndex is the index of mText for the first
                // character of the next token. First consume all whitespace characters in front
                // of the current index (otherwise, this would have to be done in each of the
                // later token searches).
                consumeWhiteSpace();

                // If we have reached the end of the string, return an EOF token.
                if (mIndex >= mText.Length)
                {
                    return new Token(TokenType.EOF, "");
                }

                // We now try to obtain a nontrivial token. There are four main categories of
                // tokens: variable identifiers, integer literals, double literals and
                // reserved keywords. When we identify a token, we advance the string index
                // and return the token.

                if (tryGetIdentifierToken(out token))
                {
                    return token;
                }

                if (tryGetIntegerToken(out token))
                {
                    return token;
                }

                if (tryGetDoubleToken(out token))
                {
                    return token;
                }

                // A reserved keyword token can be part of a variable identifier token, in the
                // sense that "if" can be part of "ifxyz" in "ifxyx == 2". However, if the
                // next token is a variable identifier, we would already have exited the routine,
                // and we therefore do not need to worry about this.
                if (tryGetReservedKeywordToken(out token))
                {
                    return token;
                }

                // If none of the above if statements led to returning a token, we cannot obtain
                // a new token from the string.
                throw new Exception(String.Format("Could not identify token from {0}", mText.Substring(origIndex)));
            }
        }

        /// <summary>
        /// The enumerator is reset by setting the string index mIndex to zero.
        /// </summary>
        public void Reset()
        {
            isBeforeFirstElement = true;
            mIndex = 0;
        }

        #endregion

        #region IDisposable methods

        public void Dispose()
        {
        }

        #endregion

        #region IEnumerator methods

        object System.Collections.IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        #endregion

    }
}

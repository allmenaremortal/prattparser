using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrattParser
{
    /// <summary>
    /// This enum defines the possible types of tokens.
    /// </summary>
    public enum TokenType
    {
        // Math operators.
        PLUS,
        MINUS,
        SLASH,
        ASTERISK,
        CARET,

        // Prefix operators.
        NOT,

        // Control flow keywords.
        IF,
        THEN,
        ELSE,

        // Comparison operators.
        EQUAL,
        LESS_THAN,
        LESS_THAN_OR_EQUAL,
        GREATER_THAN,
        GREATER_THAN_OR_EQUAL,
        NOT_EQUAL,

        // Parentheses.
        LEFT_PAREN,
        RIGHT_PAREN,

        // Special types.
        CONST_INT,
        CONST_DOUBLE,
        IDENTIFIER,

        // End-of-file token.
        EOF
    };

    /// <summary>
    /// Encapsulates a single token obtained from lexing a string.
    /// </summary>
    public class Token
    {
        private TokenType mType;
        private string mText;

        public Token(TokenType type, string text)
        {
            mType = type;
            mText = text;
        }

        public TokenType TypeOfToken { get { return mType; } set { mType = TypeOfToken; } }

        public string Text { get { return mText; } set { mText = Text; } }

        public override string ToString()
        {
            return String.Format("PrattParser.Token (type {0}, string {1})", mType, mText);
        }
    }
}

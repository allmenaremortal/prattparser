using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrattParser
{
    public class Grammar
    {
        // These dictionaries give the unary and binary node parselets for the different
        // existing token types, whenever applicable.
        private Dictionary<TokenType, PrefixNodeParselet> prefixNodeParselets;
        private Dictionary<TokenType, InfixNodeParselet> infixNodeParselets;

        public Grammar()
        {
            prefixNodeParselets = new Dictionary<TokenType, PrefixNodeParselet>();
            infixNodeParselets = new Dictionary<TokenType, InfixNodeParselet>();

            prefixNodeParselets.Add(TokenType.NOT, new NotUnaryNodeParselet());
            prefixNodeParselets.Add(TokenType.CONST_INT, new ConstIntUnaryNodeParselet());

            infixNodeParselets.Add(TokenType.PLUS, new PlusBinaryNodeParselet());
            infixNodeParselets.Add(TokenType.ASTERISK, new MultiplicationBinaryNodeParselet());
            infixNodeParselets.Add(TokenType.MINUS, new MinusBinaryNodeParselet());
            infixNodeParselets.Add(TokenType.SLASH, new DivisionBinaryNodeParselet());
            infixNodeParselets.Add(TokenType.CARET, new ExponentiationBinaryNodeParselet());
        }

        /// <summary>
        /// Try to retrieve a prefix node parselet for the token.
        /// </summary>
        /// <param name="token">The token for which to obtain a prefix node parselet.</param>
        /// <param name="unaryNodeParselet">The resulting prefix node parselet if try was successful.</param>
        /// <returns>True if the retrieval was successful.</returns>
        public bool tryGetInfixNodeParselet(Token token, out PrefixNodeParselet unaryNodeParselet)
        {
            return prefixNodeParselets.TryGetValue(token.TypeOfToken, out unaryNodeParselet);
        }

        /// <summary>
        /// Try to retrieve an infix node parselet for the token.
        /// </summary>
        /// <param name="token">The token for which to obtain an infix node parselet.</param>
        /// <param name="binaryNodeParselet">The resulting infix node parselet if try was successful.</param>
        /// <returns>True if the retrieval was successful.</returns>
        public bool tryGetInfixNodeParselet(Token token, out InfixNodeParselet binaryNodeParselet)
        {
            return infixNodeParselets.TryGetValue(token.TypeOfToken, out binaryNodeParselet);
        }
    }
}

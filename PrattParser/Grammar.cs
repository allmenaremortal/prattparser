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
        private Dictionary<TokenType, PrefixNodeParselet> unaryNodeParselets;
        private Dictionary<TokenType, InfixNodeParselet> binaryNodeParselets;

        public Grammar()
        {
            unaryNodeParselets = new Dictionary<TokenType, PrefixNodeParselet>();
            binaryNodeParselets = new Dictionary<TokenType, InfixNodeParselet>();

            unaryNodeParselets.Add(TokenType.NOT, new NotUnaryNodeParselet());
            unaryNodeParselets.Add(TokenType.CONST_INT, new ConstIntUnaryNodeParselet());

            binaryNodeParselets.Add(TokenType.PLUS, new PlusBinaryNodeParselet());
            binaryNodeParselets.Add(TokenType.ASTERISK, new MultiplicationBinaryNodeParselet());
            binaryNodeParselets.Add(TokenType.MINUS, new MinusBinaryNodeParselet());
            binaryNodeParselets.Add(TokenType.SLASH, new DivisionBinaryNodeParselet());
            binaryNodeParselets.Add(TokenType.CARET, new ExponentiationBinaryNodeParselet());
        }

        /// <summary>
        /// Try to retrieve a unary node parselet for token.
        /// </summary>
        /// <param name="token">The token for which to obtain a unary node parselet.</param>
        /// <param name="unaryNodeParselet">The resulting unary node parselet if try was successful.</param>
        /// <returns>True of the retrieval was successful.</returns>
        public bool tryGetUnaryNodeParselet(Token token, out PrefixNodeParselet unaryNodeParselet)
        {
            return unaryNodeParselets.TryGetValue(token.TypeOfToken, out unaryNodeParselet);
        }

        public bool tryGetBinaryNodeParselet(Token token, out InfixNodeParselet binaryNodeParselet)
        {
            return binaryNodeParselets.TryGetValue(token.TypeOfToken, out binaryNodeParselet);
        }
    }
}

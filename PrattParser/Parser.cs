using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrattParser
{
    /// <summary>
    /// Static class describing precedence levels. This is not an enum, since we'd like to be able
    /// to add and substract levels of precedence.
    /// </summary>
    public static class Precedence
    {
        public static int INITIAL = 0;
        public static int CONDITIONAL = 1;
        public static int SUM = 2;
        public static int PRODUCT = 3;
        public static int EXPONENT = 4;
        public static int PREFIX = 5;
        public static int POSTFIX = 6;
        public static int CALL = 7;
    }

    /// <summary>
    /// Parser capable of holding an IEnumerator<Token> stream of tokens and a grammar, and parsing
    /// the stream according to the grammar.
    /// </summary>
    public class Parser
    {
        IEnumerator<Token> tokens;
        Grammar grammar;

        public Parser(IEnumerator<Token> _tokens, Grammar _grammar)
        {
            tokens = _tokens;
            grammar = _grammar;
        }

        public IEnumerator<Token> Tokens { get { return tokens; } }

        /// <summary>
        /// Parse the token stream into an expression tree.
        /// </summary>
        /// <returns>An expression tree corresponding to the token stream.</returns>
        public Expression parse()
        {
            // By default, we expect the token stream to be positioned one token before
            // the initial token, in accordance with the IEnumerable interface standards.
            // Move the token stream to the first token.
            tokens.MoveNext();

            // Parse the token stream with minimal precedence level for stopping.
            return parseWithPrecedence(Precedence.INITIAL);
        }

        private int getPrecedence(Token token)
        {
            switch (token.TypeOfToken)
            {
                case TokenType.PLUS:
                    return Precedence.SUM;
                case TokenType.MINUS:
                    return Precedence.SUM;
                case TokenType.SLASH:
                    return Precedence.PRODUCT;
                case TokenType.ASTERISK:
                    return Precedence.PRODUCT;
                case TokenType.CARET:
                    return Precedence.EXPONENT;
                case TokenType.CONST_INT:
                    return Precedence.CALL;
                case TokenType.EOF:
                    return Precedence.INITIAL;

            }

            return Precedence.INITIAL;

            
        }

        private int precedenceOfCurrentToken()
        {
            return getPrecedence(tokens.Current);
        }

        /// <summary>
        /// Parse the expression owned by the parser until a token with lower precedence is reached.
        /// </summary>
        /// <param name="precedence">The maximum precedence level required for stopping parsing.</param>
        /// <returns>An expression tree based on the parsed expression.</returns>
        public Expression parseWithPrecedence(int precedence)
        {
            Token token;
            PrefixNodeParselet unaryNodeParselet;
            InfixNodeParselet binaryNodeParselet;

            // We assume that all binary operators are given as infix operators, so an
            // expression cannot start with a token for a binary node. Read a token
            // from the stream and retrieve a corresponding unary node parselet.
            token = tokens.Current;
            if (!grammar.tryGetUnaryNodeParselet(token, out unaryNodeParselet))
            {
                throw new Exception("Failed to obtain PrefixNodeParselet");
            }

            // Move the parser forward. When this is done, the parser represents the
            // as of yet unparsed expression to the right of the Token token. We let
            // leftExpr represent the expression obtained from combining these two.
            // (Note that in the case of for example a token representing a constant,
            // the parser object will not be used for the Expression object).
            tokens.MoveNext();
            Expression leftExpr = unaryNodeParselet.parseTogether(token, this);

            // We now have a parsed left operand expression to use in any coming
            // binary nodes requiring a left operand.

            // For as long as we encounter binary nodes with higher precedence than
            // our given precedence, we parse these (higher precedence corresponds to
            // having higher priority for parsing).
            while ((int)precedenceOfCurrentToken() > (int)precedence)
            {
                token = tokens.Current;

                // If tryGetBinaryNodeParselet returns true, the current token can be
                // parsed as an infix or postfix operation resulting in a binary node
                // (for postfix, a binary node with unused right expression).
                if (grammar.tryGetBinaryNodeParselet(token, out binaryNodeParselet))
                {
                    tokens.MoveNext();
                    leftExpr = binaryNodeParselet.parseTogether(leftExpr, token, this);
                }
                else
                {
                    throw new Exception("Failed to obtain InfixNodeParselet");
                }

            }

            return leftExpr;
        }

    }
}

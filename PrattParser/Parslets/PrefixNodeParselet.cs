using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrattParser
{
    /// <summary>
    /// The abstract base class for classes capable of generating an expression from a Token (an operation
    /// identifier, currently undergoing parsing) and a right Parser (representing the right operands, currently
    /// not parsed). In cases such as "!a", this results in unary nodes. In cases such as "if a then b else c",
    /// it results in ternary nodes.
    /// </summary>
    public abstract class PrefixNodeParselet
    {
        public abstract Expression parseTogether(Token middleToken, Parser rightParser);
    }

    public class NotUnaryNodeParselet : PrefixNodeParselet
    {
        public override Expression parseTogether(Token middleToken, Parser rightParser)
        {
            return new UnaryExpression(middleToken, rightParser.parse());
        }
    }

    public class ConstIntUnaryNodeParselet : PrefixNodeParselet
    {
        public override Expression parseTogether(Token middleToken, Parser rightParser)
        {
            return new NullaryExpression(middleToken);
        }
    }

    public class IfNodeParselet : PrefixNodeParselet
    {
        public override Expression parseTogether(Token middleToken, Parser rightParser)
        {
            // At this juncture, rightParser is positioned immediately after the "if" token
            // in the token stream. Parse until the "then" token is reached.
            Expression ifExpr = rightParser.parseWithPrecedence(Precedence.CONDITIONAL);

            // Read past the "then" token.
            rightParser.Tokens.MoveNext();

            Expression thenExpr = rightParser.parseWithPrecedence(Precedence.CONDITIONAL);

            // Read past the "else" token.
            rightParser.Tokens.MoveNext();

            Expression elseExpr = rightParser.parseWithPrecedence(Precedence.CONDITIONAL);

            return new TernaryExpression(middleToken, ifExpr, thenExpr, elseExpr);
        }
    }


}

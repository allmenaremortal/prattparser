using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PrattParser.Expressions;

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
            return new NotExpression(rightParser.parse());
        }
    }

    public class ConstIntUnaryNodeParselet : PrefixNodeParselet
    {
        public override Expression parseTogether(Token middleToken, Parser rightParser)
        {
            return new ConstIntExpression(Convert.ToInt32(middleToken.Text));
        }
    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrattParser.Expressions
{
    public class NotExpression : UnaryExpression
    {
        public NotExpression(Expression _expr)
            : base(_expr)
        {

        }

        public override T acceptVisitor<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.visitNotExpression(this);
        }
    }
}

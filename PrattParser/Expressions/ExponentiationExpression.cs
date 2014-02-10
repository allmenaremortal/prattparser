using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrattParser.Expressions
{
    public class ExponentiationExpression : BinaryExpression
    {

        public ExponentiationExpression(Expression _leftExpr, Expression _rightExpr)
            : base(_leftExpr, _rightExpr)
        {
        }

        public override T acceptVisitor<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.visitExponentiationExpression(this);
        }
    }
}

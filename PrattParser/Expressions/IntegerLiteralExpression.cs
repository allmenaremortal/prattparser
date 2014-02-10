using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrattParser.Expressions
{
    public class ConstIntExpression : NullaryExpression
    {
        private int value;

        public ConstIntExpression(int _value)
            : base()
        {
            value = _value;
        }

        public int Value
        {
            get
            {
                return value;
            }
        }

        public override T acceptVisitor<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.visitConstIntExpression(this);
        }
    }
}

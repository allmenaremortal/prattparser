﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrattParser.Expressions
{
    public class SubtractionExpression : BinaryExpression
    {
        public SubtractionExpression(Expression _leftExpr, Expression _rightExpr)
            : base(_leftExpr, _rightExpr)
        {
        }

        public override T acceptVisitor<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.visitSubtractionExpression(this);
        }
    }
}

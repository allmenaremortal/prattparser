using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrattParser
{

    /// <summary>
    /// Abstract expression class for defining expression trees.
    /// </summary>
    public abstract class Expression
    {
        public abstract T acceptVisitor<T>(IExpressionVisitor<T> visitor);
    }

    public abstract class NullaryExpression : Expression
    {
        public NullaryExpression()
        {
        }
    }

    public abstract class UnaryExpression : Expression
    {
        private Expression expr;

        public UnaryExpression(Expression _expr)
        {
            expr = _expr;
        }

        public Expression Expr
        {
            get { return expr; }
            set { expr = Expr; }
        }
    }

    public abstract class BinaryExpression : Expression
    {
        private Expression leftExpr;
        private Expression rightExpr;

        public BinaryExpression(Expression _leftExpr, Expression _rightExpr)
        {
            leftExpr = _leftExpr;
            rightExpr = _rightExpr;
        }

        public Expression LeftExpr
        {
            get { return leftExpr; }
            set { leftExpr = LeftExpr; }
        }

        public Expression RightExpr
        {
            get { return rightExpr; }
            set { rightExpr = RightExpr; }
        }
    }

    public abstract class TernaryExpression : Expression
    {
        private Expression leftExpr;
        private Expression middleExpr;
        private Expression rightExpr;

        public TernaryExpression(Expression _leftExpr, Expression _middleExpr, Expression _rightExpr)
        {
            leftExpr = _leftExpr;
            middleExpr = _middleExpr;
            rightExpr = _rightExpr;
        }

        public Expression LeftExpr
        {
            get { return leftExpr; }
            set { leftExpr = LeftExpr; }
        }

        public Expression MiddleExpr
        {
            get { return middleExpr; }
            set { middleExpr = MiddleExpr; }
        }

        public Expression RightExpr
        {
            get { return rightExpr; }
            set { rightExpr = RightExpr; }
        }
    }

}

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
        public abstract void acceptVisitor(IExpressionVisitor visitor);
    }

    /// <summary>
    /// A TernaryExpression contains a Token with information about the ternary node, as well as
    /// three expressions describing the operands for the operation described by the Token object.
    /// </summary>
    public class TernaryExpression : Expression
    {
        Token ternaryOperatorToken;
        Expression leftExpr;
        Expression middleExpr;
        Expression rightExpr;

        public TernaryExpression(Token _ternaryOperatorToken, Expression _leftExpr, Expression _middleExpr, Expression _rightExpr)
        {
            ternaryOperatorToken = _ternaryOperatorToken;
            leftExpr = _leftExpr;
            middleExpr = _middleExpr;
            rightExpr = _rightExpr;
        }

        public Token TernaryOperatorToken { get { return ternaryOperatorToken; } set { ternaryOperatorToken = TernaryOperatorToken; } }
        public Expression LeftExpr { get { return leftExpr; } set { leftExpr = LeftExpr; } }
        public Expression MiddleExpr { get { return middleExpr; } set { middleExpr = MiddleExpr; } }
        public Expression RightExpr { get { return rightExpr; } set { rightExpr = RightExpr; } }

        public override void acceptVisitor(IExpressionVisitor visitor)
        {
            visitor.writeTernaryExpression(this);
        }

    }

    /// <summary>
    /// A BinaryExpression contains a Token with information about the binary node, as well as
    /// two expressions describing the operands for the operation described by the Token object.
    /// </summary>
    public class BinaryExpression : Expression
    {
        Token binaryOperatorToken;
        Expression leftExpr;
        Expression rightExpr;

        public BinaryExpression(Expression _leftExpr, Token _binaryOperatorToken, Expression _rightExpr)
        {
            leftExpr = _leftExpr;
            binaryOperatorToken = _binaryOperatorToken;
            rightExpr = _rightExpr;
        }

        public Token BinaryOperatorToken { get { return binaryOperatorToken; } set { binaryOperatorToken = BinaryOperatorToken; } }
        public Expression LeftExpr { get { return leftExpr; } set { leftExpr = LeftExpr; } }
        public Expression RightExpr { get { return rightExpr; } set { rightExpr = RightExpr; } }

        public override void acceptVisitor(IExpressionVisitor visitor)
        {
            visitor.writeBinaryExpression(this);
        }
    
    }

    /// <summary>
    /// A UnaryExpression contains a Token with information about the unary node, as well as
    /// a single expression describing the operand for the operation described by the Token object.
    /// </summary>
    public class UnaryExpression : Expression
    {
        Token unaryOperatorToken;
        Expression operand;

        public UnaryExpression(Token _unaryOperatorToken, Expression _operandToken)
        {
            unaryOperatorToken = _unaryOperatorToken;
            operand = _operandToken;
        }

        public Token UnaryOperatorToken { get { return unaryOperatorToken; } set { unaryOperatorToken = UnaryOperatorToken; } }
        public Expression Operand { get { return operand; } set { operand = Operand; } }

        public override void acceptVisitor(IExpressionVisitor visitor)
        {
            visitor.writeUnaryExpression(this);
        }

    }

    /// <summary>
    /// A NullaryExpression contains a single Token and nothing else. These Expressions corresponds
    /// to leafs of the expression tree.
    /// </summary>
    public class NullaryExpression : Expression
    {
        Token token;

        public NullaryExpression(Token _token)
        {
            token = _token;
        }

        public Token Token { get { return token; } set { token = Token; } }

        public override void acceptVisitor(IExpressionVisitor visitor)
        {
            visitor.writeNullaryExpression(this);
        }

    }


}

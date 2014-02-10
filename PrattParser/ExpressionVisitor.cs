using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace PrattParser
{
    /// <summary>
    /// Common interface for Expression class visitors.
    /// </summary>
    public interface IExpressionVisitor
    {
        void writeTernaryExpression(TernaryExpression expr);
        void writeBinaryExpression(BinaryExpression expr);
        void writeUnaryExpression(UnaryExpression expr);
        void writeNullaryExpression(NullaryExpression expr);
    }

    /// <summary>
    /// IExpressionVisitor for printing an expression as a tree to the debug output window.
    /// </summary>
    public class ExpressionPrinter : IExpressionVisitor
    {
        private string treeString;
        private int indentationLevel = 0;

        private const int indentationSize = 2;

        /// <summary>
        /// Print the expression expr to the debug output window.
        /// </summary>
        public void writeExpression(Expression expr)
        {
            treeString = "";

            // Visit the expression. This will generate visits to all subsequent expression
            // nodes owned by the expression and will gradually fill out the treeString.
            expr.acceptVisitor(this);

            // Write the result to the debug output window.
            Debug.Write(treeString);

        }

        public void writeTernaryExpression(TernaryExpression expr)
        {
            treeString = treeString + System.Environment.NewLine +
                          new string(' ', indentationLevel * indentationSize) +
                          string.Format("Ternary node, token \"{0}\" of token type {1}", expr.TernaryOperatorToken.Text, expr.TernaryOperatorToken.TypeOfToken);

            indentationLevel++;
            expr.LeftExpr.acceptVisitor(this);
            expr.MiddleExpr.acceptVisitor(this);
            expr.RightExpr.acceptVisitor(this);
            indentationLevel--;
        }

        public void writeBinaryExpression(BinaryExpression expr)
        {
            treeString = treeString + System.Environment.NewLine +
                          new string(' ', indentationLevel * indentationSize) +
                          string.Format("Binary node, token \"{0}\" of token type {1}", expr.BinaryOperatorToken.Text, expr.BinaryOperatorToken.TypeOfToken);

            indentationLevel++;
            expr.LeftExpr.acceptVisitor(this);
            expr.RightExpr.acceptVisitor(this);
            indentationLevel--;
        }

        public void writeUnaryExpression(UnaryExpression expr)
        {
            treeString = treeString + System.Environment.NewLine +
                          new string(' ', indentationLevel * indentationSize) +
                          string.Format("Unary node, token \"{0}\" of token type {1}", expr.UnaryOperatorToken.Text, expr.UnaryOperatorToken.TypeOfToken);

            indentationLevel++;
            expr.Operand.acceptVisitor(this);
            indentationLevel--;
        }

        public void writeNullaryExpression(NullaryExpression expr)
        {
            treeString = treeString + System.Environment.NewLine +
                         new string(' ', indentationLevel * indentationSize) +
                         string.Format("Nullary node, token \"{0}\" of token type {1}", expr.Token.Text, expr.Token.TypeOfToken);
        }
    }
}

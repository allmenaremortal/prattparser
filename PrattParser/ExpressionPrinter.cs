using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using PrattParser.Expressions;

namespace PrattParser
{

    /// <summary>
    /// IExpressionVisitor for printing an expression as a tree to the debug output window.
    /// </summary>
    public class ExpressionPrinter : IExpressionVisitor<object>
    {
        private string treeString;
        private int indentationLevel = 0;

        private const int indentationSize = 2;

        private void addNewlineAndIndentation()
        {
            treeString = treeString + System.Environment.NewLine + new string(' ', indentationLevel*indentationSize);
        }

        /// <summary>
        /// Print the expression expr to the debug output window.
        /// </summary>
        public object visitExpression(Expression expr)
        {
            treeString = "";

            // Visit the expression. This will generate visits to all subsequent expression
            // nodes owned by the expression and will gradually fill out the treeString.
            expr.acceptVisitor(this);

            // Write the result to the debug output window.
            Debug.Write(treeString);

            return null;
        }

        public object visitNotExpression(NotExpression expr)
        {
            addNewlineAndIndentation();
            treeString = treeString + string.Format("Not node");

            indentationLevel++;
            expr.Expr.acceptVisitor(this);
            indentationLevel--;

            return null;
        }



        public object visitAdditionExpression(AdditionExpression expr)
        {
            addNewlineAndIndentation();
            treeString = treeString + string.Format("Addition node");

            indentationLevel++;
            expr.LeftExpr.acceptVisitor(this);
            expr.RightExpr.acceptVisitor(this);
            indentationLevel--;

            return null;
        }

        public object visitSubtractionExpression(SubtractionExpression expr)
        {
            addNewlineAndIndentation();
            treeString = treeString + string.Format("Subtraction node");

            indentationLevel++;
            expr.LeftExpr.acceptVisitor(this);
            expr.RightExpr.acceptVisitor(this);
            indentationLevel--;

            return null;
        }

        public object visitMultiplicationExpression(MultiplicationExpression expr)
        {
            addNewlineAndIndentation();
            treeString = treeString + string.Format("Multiplication node");

            indentationLevel++;
            expr.LeftExpr.acceptVisitor(this);
            expr.RightExpr.acceptVisitor(this);
            indentationLevel--;

            return null;
        }

        public object visitDivisionExpression(DivisionExpression expr)
        {
            addNewlineAndIndentation();
            treeString = treeString + string.Format("Division node");

            indentationLevel++;
            expr.LeftExpr.acceptVisitor(this);
            expr.RightExpr.acceptVisitor(this);
            indentationLevel--;

            return null;
        }

        public object visitExponentiationExpression(ExponentiationExpression expr)
        {
            addNewlineAndIndentation();
            treeString = treeString + string.Format("Exponentiation node");

            indentationLevel++;
            expr.LeftExpr.acceptVisitor(this);
            expr.RightExpr.acceptVisitor(this);
            indentationLevel--;

            return null;
        }

        public object visitConstIntExpression(ConstIntExpression expr)
        {
            addNewlineAndIndentation();
            treeString = treeString + string.Format("Integer literal expression with value {0}", expr.Value);

            return null;
        }


    }
}

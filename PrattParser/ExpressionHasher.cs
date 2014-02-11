using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using PrattParser.Expressions;

namespace PrattParser
{

    /// <summary>
    /// IExpressionVisitor for creating an int hash value from an expression tree.
    /// </summary>
    public class ExpressionHasher : IExpressionVisitor<int>
    {

        // Hash function type declarations.
        private delegate int UnaryExpressionHasher(int singleHash);
        private delegate int BinaryExpressionHasher(int leftHash, int rightHash);

        // Static hash functions for each expression class.
        private static UnaryExpressionHasher notHasher = (singleHash) => 15 +  31 * singleHash;
        private static BinaryExpressionHasher additionHasher = (leftHash, rightHash) => 7 + 2 * leftHash - 3 * rightHash;
        private static BinaryExpressionHasher subtractionHasher = (leftHash, rightHash) => 15 + 5 * leftHash - 7 * rightHash;
        private static BinaryExpressionHasher multiplicationHasher = (leftHash, rightHash) => 6 + 11 * leftHash - 13 * rightHash;
        private static BinaryExpressionHasher divisionHasher = (leftHash, rightHash) => 8 + 17 * leftHash - 19 * rightHash;
        private static BinaryExpressionHasher exponentiationHasher = (leftHash, rightHash) => 16 + 23 * leftHash - 29 * rightHash;

        /// <summary>
        /// Generate the hash value.
        /// </summary>
        public int visitExpression(Expression expr)
        {
            return expr.acceptVisitor(this);
        }

        public int visitNotExpression(NotExpression expr)
        {
            int singleHashValue = expr.Expr.acceptVisitor(this);
            return notHasher(singleHashValue);
        }

        public int visitAdditionExpression(AdditionExpression expr)
        {
            int leftHashValue = expr.LeftExpr.acceptVisitor(this);
            int rightHashValue = expr.RightExpr.acceptVisitor(this);
            return additionHasher(leftHashValue, rightHashValue);
        }

        public int visitSubtractionExpression(SubtractionExpression expr)
        {
            int leftHashValue = expr.LeftExpr.acceptVisitor(this);
            int rightHashValue = expr.RightExpr.acceptVisitor(this);
            return subtractionHasher(leftHashValue, rightHashValue);
        }

        public int visitMultiplicationExpression(MultiplicationExpression expr)
        {
            int leftHashValue = expr.LeftExpr.acceptVisitor(this);
            int rightHashValue = expr.RightExpr.acceptVisitor(this);
            return multiplicationHasher(leftHashValue, rightHashValue);
        }

        public int visitDivisionExpression(DivisionExpression expr)
        {
            int leftHashValue = expr.LeftExpr.acceptVisitor(this);
            int rightHashValue = expr.RightExpr.acceptVisitor(this);
            return divisionHasher(leftHashValue, rightHashValue);
        }

        public int visitExponentiationExpression(ExponentiationExpression expr)
        {
            int leftHashValue = expr.LeftExpr.acceptVisitor(this);
            int rightHashValue = expr.RightExpr.acceptVisitor(this);
            return exponentiationHasher(leftHashValue, rightHashValue);
        }

        public int visitConstIntExpression(ConstIntExpression expr)
        {
            return expr.Value;
        }


    }
}

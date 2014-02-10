using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrattParser
{
    /// <summary>
    /// The abstract base class for classes capable of generating an expression from an Expression
    /// (representing a left operand, already parsed), a Token (a middle operation identifier, currently
    /// undergoing parsing) and a right Parser (representing the right operand, currently not parsed).
    /// </summary>
    public abstract class InfixNodeParselet
    {
        // As two binary nodes can be juxtaposed to the same token in a token stream, they require
        // precedence conventions in order to resolve ownership of the token.
        protected int nodePrecedence;

        // Boolean value determining whether the operator is left associative or not. For example,
        // "-" is left-associative, in that a - b - c  = (a - b) - c.
        protected bool isLeftAssociative;

        // Combine the left expression, middle token and right parser into one expression.
        public Expression parseTogether(Expression leftExpr, Token middleToken, Parser rightParser)
        {
            int precedence = NodePrecedence + (isLeftAssociative ? 1 : 0);
            return new BinaryExpression(leftExpr, middleToken, rightParser.parseWithPrecedence(precedence));
        }

        public int NodePrecedence { get { return nodePrecedence;} }

        public bool IsLeftAssociative { get { return isLeftAssociative; } }
    }

    public class PlusBinaryNodeParselet : InfixNodeParselet
    {
        public PlusBinaryNodeParselet()
        {
            nodePrecedence = Precedence.SUM;
            isLeftAssociative = true;
        }
    }

    public class MinusBinaryNodeParselet : InfixNodeParselet
    {
        public MinusBinaryNodeParselet()
        {
            nodePrecedence = Precedence.SUM;
            isLeftAssociative = true;
        }
    }

    public class MultiplicationBinaryNodeParselet : InfixNodeParselet
    {
        public MultiplicationBinaryNodeParselet()
        {
            nodePrecedence = Precedence.PRODUCT;
            isLeftAssociative = true;
        }
    }

    public class DivisionBinaryNodeParselet : InfixNodeParselet
    {
        public DivisionBinaryNodeParselet()
        {
            nodePrecedence = Precedence.PRODUCT;
            isLeftAssociative = true;
        }
    }

    public class ExponentiationBinaryNodeParselet : InfixNodeParselet
    {
        public ExponentiationBinaryNodeParselet()
        {
            nodePrecedence = Precedence.EXPONENT;
            isLeftAssociative = false;
        }
    }

}

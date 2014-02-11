using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PrattParser.Expressions;

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

        // Combine the left expression and right parser into one expression.
        public abstract Expression parseTogether(Expression leftExpr, Parser rightParser);

        // The node precedence determines how far the right expression of rightParser is parsed.
        public int NodePrecedence
        {
            get { return nodePrecedence; }
        }

        // Left associativity determines the precedence passed to the rightParser, in order to
        // determine whether next infix parselets of same precedence are given higher or
        // lower precedence than the current parselet.
        public bool IsLeftAssociative
        {
            get { return isLeftAssociative; }
        }

        // This predecende is the one taking into account both the precedence of the infix
        // node in the operation precedence hierarchy, as well as the correction for
        // associativity.
        public int AssociativityCorrectedPrecedence
        {
            // When parsing left associative operators, we parse the right-hand expression
            // requiring high precedence, to ensure that the left-hand side takes priority.
            get { return NodePrecedence + (isLeftAssociative ? 0 : 1); }
        }
    }

    public class PlusBinaryNodeParselet : InfixNodeParselet
        {
            public PlusBinaryNodeParselet()
            {
                nodePrecedence = Precedence.SUM;
                isLeftAssociative = true;
            }

            public override Expression parseTogether(Expression leftExpr, Parser rightParser)
            {
                return new AdditionExpression(leftExpr,
                    rightParser.parseWithPrecedence(AssociativityCorrectedPrecedence));
            }
        }

        public class MinusBinaryNodeParselet : InfixNodeParselet
        {
            public MinusBinaryNodeParselet()
            {
                nodePrecedence = Precedence.SUM;
                isLeftAssociative = true;
            }

            public override Expression parseTogether(Expression leftExpr, Parser rightParser)
            {
                return new SubtractionExpression(leftExpr,
                    rightParser.parseWithPrecedence(AssociativityCorrectedPrecedence));
            }
        }

        public class MultiplicationBinaryNodeParselet : InfixNodeParselet
        {
            public MultiplicationBinaryNodeParselet()
            {
                nodePrecedence = Precedence.PRODUCT;
                isLeftAssociative = true;
            }

            public override Expression parseTogether(Expression leftExpr, Parser rightParser)
            {
                return new MultiplicationExpression(leftExpr,
                    rightParser.parseWithPrecedence(AssociativityCorrectedPrecedence));
            }
        }

        public class DivisionBinaryNodeParselet : InfixNodeParselet
        {
            public DivisionBinaryNodeParselet()
            {
                nodePrecedence = Precedence.PRODUCT;
                isLeftAssociative = true;
            }

            public override Expression parseTogether(Expression leftExpr, Parser rightParser)
            {
                return new DivisionExpression(leftExpr,
                    rightParser.parseWithPrecedence(AssociativityCorrectedPrecedence));
            }
        }

        public class ExponentiationBinaryNodeParselet : InfixNodeParselet
        {
            public ExponentiationBinaryNodeParselet()
            {
                nodePrecedence = Precedence.EXPONENT;
                isLeftAssociative = false;
            }

            public override Expression parseTogether(Expression leftExpr, Parser rightParser)
            {
                return new ExponentiationExpression(leftExpr,
                    rightParser.parseWithPrecedence(AssociativityCorrectedPrecedence));
            }

        }

    }

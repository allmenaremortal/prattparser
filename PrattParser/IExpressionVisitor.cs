using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using PrattParser.Expressions;

namespace PrattParser
{
    /// <summary>
    /// Common interface for Expression class visitors.
    /// </summary>
    public interface IExpressionVisitor<T>
    {
        T visitExpression(Expression expr);
        T visitAdditionExpression(AdditionExpression expr);
        T visitSubtractionExpression(SubtractionExpression expr);
        T visitMultiplicationExpression(MultiplicationExpression expr);
        T visitDivisionExpression(DivisionExpression expr);
        T visitExponentiationExpression(ExponentiationExpression expr);
        T visitNotExpression(NotExpression expr);
        T visitConstIntExpression(ConstIntExpression expr);
    }
}
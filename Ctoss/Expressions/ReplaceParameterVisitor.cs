using System.Linq.Expressions;

namespace Ctoss.Expressions;


internal class ReplaceParameterVisitor : ExpressionVisitor
{
    private readonly ParameterExpression _oldParameter;
    private readonly ParameterExpression _newParameter;

    public ReplaceParameterVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        _oldParameter = oldParameter;
        _newParameter = newParameter;
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        return node == _oldParameter ? _newParameter : base.VisitParameter(node);
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        // Visit the left and right sides of the binary expression
        var left = Visit(node.Left);
        var right = Visit(node.Right);

        // Perform the binary operation
        return node.NodeType switch
        {
            ExpressionType.Add => Expression.Add(left, right),
            ExpressionType.Subtract => Expression.Subtract(left, right),
            ExpressionType.Multiply => Expression.Multiply(left, right),
            ExpressionType.Divide => Expression.Divide(left, right),
            _ => base.VisitBinary(node)
        };
    }
}

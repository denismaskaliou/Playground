using System.Linq.Expressions;
using MongoDB.Tests.Scripts;

namespace MongoDB.Tests.Infrastructure.Models;

public class UnwindProducts
{
    public Product Products { get; set; } = default!;
}

public static class UnwindProgramsExtensions
{
    public static Expression<Func<UnwindProducts, bool>> ToUnwindProductsFilter(this Expression<Func<Product, bool>> source)
    {
        ArgumentNullException.ThrowIfNull(source);
        
        var parameter = Expression.Parameter(typeof(UnwindProducts), "up");
        var productProperty = Expression.Property(parameter, nameof(UnwindProducts.Products));
        var body = new ParameterReplacer(source.Parameters[0], productProperty).Visit(source.Body);
        
        return Expression.Lambda<Func<UnwindProducts, bool>>(body, parameter);
    }

    private class ParameterReplacer(ParameterExpression oldNode, Expression expression) : ExpressionVisitor
    {
        protected override Expression VisitParameter(ParameterExpression newNode) =>
            newNode == oldNode
                ? expression
                : base.VisitParameter(newNode);
    }
}
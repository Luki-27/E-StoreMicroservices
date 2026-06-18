
using Catalog.API.Exceptions;

namespace Catalog.API.Products.GetProductById;

public record GetProductByIdQuery(Guid id): IQuery<GetProductByIdResult>;
public record GetProductByIdResult(Product Product);

public class GetProductByIdQueryHandler(IDocumentSession session)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var result = await session.LoadAsync<Product>(query.id,cancellationToken);

        if(result is null)
        {
            throw new ProductNotFoundException(query.id);
        }

        return new GetProductByIdResult(result);
    }
}

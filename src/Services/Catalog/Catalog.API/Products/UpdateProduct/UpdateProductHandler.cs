
using Catalog.API.Exceptions;
using FluentValidation;

namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id, string Name, string Description, decimal Price, List<string> Categories, string ImageFile) :
    ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);


public class UpdateProductCommandValidator:AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(4, 100).WithMessage("Name shoud be withn length 4, 100");
        RuleFor(x => x.Price)
            .GreaterThan(10).WithMessage("Price must be greater than 10");
    }
}

public class UpdateProductCommandHandler(IDocumentSession session)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var prod = await session.LoadAsync<Product>(command.Id,cancellationToken);
        if (prod == null)
        {
            throw new ProductNotFoundException(command.Id);
        }

        prod.Name = command.Name;
        prod.Description = command.Description;
        prod.Price = command.Price;
        prod.Categories = command.Categories;
        prod.ImageFile = command.ImageFile;

        session.Update(prod);
        await session.SaveChangesAsync();

        return new UpdateProductResult(true);
    }
}

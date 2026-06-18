using FluentValidation;

namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand (string Name, string Description, decimal Price, List<string> Categories, string ImageFile):
    ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);


public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is requried");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(x => x.Categories).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
        RuleFor(x => x.Price).GreaterThan(10).WithMessage("Price should be greater than 10");
    }
}


public class CreateProductCommandHandler(IDocumentSession session)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = command.Name,
            Description = command.Description,
            Price = command.Price,
            Categories = command.Categories,
            ImageFile = command.ImageFile

        };

        //save to DB    
        session.Store(product);
        await session.SaveChangesAsync();
        return new CreateProductResult(product.Id);


    }
}

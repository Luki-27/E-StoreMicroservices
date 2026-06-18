using Marten.Schema;

namespace Catalog.API.Data;

public class InitialCatalogData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        using var session = store.LightweightSession();

        if (await session.Query<Product>().AnyAsync())
            return;

        session.Store<Product>(GetPreConfiguredProducts());
            
        await session.SaveChangesAsync();
    }

    private static IEnumerable<Product> GetPreConfiguredProducts() => new List<Product>
    {

        new Product()
        {
            Id= Guid.NewGuid(),
            Name = "Apple",
            Description = "No ",
            Categories = new List<string>{"Fruit"},
            Price = 20.5M,
            ImageFile = "prd1.png"
        },
        new Product()
        {
            Id= Guid.NewGuid(),
            Name = "Smart Watch",
            Description = "Its a watch ",
            Categories = new List<string>{"Accessories"},
            Price = 3000.5M,
            ImageFile = "prd2.png"
        },
        new Product()
        {
            Id= Guid.NewGuid(),
            Name = "IPhone 1",
            Description = "Here is the trend",
            Categories = new List<string>{"Phone"},
            Price = 20000.5M,
            ImageFile = "prd3.png"
        },
        new Product()
        {
            Id= Guid.NewGuid(),
            Name = "Gold Cup",
            Description = "FIFA World Cup",
            Categories = new List<string>{"Entertainment"},
            Price = 20000000.5M,
            ImageFile = "prd4.png"
        },

    };
}

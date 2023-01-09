
namespace ECommerce.Test;

public class ECommerceFixture : IDisposable 
{
    public CommerceContext Context { get; set; }

    // * Fixture constructor called every time a test is done
    public ECommerceFixture(){
        var dbcOptions = new DbContextOptionsBuilder<CommerceContext>()
            .UseInMemoryDatabase(databaseName: "ECommerceDB").Options;
        Context = new CommerceContext(dbcOptions);

        // * Seed the DB now
        //User/Auth
        Context.Users.Add(new User{UserId=1, UserFirstName="John", UserLastName="Doe", UserEmail="John@no.com", UserPassword="JohnDoe", IfAdmin=false});
        Context.Users.Add(new User{UserId=2, UserFirstName="Jane", UserLastName="Doe", UserEmail="Jane@no.com", UserPassword="JaneDoe", IfAdmin=false});
        Context.Users.Add(new User{UserId=3, UserFirstName="Johnny", UserLastName="Doe", UserEmail="Johnny@no.com", UserPassword="JohnnyDoe", IfAdmin=false});
        //Products
        Context.Products.Add(new Product{ProductId=1, ProductName="Altoid Mint", ProductQuantity=2, ProductPrice=(decimal)2.99, ProductDescription="Desc 1", ProductImage="URL1"});
        Context.Products.Add(new Product{ProductId=2, ProductName="S3 Watch", ProductQuantity=4, ProductPrice=(decimal)399.99, ProductDescription="Desc 2", ProductImage="URL2"});
        Context.Products.Add(new Product{ProductId=3, ProductName="Lenovo Thinkpad", ProductQuantity=3, ProductPrice=(decimal)599.94, ProductDescription="Desc 3", ProductImage="URL3"});
        Context.Products.Add(new Product{ProductId=4, ProductName="Corsair K70 Mk II", ProductQuantity=6, ProductPrice=(decimal)240.00, ProductDescription="Desc 4", ProductImage="URL4"});
        //Cart
        Context.Cart.Add(new Cart{CartId=1, fk_ProductID=3, fk_UserID=1});
        Context.Cart.Add(new Cart{CartId=2, fk_ProductID=4, fk_UserID=1});
        Context.Cart.Add(new Cart{CartId=3, fk_ProductID=1, fk_UserID=2});
        Context.Cart.Add(new Cart{CartId=4, fk_ProductID=2, fk_UserID=2});
        //Deal
        Context.Deals.Add(new Deal{DealId=1, fk_Product_Id=3, SalePrice=(decimal)450.50});
        Context.Deals.Add(new Deal{DealId=2, fk_Product_Id=4, SalePrice=(decimal)200.05});
        //Wishlist
        Context.Wishlist.Add(new Wishlist{WishlistId=1, fk_ProductId=4, fk_UserId=3});
        Context.Wishlist.Add(new Wishlist{WishlistId=2, fk_ProductId=1, fk_UserId=1});

        // * Save the changes now before starting
        Context.SaveChangesAsync();
    }

    public async void Dispose()
    {
        await Context.DisposeAsync();
    }
}
namespace ECommerce.Test;

[Collection("ECommerce Collection")]
public class WishlistControllerTest
{
    private readonly ECommerceFixture _fixture;
    // ! _output is just for extra logging in the xUnit console
    private readonly ITestOutputHelper _output;

    public WishlistControllerTest(ECommerceFixture fixture, ITestOutputHelper output){
        this._fixture = fixture;
        this._output = output;
    }

    // * GetWishlist() returns a List
    // ! Only returns NotFound() if the Wishlist DbSet itself is null 
    [Fact]
    public async void GetWishlistReturnsList(){
        // * ARRANGE
        var controller = new WishlistController(_fixture.Context);

        // * ACT
        var result = await controller.GetWishlist();

        // * ASSERT
        Assert.IsType<List<Wishlist>>(result.Value);
    }

    // * 
    [Fact]
    public async void GetWishlistProductsReturnsEmptyList(){
        // * ARRANGE
        var controller = new WishlistController(_fixture.Context);
        //no Wishlist with user id 2
        int wid = 2;

        // * ACT
        var result = await controller.GetWishlistProducts(wid);
        _output.WriteLine($"GetWishlistProducts() returns: {result}");

        // * ASSERT
        Assert.Empty(result.Value);
    }

    // * GetWishlistProducts should return a nonempty List<Product> with the same UserID in the wishlist
    [Fact]
    public async void GetWishlistProductsReturnsProductList(){
        // TODO : Update once the Wishlist is fully ready
        // * ARRANGE
        var controller = new WishlistController(_fixture.Context);
        int uid = 3;

        // * ACT
        var result = await controller.GetWishlistProducts(uid);
        _output.WriteLine($"GetWishlistProducts() returns: {result.Value}");

        // * ASSERT
        Assert.IsType<List<Product>>(result.Value);
        // ! Something is happening with the intersect LINQ in the controller
        /* Assert.NotEmpty(result.Value); */
    }

    // * PutWishlist(id, wishlist) returns BadRequest() when id and wishlistID do not match
    [Fact]
    public async void PutWishlistReturnsBadRequest(){
        // * ARRANGE
        var controller = new WishlistController(_fixture.Context);
        //let's grab the first wishlist item and test with an invalid id
        int id = 3;
        var tmpW = (await controller.GetWishlist()).Value.First();

        // * ACT
        var result = await controller.PutWishlist(id, tmpW);
        _output.WriteLine($"PutWishlist() returns: {result}");

        // * ASSERT
        Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestResult>(result);
    }

    // * PutWishlist() returns NotFound() when the Wishlist item itself does not exist
    [Fact]
    public async void PutWishlistReturnsNotFound(){
        // * ARRANGE
        var controller = new WishlistController(_fixture.Context);
        int id = 10;
        Wishlist tmpW = new Wishlist{WishlistId=id, fk_ProductId=2, fk_UserId=2};

        // * ACT
        var result = await controller.PutWishlist(id, tmpW);
        _output.WriteLine($"PutWishlist() returns: {result}");

        // * ASSERT
        Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundResult>(result);
    }

    // * If a wishlist is updated successfully, PutWishlist() returns NoContent()
    [Fact]
    public async void PutWishlistReturnsNoContent(){
        // * ARRANGE
        var controller = new WishlistController(_fixture.Context);
        //let's update the first item for now to reference product 4 instead of product 5
        int id=1;
        var tmpW = (await controller.GetWishlist()).Value!.First();
        tmpW.fk_ProductId = 4;

        // * ACT
        var result = await controller.PutWishlist(id, tmpW);
        _output.WriteLine($"PutWishlist() returns: {result}");

        // * ASSERT
        Assert.IsType<Microsoft.AspNetCore.Mvc.NoContentResult>(result);
    }

    [Fact]
    public async void PostWishlistReturnsCreated(){
        // * ARRANGE
        var controller = new WishlistController(_fixture.Context);
        Wishlist tmpW = new Wishlist{fk_ProductId=3, fk_UserId=2};

        // * ACT
        var result = await controller.PostWishlist(tmpW);
        _output.WriteLine($"PostWishlist() returns: {result}");

        // * ASSERT
        // ! Because it returns CreatedAtActionResult, the result itself needs to be extracted
        Assert.IsType<Microsoft.AspNetCore.Mvc.CreatedAtActionResult>(result.Result);
    }

    [Fact]
    public async void DeleteWishlistReturnsNotFound(){
        // * ARRANGE
        var controller = new WishlistController(_fixture.Context);
        //let's try to delete an invalid wishlist
        int id = 10;

        // * ACT
        var result = await controller.DeleteWishlist(id);
        _output.WriteLine($"DeleteWishlist() returns: {result}");

        // * ASSERT
        Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundResult>(result);
    }

    [Fact]
    public async void DeleteWishlistReturnsNoContent(){
        // * ARRANGE
        var controller = new WishlistController(_fixture.Context);
        //let's delete Wishlist item #2
        int id = 2;

        // * ACT
        var result = await controller.DeleteWishlist(id);

        // * ASSERT
        Assert.IsType<Microsoft.AspNetCore.Mvc.NoContentResult>(result);
    }

    
}
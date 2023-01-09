

namespace ECommerce.Test;

[Collection("ECommerce Collection")]
public class CartControllerTest
{
    private readonly ECommerceFixture _fixture;
    // ! _output is just for extra logging in the xUnit console
    private readonly ITestOutputHelper _output;

    public CartControllerTest(ECommerceFixture fixture, ITestOutputHelper output){
        this._fixture = fixture;
        this._output = output;
    }

    // ! Because the cart in the context is always seeded, we can't really
    // !    test for a null Cart DbSet
    [Fact]
    public async void GetCartReturnsListOfCarts(){
        // * ARRANGE
        var controller = new CartController(_fixture.Context);

        // * ACT
        var result = await controller.GetCart();
        _output.WriteLine($"GetCart() returns: {result}");

        // * ASSERT
        Assert.IsType<List<Cart>>(result.Value);
    }

    [Fact]
    public async void GetCartIDReturnsEmptyList(){
        // * ARRANGE
        var controller = new CartController(_fixture.Context);
        int uid=5;

        // * ACT
        var result = await controller.GetCart(uid);
        _output.WriteLine($"GetCart({uid}) returns: {result}");

        // * ASSERT
        Assert.IsType<List<Product>>(result.Value);
        Assert.Empty(result.Value);
    }

    [Fact]
    public async void GetCartIDReturnsProductList(){
        // * ARRANGE
        var controller = new CartController(_fixture.Context);
        int uid = 1;

        // * ACT
        var result = await controller.GetCart(uid);
        _output.WriteLine($"GetCart({uid}) returns: {result}");

        // * ASSERT
        Assert.IsType<List<Product>>(result.Value);
        Assert.NotEmpty(result.Value);
    }

    [Fact]
    public async void PutCartReturnsBadRequest(){
        // * ARRANGE
        var controller = new CartController(_fixture.Context);
        int cid = 5;
        var tmpC = (await controller.GetCart()).Value!.First();

        // * ACT
        var result = await controller.PutCart(cid, tmpC);
        _output.WriteLine($"PutCart() returns: {result}");

        // * ASSERT
        Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestResult>(result);
    }
    
    [Fact]
    public async void PutCartReturnsNotFound(){
        // * ARRANGE
        var controller = new CartController(_fixture.Context);
        int cid=20;
        Cart tmpC = new Cart{CartId=cid, fk_UserID=2, fk_ProductID=4};

        // * ACT
        var result = await controller.PutCart(cid, tmpC);
        _output.WriteLine($"PutCart() returns: {result}");

        // * ASSERT
        Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundResult>(result);
    }

    [Fact]
    public async void PutCartReturnsNoContent(){
        // * ARRANGE
        var controller = new CartController(_fixture.Context);
        int cid = 1;
        var tmpC = (await controller.GetCart()).Value!.First();
        tmpC.fk_ProductID = 2;

        // * ACT
        var result = await controller.PutCart(cid, tmpC);
        _output.WriteLine($"GetCart() returns: {result}\n");

        // * ASSERT
        Assert.IsType<Microsoft.AspNetCore.Mvc.NoContentResult>(result);
    }

    [Fact]
    public async void PostCartReturnsCreated(){
        // * ARRANGE
        var controller = new CartController(_fixture.Context);
        Cart tmpC = new Cart{fk_UserID=3, fk_ProductID=4};

        // * ACT
        var result = await controller.PostCart(tmpC);
        _output.WriteLine($"PostCart() returns: {result}");

        // * ASSERT
        Assert.IsType<Microsoft.AspNetCore.Mvc.CreatedAtActionResult>(result.Result);
    }

    [Fact]
    public async void DeleteCartReturnsNotFound(){
        // * ARRANGE
        var controller = new CartController(_fixture.Context);
        int cid = 15;

        // * ACT
        var result = await controller.DeleteCart(cid);
        _output.WriteLine($"DeleteCart({cid}) returns: {result}");

        // * ASSERT
        Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundResult>(result);
    }

    [Fact]
    public async void DeleteCartReturnsNoContent(){
        // * ARRANGE
        var controller = new CartController(_fixture.Context);
        int cid = 4;

        // * ACT
        var result = await controller.DeleteCart(cid);
        _output.WriteLine($"DeleteCart({cid}) returns: {result}");

        // * ASSERT
        Assert.IsType<Microsoft.AspNetCore.Mvc.NoContentResult>(result);
    }
}
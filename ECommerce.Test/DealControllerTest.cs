

namespace ECommerce.Test;

[Collection("ECommerce Collection")]
public class DealControllerTest
{
    private readonly ECommerceFixture _fixture;
    // ! _output is just for extra logging in the xUnit console
    private readonly ITestOutputHelper _output;

    public DealControllerTest(ECommerceFixture fixture, ITestOutputHelper output){
        this._fixture = fixture;
        this._output = output;
    }

    [Fact]
    public async void GetDealsReturnsList(){
        // * ARRANGE
        var controller = new DealController(_fixture.Context);

        // * ACT
        var result = await controller.GetDeals();

        // * ASSERT
        Assert.IsType<List<Deal>>(result.Value);
    }

    [Fact]
    public async void GetDealIDReturnsNotFound(){
        // * ARRANGE
        var controller = new DealController(_fixture.Context);
        // ! No deal with ID: 3
        int id = 3;

        // * ACT
        var result = await controller.GetDeal(id);

        // * ASSERT
        Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundResult>(result.Result);
    }

    // ! GetDeal does not produce a status code on success
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async void GetDealIDReturnsDeal(int id){
        // * ARRANGE
        var controller = new DealController(_fixture.Context);

        // * ACT
        var result = await controller.GetDeal(id);

        // * ASSERT
        Assert.IsType<Deal>(result.Value);
        Assert.NotNull(result.Value);
    }

    // * CreateDeal() produces BadRequest() when ProductID is invalid or SalePrice is negative
    [Theory]
    [InlineData(5, 150.0)]
    [InlineData(4, -50.0)]
    public async void CreateDealReturnsBadRequest(int pId, double sp){
        // * ARRANGE
        var controller = new DealController(_fixture.Context);
        Deal tmpD = new Deal{DealId=0, fk_Product_Id=pId, SalePrice=(decimal)sp};

        // * ACT
        var result = await controller.CreateDeal(tmpD);
        _output.WriteLine($"CreateDeal() Returned: {result}");

        // * ASSERT
        Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
    }

    // * CreateDeal returns Ok() if a Deal is successfully made
    [Fact]
    public async void CreateDealReturnsOk(){
        // * ARRANGE
        var controller = new DealController(_fixture.Context);
        Deal tmpD = new Deal{fk_Product_Id = 2, SalePrice=(decimal)250.0};

        // * ACT
        var result = await controller.CreateDeal(tmpD);
        _output.WriteLine($"CreateDeal() Returned: {result}");

        // * ASSERT
        Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result);
    }

    // * UpdateDeal() returns NotFound() if a deal is not in the Deals DbSet
    [Fact]
    public async void UpdateDealReturnsNotFound(){
        // * ARRANGE
        var controller = new DealController(_fixture.Context);
        Deal tmpD = new Deal{DealId=4, fk_Product_Id=1, SalePrice=(decimal)0.99};

        // * ACT
        var result = await controller.UpdateDeal(tmpD);
        _output.WriteLine($"UpdateDeal() returns: {result}");

        // * ASSERT
        Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundResult>(result);
    }

    // * UpdateDeal() returns NoContent() if a deal is successfully updated
    [Fact]
    public async void UpdateDealReturnsNoContent(){
        // * ARRANGE
        var controller = new DealController(_fixture.Context);
        //let's grab a deal from the Context
        var tmpD = (await controller.GetDeal(1)).Value;
        //Now that the Deal is tracked, we can attempt to update it
        tmpD.SalePrice=(decimal)300.50;

        // * ACT
        var result = await controller.UpdateDeal(tmpD);
        _output.WriteLine($"UpdateDeal() returns: {result}");

        // * ASSERT
        Assert.IsType<Microsoft.AspNetCore.Mvc.NoContentResult>(result);
    }

    // * DeleteDeal(id) returns NotFound() if no deal exists
    [Fact]
    public async void DeleteDealIDReturnsNotFound(){
        // * ARRANGE
        var controller = new DealController(_fixture.Context);
        //only 2 Deals in the Context, let's try deleting #5
        int id = 5;

        // * ACT
        var result = await controller.DeleteDeal(id);
        _output.WriteLine($"DeleteDeal() returns: {result}");

        // * ASSERT
        Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundResult>(result);
    }

    // * DeleteDeal(id) returns NoContent() if a deal is deleted
    [Fact]
    public async void DeleteDealIDReturnsNoContent(){
        // * ARRANGE
        var controller = new DealController(_fixture.Context);
        int id = 1;

        // * ACT
        var result = await controller.DeleteDeal(id);
        _output.WriteLine($"DeleteDeal(id) returns: {result}");

        // * ASSERT
        Assert.IsType<Microsoft.AspNetCore.Mvc.NoContentResult>(result);
    }

    // * DeleteDeals() returns NoContent() if all deals are cleared
    [Fact]
    public async void DeleteDealsReturnsNoContent(){
        // * ARRANGE
        var controller = new DealController(_fixture.Context);

        // * ACT
        var result = await controller.DeleteDeals();
        _output.WriteLine($"DeleteDeals() returns: {result}");

        // * ASSERT
        Assert.IsType<Microsoft.AspNetCore.Mvc.NoContentResult>(result);
    }

    // * DeleteDeals() returns BadRequest(obj message) if there are no Deals to delete
    [Fact]
    public async void DeleteDealsReturnsBadRequest(){
        // * ARRANGE
        var controller = new DealController(_fixture.Context);
        //first let's clear the deals out
        await controller.DeleteDeals();

        // * ACT
        var result = await controller.DeleteDeals();
        _output.WriteLine($"DeleteDeals() returns: {result}");

        // * ASSERT
        Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
    }
}
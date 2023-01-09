

using ECommerceAPI.Controllers;

namespace ECommerce.Test;

[Collection("ECommerce Collection")]
public class AuthControllerTest
{
    private readonly ECommerceFixture _fixture;
    // ! _output is just for extra logging in the xUnit console
    private readonly ITestOutputHelper _output;

    public AuthControllerTest(ECommerceFixture fixture, ITestOutputHelper output){
        this._fixture = fixture;
        this._output = output;
    }

    // * Register should return a 400 when email is not unique
    // ! Looks like constraint checks don't necessarily work with an in-memory DB
    // ? Might look into using a local SQLite db for testing to actually test constraints too 

    [Fact]
    public async void RegisterReturnsOkAndUser(){
        // * ARRANGE
        _output.WriteLine("Arranging Register() test...");
        var controller = new AuthController(_fixture.Context);
        User tmpU = new User{UserId=0, UserFirstName="Temp", UserLastName="Temp", UserEmail="temp@no.com", UserPassword="TempPW", IfAdmin=false};

        // * ACT
        var result = await controller.Register(tmpU);
        _output.WriteLine($"Registering User returned: {result}");

        // * ASSERT
        Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result);
    }

    // * Login returns a null User and a BadRequestObjectResult on invalid inputs
    // ! User should be extracted from the ActionResult.Value
    // ! Status extracted from ActionResult.Result
    [Theory]
    [InlineData("John@no.com", "badpass")]
    [InlineData("John@mo.com", "JohnDoe")]
    public async void LoginReturnsBadRequest(string email, string pw){
        // * ARRANGE
        _output.WriteLine("Arranging Bad Login() test...");
        var controller = new AuthController(_fixture.Context);
        //just need a mostly-blank user with an email and password
        User tmpU = new User{UserEmail=email, UserPassword=pw};

        // * ACT
        var result = await controller.Login(tmpU);
        _output.WriteLine($"Logging in this user returned: {result.Result}\nUser: {result.Value}");

        // * ASSERT
        Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result.Result);
        Assert.Null(result.Value);
    }

    // * Login should return a User object on valid inputs
    // ! Login does not produce a status code on success, we could check for that later
    [Fact]
    public async void LoginReturnsUser(){
        // * ARRANGE
        var controller = new AuthController(_fixture.Context);
        User tmpU = new User{UserEmail="John@no.com", UserPassword="JohnDoe"};

        // * ACT
        var result = await controller.Login(tmpU);
        _output.WriteLine($"Login result gives a Result: {result.Result}\nUser: {result.Value}");

        // * ASSERT
        Assert.NotNull(result.Value);
        Assert.IsType<User>(result.Value);
    }

    [Fact]
    public async void LogoutReturnsOk(){
        // * ARRANGE
        var controller = new AuthController(_fixture.Context);

        // * ACT
        var result = controller.Logout();
        _output.WriteLine($"Logout produces: {result}");

        // * ASSERT
        Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
    }
}
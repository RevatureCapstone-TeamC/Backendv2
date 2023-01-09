//using ECommerce.Data;
using ECommerce.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//var connValue = builder.Configuration["ECommerce:ConnectionString"];

//builder.Services.AddDbContext<CommerceContext>(opt => opt.UseSqlServer(connValue));

//builder.Services.AddScoped<IContext>(provider => provider.GetService<Context>());

var ECommerceAPI = "_ECommerceAPI";

var connectionString = builder.Configuration["ConnectionStrings:ECommerce"];

builder.Services.AddDbContext<CommerceContext>(opts =>
    opts.UseSqlServer(connectionString)
);

builder.Services.AddCors(options => {
    options.AddPolicy(name: ECommerceAPI,
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        });
});

/* builder.Services.AddSingleton<IRepository>
    (sp => new SQLRepository(connectionString, sp.GetRequiredService<ILogger<SQLRepository>>())); */
//builder.Services.AddDbContext<CommerceContext>( opts => 
//    opts.UseSqlServer(connectionString)
//);

builder.Services.AddMvc().AddControllersAsServices();
builder.Services.AddControllers();


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseCors(ECommerceAPI);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

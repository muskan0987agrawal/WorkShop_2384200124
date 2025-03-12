using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoryLayer.Context;

var builder = WebApplication.CreateBuilder(args);

//Databse connectivity
var connectionString = builder.Configuration.GetConnectionString("sqlConnection");
builder.Services.AddDbContext<AddressBookDbContext>(options => options.UseSqlServer(connectionString));

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

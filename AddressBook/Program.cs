using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //Databse connectivity
        var connectionString = builder.Configuration.GetConnectionString("sqlConnection");
        builder.Services.AddDbContext<AddressBookDbContext>(options => options.UseSqlServer(connectionString));

        // Add services to the container.

        builder.Services.AddControllers();

        //Register Dependencies
        builder.Services.AddScoped<IAddressBookBL, AddressBookBL>();
        builder.Services.AddScoped<IAddressBookRL, AddressBookRL>();

        //Swagger configuration
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
 
        var app = builder.Build();
        app.UseSwagger();
        app.UseSwaggerUI();
        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
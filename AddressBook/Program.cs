using BusinessLayer.Interface;
using BusinessLayer.Service;
using BusinessLayer.Validator;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using BusinessLayer.Mapping;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //Databse connectivity
        var connectionString = builder.Configuration.GetConnectionString("sqlConnection");
        builder.Services.AddDbContext<AddressBookDbContext>(options => options.UseSqlServer(connectionString));

        
        builder.Services.AddAutoMapper(typeof(AddressBookProfile));

        builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
        builder.Services.AddScoped<IValidator<RequestAddressBook>, RequestAddressBookValidator>();


        // Add services to the container.

        builder.Services.AddControllers();

        //Register Dependencies
        builder.Services.AddScoped<IAddressBookBL, AddressBookBL>();
        builder.Services.AddScoped<IAddressBookRL, AddressBookRL>();

        //Swagger configuration
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSwaggerGen(options =>
        {
            var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });


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
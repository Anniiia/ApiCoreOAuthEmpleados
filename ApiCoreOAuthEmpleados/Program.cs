using ApiCoreOAuthEmpleados.Data;
using ApiCoreOAuthEmpleados.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connectionString = builder.Configuration.GetConnectionString("SqlAzure");
builder.Services.AddTransient<RepositoryEmpleados>();
builder.Services.AddDbContext<HospitalContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Api Outh Empleados",
        Description = "Api con Token de Seguridad"
    });
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(options => 
{
    options.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Api Oauth Empleados");
    options.RoutePrefix = "";
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

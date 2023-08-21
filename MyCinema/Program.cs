using System.Runtime.InteropServices.ComTypes;
using Microsoft.EntityFrameworkCore;
using MyCinema.Model;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CinemaContext>(optionsAction: _=>
{
    _.UseMySQL(connectionString:"Server=localhost;Database=cinema;Uid=root;Pwd=2792001dung");
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = "your_issuer_here", // Replace with your JWT issuer
            ValidAudience = "your_audience_here", // Replace with your JWT audience
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key_here")) // Replace with your JWT secret key
        };
    });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}







app.UseHttpsRedirection();

// Them cau hinh goi local Image
app.UseStaticFiles();
app.UseCors();
//them Authorization
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
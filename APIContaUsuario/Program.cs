using APIContaUsuario.Contexto;
using APIContaUsuario.Repository;
using APIContaUsuario.Repository.Interface;
using APIContaUsuario.TokenSettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var ConnectionStrings = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(ConnectionStrings));

builder.Services.AddScoped<IProfileUser, RepositoryBase>();
builder.Services.AddScoped<IRoleUser, RepositoryBase>();
builder.Services.AddScoped<IRefreshToken, RepositoryBase>();

builder.Services.AddCors();
builder.Services.AddControllers();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin",
      policy =>
      {
          policy.RequireRole("Admin");
      });

    options.AddPolicy("Member",
      policy =>
      {
          policy.RequireRole("Member");
      });
});

var key = Encoding.ASCII.GetBytes(Settings.Key);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(options => options
   .AllowAnyOrigin()
   .AllowAnyMethod()
   .AllowAnyHeader());

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

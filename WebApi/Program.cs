using Application;
using Application.Common.SQL;
using Application.CQRS.Tasks.Handlers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.
            AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString))
            .BuildServiceProvider();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


builder.Services.AddTransient<SqlManager>(provider => new SqlManager(connectionString));

builder.Services.AddMediatR(typeof(GetTaskByIdQueryHandler).Assembly);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowAllOrigins");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

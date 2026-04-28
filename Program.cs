using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoContext>(opt => opt.UseSqlite("Data Source = TodoDto.db"));
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

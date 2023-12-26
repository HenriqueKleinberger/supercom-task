using Microsoft.EntityFrameworkCore;
using SupercomTask.BLL;
using SupercomTask.BLL.Interfaces;
using SupercomTask.DAL;
using SupercomTask.DAL.Interfaces;
using SupercomTask.Models;
using SupercomTask.Utils.Time.Interfaces;
using SupercomTask.Utils.Time;
using FluentValidation;
using System;
using SupercomTask.DTO;
using SupercomTask.Validators;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddMvc();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<SuperComTaskContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));

builder.Services.AddScoped<ICardDAL, CardDAL>();
builder.Services.AddScoped<IStatusDAL, StatusDAL>();
builder.Services.AddScoped<ICardBLL, CardBLL>();
builder.Services.AddScoped<ITimeHelper, TimeHelper>();
builder.Services.AddValidatorsFromAssemblyContaining<CardDTOValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
public partial class Program { }

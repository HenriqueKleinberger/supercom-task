using Microsoft.EntityFrameworkCore;
using SupercomTask.BLL;
using SupercomTask.BLL.Interfaces;
using SupercomTask.DAL;
using SupercomTask.DAL.Interfaces;
using SupercomTask.Models;
using SupercomTask.Utils.Time.Interfaces;
using SupercomTask.Utils.Time;
using FluentValidation;
using SupercomTask.Validators;
using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "Send expirated tasks to queue";
});

builder.Services.AddControllers();
builder.Services.AddMvc();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy
                          .WithOrigins("http://localhost:3000")
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                      });
});

builder.Services.AddScoped<IConnection>(provider =>
{
    var factory = new ConnectionFactory
    {
        HostName = "localhost",
        Port = 5672,
        UserName = "guest",
        Password = "guest",
    };

    return factory.CreateConnection();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<SuperComTaskContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<ICardDAL, CardDAL>();
builder.Services.AddScoped<IStatusDAL, StatusDAL>();
builder.Services.AddScoped<ICardBLL, CardBLL>();
builder.Services.AddScoped<ITimeHelper, TimeHelper>();
builder.Services.AddValidatorsFromAssemblyContaining<CardDTOValidator>();
builder.Services.AddHostedService<SendExpiredTasksToQueueService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();
app.UseCors(MyAllowSpecificOrigins);
app.Run();
public partial class Program { }

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
using SupercomTask.Config;

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

builder.Services.AddConfiguration<RabbitMqConfiguration>(builder.Configuration, "RabbitMq");

builder.Services.AddLogging(builder =>
{
    builder.AddConsole(); // Log to the console
    builder.AddDebug();   // Log to the debug output window
                          // Add other logging providers as needed
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<SuperComTaskContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IBaseDAL, BaseDAL>();
builder.Services.AddScoped<ICardDAL, CardDAL>();
builder.Services.AddScoped<IStatusDAL, StatusDAL>();
builder.Services.AddScoped<ICardBLL, CardBLL>();
builder.Services.AddScoped<IExpirationPublishedBLL, ExpirationPublishedBLL>();
builder.Services.AddScoped<IExpirationPublishedDAL, ExpirationPublishedDAL>();
builder.Services.AddScoped<ITimeHelper, TimeHelper>();
builder.Services.AddValidatorsFromAssemblyContaining<CardDTOValidator>();
builder.Services.AddHostedService<SendExpiredTasksToQueueService>();
builder.Services.AddHostedService<LogExpiredTasks>();


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

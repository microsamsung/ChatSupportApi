using ChatSupportApi.Repository;
using ChatSupportApi.Repository.Implementation;
using ChatSupportApi.Services.Contracts;
using ChatSupportApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DBConnection")));

builder.Services.AddScoped<IAgentsRepository, AgentsRepository>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddSingleton<ChatQueueMonitor>();
builder.Services.AddHostedService<ChatQueueMonitor>();


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

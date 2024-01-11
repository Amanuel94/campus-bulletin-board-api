using Board.Common.Mongo;
using Board.User.Service.Settings;
using Board.Common.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMongo()
                .AddPersistence<Board.Channel.Service.Model.Channel>("channels")
                .AddPersistence<Board.Channel.Service.Model.UserItem>("useritem");

builder.Services.AddMassTransitWithRabbitMQ();

builder.Services.AddAuth();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

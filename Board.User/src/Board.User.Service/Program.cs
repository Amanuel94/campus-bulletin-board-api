using Board.Common.Mongo;
using Board.User.Service.Models;
using Board.User.Service.Password;
using Board.User.Service.PasswordService.Interfaces;
using Board.User.Service.Settings;
using Board.Common.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddMongo()
                .AddPersistence<User>("User");
builder.Services.AddMassTransitWithRabbitMQ();

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddAuth();
// builder.Services.AddMassTransitHostedService();
builder.Services.AddControllers(options => {
    options.SuppressAsyncSuffixInActionNames = false;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

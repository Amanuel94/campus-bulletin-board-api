using Board.Common.Mongo;
using Board.User.Services.Models;
using Board.User.Services.Password;
using Board.User.Services.PasswordService.Interfaces;
using Board.User.Services.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddMongo()
                .AddPersistence<User>("User");

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

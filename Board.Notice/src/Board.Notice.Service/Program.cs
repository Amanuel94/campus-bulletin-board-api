using Board.Common.Mongo;
using Board.Notice.Service.Model;
using Board.Notice.Service.Policies;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddMongo().AddPersistence<ChannelItem>("channelItem");
builder.Services.AddMongo().AddPersistence<Notice>("notice");
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ChannelCreatorPolicy", policy =>
    {
        policy.Requirements.Add(new ChannelCreatorRequirement(Guid.Empty));
    });
});

builder.Services.AddSingleton<IAuthorizationHandler, ChannelCreatorAuthorizationHandler>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();

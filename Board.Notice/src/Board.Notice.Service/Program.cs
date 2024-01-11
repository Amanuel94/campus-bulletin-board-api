// using System.Text;
using Board.Auth.Jwt.Interfaces;
using Board.Auth.Service.Jwt;
using Board.Common.Mongo;
using Board.Common.RabbitMQ;
using Board.Notice.Service.Model;
using Board.Notice.Service.Policies;
using Board.User.Service.Settings;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
// using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddMongo().AddPersistence<ChannelItem>("channelItem").AddPersistence<Notice>("notice");
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidateAudience = true,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,
//             ValidIssuer = builder.Configuration["JWTSettings:Issuer"],
//             ValidAudience = builder.Configuration["JWTSettings:Audience"],
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSettings:Key"]))
//         };
//     });

builder.Services.AddIdentityAuth();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ChannelCreatorPolicy", policy =>
    {
        policy.Requirements.Add(new ChannelCreatorRequirement(Guid.Empty));
    });
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.AddSingleton<IAuthorizationHandler, ChannelCreatorAuthorizationHandler>();

builder.Services.AddMassTransitWithRabbitMQ();

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

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

// app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.Run();

using Board.Auth.Jwt.Interfaces;
using Board.Auth.Service.Jwt;
using Board.Common.Mongo;
using Board.Common.RabbitMQ;
using Board.Common.Settings;
using Board.Notice.Service.Model;
using Board.Notice.Service.Policies;
using Microsoft.AspNetCore.Authorization;
using Polly;
using Polly.Timeout;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddMongo().AddPersistence<ChannelItem>("channelItem").AddPersistence<Notice>("notice");

builder.Services.AddIdentityAuth();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ChannelCreatorPolicy", policy =>
    {
        policy.Requirements.Add(new ChannelCreatorRequirement(Guid.Empty));
    });
});

builder.Services.AddHttpContextAccessor();

var seed = new Random();
builder.Services.AddHttpClient<NotificationClient>(client =>{
    client.BaseAddress = new Uri(builder.Configuration["NotificationServiceUrl"]!);
}
).ConfigurePrimaryHttpMessageHandler(_ => {
    var clientHandler = new HttpClientHandler();
    clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };


    return clientHandler;

}).AddTransientHttpErrorPolicy(builder => builder.Or<TimeoutRejectedException>().WaitAndRetryAsync(
    5,
    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(seed.Next(0, 1000))
  ))
.AddTransientHttpErrorPolicy(builder => builder.Or<TimeoutRejectedException>().CircuitBreakerAsync(
    3,
    TimeSpan.FromSeconds(15)
))
.AddPolicyHandler(Polly.Policy.TimeoutAsync<HttpResponseMessage>(1));


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

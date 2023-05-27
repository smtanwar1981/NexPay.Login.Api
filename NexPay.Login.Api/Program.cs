using NexPay.Login.Api.Core;
using NexPay.Login.Api.Repository;
using NexPay.Login.Api.Service;
using NexPay.Publisher.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options => AuthenticationScheme.AddScheme(options))
    .AddJwtBearer(o => AuthenticationScheme.AddBearer(o, builder));
builder.Services.AddAuthentication();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMessagePublisher, MessagePublisher>();
builder.Services.AddSwaggerGen(options =>
{
    SwaggerDefinition.AddSwaggerDefinition(options);
});

builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

app.UseCors(builder =>
        builder
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
    );

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/swagger/V1/swagger.json", "NexPay Login Api");
    });
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

using AcceralytDevTest.datasources;
using AcceralytDevTest.models;
using AcceralytDevTest.repository;
using AcceralytDevTest.services;
using AcceralytDevTest.utils;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var elasticsearchUri = builder.Configuration["Elasticsearch__Uri"];
if (string.IsNullOrEmpty(elasticsearchUri))
{
    throw new InvalidOperationException("Elasticsearch URI not configured in appsettings.");
}

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri(elasticsearchUri))
    {
        AutoRegisterTemplate = true,
        IndexFormat = "logstash-{0:yyyy.MM.dd}"
    })
    .CreateLogger();

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddSerilog();
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<PostgreSqldbContext>(options=>options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHandler, PasswordHandler>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();



app.Run();


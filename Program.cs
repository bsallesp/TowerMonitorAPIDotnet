using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Azure.Cosmos;
using TowerApi.API.Background;
using TowerApi.Application.Services;
using TowerApi.Application.Services.Interfaces;
using TowerApi.Domain.Repositories;
using TowerApi.Domain.Repositories.Interfaces;
using TowerApi.Infrastructure.Data;
using TowerApi.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

var keyVaultUrl = new Uri("https://vb-keyvault-vb2025.vault.azure.net/");
var secretClient = new SecretClient(vaultUri: keyVaultUrl, credential: new DefaultAzureCredential());

var sqlSecret = await secretClient.GetSecretAsync("SqlConnectionString");
var sqlConnectionString = sqlSecret.Value.Value;

var cosmosSecret = await secretClient.GetSecretAsync("CosmosConnectionString");
var cosmosConnectionString = cosmosSecret.Value.Value;

builder.Services.AddDbContext<SqlServerDbContext>(options =>
    options.UseSqlServer(sqlConnectionString));

builder.Services.AddSingleton(sp => new CosmosClient(cosmosConnectionString));

builder.Services.AddScoped<ITowerRepository, TowerRepository>();
builder.Services.AddScoped<ITowerLiveStatusRepository, TowerLiveStatusRepository>();
builder.Services.AddScoped<ITowerStatusSimulatorService, TowerStatusSimulatorService>();
builder.Services.AddScoped<TowerStatusAggregationService>();

builder.Services.AddHostedService<TowerStatusSimulatorHostedService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.MapGet("/", () => "API Running!");

app.Run();
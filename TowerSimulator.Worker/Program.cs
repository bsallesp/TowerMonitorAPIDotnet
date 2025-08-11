using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using TowerApi.Domain.Repositories;
using TowerApi.Domain.Repositories.Interfaces;
using TowerApi.Infrastructure.Data;
using TowerApi.Infrastructure.Repositories;
using TowerSimulator.Worker;

var keyVault = new SecretClient(new Uri("https://vb-keyvault-vb2025.vault.azure.net/"), new DefaultAzureCredential());
var sql = (await keyVault.GetSecretAsync("SqlConnectionString")).Value.Value;
var cosmos = (await keyVault.GetSecretAsync("CosmosConnectionString")).Value.Value;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<SqlServerDbContext>(o => o.UseSqlServer(sql));
builder.Services.AddSingleton(new CosmosClient(cosmos));
builder.Services.AddScoped<ITowerRepository, TowerRepository>();
builder.Services.AddScoped<ITowerLiveStatusRepository, TowerLiveStatusRepository>();
builder.Services.AddHostedService<TowerStatusSimulatorHostedService>();

await builder.Build().RunAsync();
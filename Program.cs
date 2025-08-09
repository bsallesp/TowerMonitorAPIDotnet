using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using TowerApi.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

var keyVaultUrl = new Uri("https://vb-keyvault-vb2025.vault.azure.net/");
var client = new SecretClient(vaultUri: keyVaultUrl, credential: new DefaultAzureCredential());
KeyVaultSecret secret = client.GetSecret("SqlConnectionString");
var connectionString = secret.Value;

builder.Services.AddDbContext<SqlServerDbContext>(options =>
    options.UseSqlServer(connectionString));

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
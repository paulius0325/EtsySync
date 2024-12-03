using EtsySync.Data;
using EtsySync.Services;
using Microsoft.EntityFrameworkCore;
using EtsyGateway;
using EtsySync.Interface;
using EtsySync.Repositories;



var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddDbContext<InvoiceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<EncryptionService>();
builder.Services.AddScoped<IInvoiceGeneratorService, InvoiceGeneratorService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<SalesDataTableService>();
builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<ISalesDataService, SalesDataService>();
builder.Services.AddScoped<SellerService>();
builder.Services.AddTransient<ReceiptTransactionsService>();
builder.Services.AddTransient<ICsvParserService, CsvParserService>();

//builder.Services.AddSingleton<OauthService>();
//builder.Services.AddTransient<IReceiptTransactionsClient, ReceiptTransactionsClient>();
//builder.Services.AddScoped<ISerialNumberService, SerialNumberService>();
//builder.Services.AddHttpClient<ReceiptTransactionsClient>(client =>
//{
//    client.BaseAddress = new Uri("https://openapi.etsy.com/");
//});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseRouting();

await app.RunAsync();







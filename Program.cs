using ChartsExampleServer.Subscriptions.MiddleWare;
using ChartsExampleServer.Subscriptions;
using ChartsExampleServer.Models;
using ChartsExampleServer.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddChartsExampleSubscriptionsServices();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS ayarlarý
builder.Services.AddCors(opt => opt.AddPolicy("AllowSpecificOrigin", policy => policy
       .WithOrigins("http://localhost:4200", "https://localhost:4200") // Angular istemcinin URL'sini ekledik
       .AllowAnyHeader()
       .AllowAnyMethod()
       .AllowCredentials()
));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();
app.UseExceptionHandler("/error");


app.UseChartsExampleDataBaseSubscriptions<DataBaseSubscription<Personel>>("Personels");
app.UseChartsExampleDataBaseSubscriptions<DataBaseSubscription<Sale>>("Sales");


app.MapControllers();
app.MapHub<SalesHub>("/sales-hub");

app.Run();

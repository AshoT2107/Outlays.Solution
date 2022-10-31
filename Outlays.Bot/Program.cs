using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Outlays.Bot.Repository;
using Outlays.Bot.Services;
using Outlays.Data.Data;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<OutlaysDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("OutlaysDb"));
});

builder.Services.AddHttpClient<ITelegramBotClient>("tgwebhook")
                .AddTypedClient<ITelegramBotClient>((client,sp)=>
                new TelegramBotClient(builder.Configuration["BotToken"],client));

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<TelegramBotService>();
builder.Services.AddScoped<RoomRepository>();
builder.Services.AddScoped<OutlayRepository>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using DeviceGateway.MessageQueue;
using DeviceGateway.MessageQueue.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Configure for MQTT            
builder.Services.AddSingleton<IMqttPublisher, MqttPublisher>();
builder.Services.AddHostedService<MqttSubcriber>();
// Configure for RabbitMQ
//services.AddHostedService<RabbitMqConsumer>();
builder.Services.AddTransient<IRabbitMqPublisher, RabbitMqPublisher>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

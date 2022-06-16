
using DeviceGateway.MessageQueue.Interface;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceGateway.MessageQueue
{
    public class RabbitMqPublisher : IRabbitMqPublisher
    {
        private static IModel model;

        protected IConnection _connection;
        protected IModel _channel;

        protected string exchange;
        protected string queue;
        protected ConnectionFactory factory;

        public RabbitMqPublisher(IConfiguration configuration)
        {
            var appName = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}";

            exchange = configuration.GetValue<string>("RabbitMq:Exchange");
            queue = $"{configuration.GetValue<string>("RabbitMq:Queue")}-{appName}-{Guid.NewGuid()}";
            var routeKey = configuration.GetValue<string>("RabbitMq:RouteKey");
            var uri = configuration.GetValue<string>("RabbitMq:Uri");

            Console.WriteLine($"Init RabbitMQ publisher at uri: {uri}, exchange: {exchange}, and queue: {queue}, routeKey: {routeKey}");
         
            factory = new ConnectionFactory { Uri = new System.Uri(uri) };

            // create connection  
            _connection = factory.CreateConnection();

            // create channel  
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange, ExchangeType.Fanout);
            _channel.QueueDeclare(queue, false, false, false, null);
            //_channel.QueueBind(queue, exchange, routeKey, null);
            _channel.BasicQos(0, 1, false);


            model = factory.CreateConnection().CreateModel();
            model.ExchangeDeclare(exchange, ExchangeType.Fanout);
    

        }

        public async Task PublishData(string data, CancellationToken cancellationToken = default)
        {
            byte[] messageBuffer = System.Text.Encoding.UTF8.GetBytes(data);

            model.BasicPublish(exchange, "", false, model.CreateBasicProperties(), messageBuffer);
        }

        
    }
}

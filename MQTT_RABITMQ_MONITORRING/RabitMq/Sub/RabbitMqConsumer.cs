
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceGateway.MessageQueue
{
    public class RabbitMqConsumer : BackgroundService
    {
        protected IConnection _connection;
        protected IModel _channel;

        protected string exchange;
        protected string queue;
        protected ConnectionFactory factory;

        protected string topicHeader;


        public RabbitMqConsumer(IConfiguration configuration)
        {
            var appName = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}";
            var routeKey = configuration.GetValue<string>("RabbitMq:RouteKey", "FireSafeServer.Queue.*");
            var uri = configuration.GetValue<string>("RabbitMq:Uri", "localhost");


            exchange = configuration.GetValue<string>("RabbitMq:Exchange", "FireSafeServerExchange");
            queue = $"{configuration.GetValue<string>("RabbitMq:Queue", "FireSafeServerQueue")}-{appName}-{Guid.NewGuid()}";
            topicHeader = configuration.GetValue<string>("Mqtt:TopicHeader", "fire-safe");

            Console.WriteLine($"Init RabbitMQ subcriber at uri: {uri}, exchange: {exchange}, and queue: {queue}, routeKey: {routeKey}");

        


                factory = new ConnectionFactory { Uri = new System.Uri(uri) };

                // create connection  
                _connection = factory.CreateConnection();

                // create channel  
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange, ExchangeType.Fanout);
                _channel.QueueDeclare(queue, false, false, false, null);
                _channel.QueueBind(queue, exchange, routeKey, null);
                _channel.BasicQos(0, 1, false);

                _connection.ConnectionShutdown += BrokerShutdown;
   

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                await ProcessNewMessage(ea.Body.ToArray());
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            _channel.BasicConsume(queue, false, consumer);
        }

        // Nhận mess từ MQTT showw thông tin pin yếu ra
        private async Task ProcessNewMessage(byte[] message)
        {
   
                Console.WriteLine($"Consumer received {Encoding.UTF8.GetString(message)}");

                await Task.Delay(0);
       
        }


        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            Console.WriteLine($"Consummer cancelled... {JsonConvert.SerializeObject(e)}");
        }
        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
            Console.WriteLine($"Consummer unregistered... {JsonConvert.SerializeObject(sender)}, {JsonConvert.SerializeObject(e)}");
        }
        private void OnConsumerRegistered(object sender, ConsumerEventArgs e)
        {
            Console.WriteLine($"Consummer registered...");
        }
        private void OnConsumerShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine($"Consummer shutdowned... {JsonConvert.SerializeObject(e)}");
        }
        private void BrokerShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine($"Broker shutdowned... {JsonConvert.SerializeObject(e)}");
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }

    }
}

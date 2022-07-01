
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceGateway.MessageQueue
{
    public class MqttPublisher : Interface.IMqttPublisher
    {
        private IMqttClient mqttClient;
        //IConfiguration configuration;
        public MqttPublisher(IConfiguration configuration)
        {
            var serverAddress = configuration.GetValue<string>("Mqtt:ServerAddress");
            var serverPort = configuration.GetValue<int>("Mqtt:ServerPort"); 
            var userName = configuration.GetValue<string>("Mqtt:UserName");
            var password = configuration.GetValue<string>("Mqtt:PassWord");
            var subcribeTopic = configuration.GetValue<string>("Mqtt:SubcribeTopic");

            var optionsBuilder = new MqttClientOptionsBuilder()
                    .WithClientId("FireSafeInventory-" + DateTime.Now.Ticks)
                    .WithTcpServer(serverAddress, serverPort)
                    .WithCredentials(userName, password)
                    .WithCleanSession()
                    .Build();

            mqttClient = new MqttFactory().CreateMqttClient();

            Task.Run(async () => await mqttClient.ConnectAsync(optionsBuilder, CancellationToken.None));

        }

        public async Task PublishMessageAsync(string topic, string payload, bool Retain = false)
        {
            try
            {
                var applicationMessage = new MqttApplicationMessageBuilder()
                        .WithTopic(topic)
                        .WithPayload(payload)
                        .WithExactlyOnceQoS()
                        .WithRetainFlag(Retain)
                        .Build();

                var result = await mqttClient.PublishAsync(applicationMessage, System.Threading.CancellationToken.None);
            }
            catch (Exception ex)
            {
                //Log.Error(ex, $"Topic: {topic}, payload: {payload}");
            }

        }
    }
}

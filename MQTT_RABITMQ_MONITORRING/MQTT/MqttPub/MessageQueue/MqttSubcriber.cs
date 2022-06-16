
using DeviceGateway.MessageQueue.Interface;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MqttPub.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace DeviceGateway.MessageQueue
{
    public class MqttSubcriber : BackgroundService
    {
        private IMqttClient mqttClient;
        ConcurrentQueue<MqttApplicationMessage> messageQueue;
        
        IMqttClientOptions optionsBuilder;
        string subcribeTopic;
        IRabbitMqPublisher rabbitMqPublisher;

        private int batteryLow;
        private string serverAddress;
        private string userName;
        private string password;
        private int serverPort;

        public MqttSubcriber(IConfiguration configuration, IRabbitMqPublisher rabbitMqPublisher )
        {
            serverAddress = configuration.GetValue<string>("Mqtt:ServerAddress");
            serverPort = configuration.GetValue<int>("Mqtt:ServerPort");
            userName = configuration.GetValue<string>("Mqtt:UserName");
            password = configuration.GetValue<string>("Mqtt:PassWord");
            subcribeTopic = configuration.GetValue<string>("Mqtt:SubcribeTopic");
            batteryLow = configuration.GetValue<int>("BatteryLow", 20);

            optionsBuilder = new MqttClientOptionsBuilder()
                    .WithClientId("FireSafeInventory-" + DateTime.Now.Ticks)
                    .WithTcpServer(serverAddress, serverPort)
                    .WithCredentials(userName, password)
                    .WithCleanSession()
                    .Build();

            mqttClient = new MqttFactory().CreateMqttClient();

            messageQueue = new ConcurrentQueue<MqttApplicationMessage>();

            Console.WriteLine($"Init MQTT subcriber at server address: {serverAddress}, port: {serverPort}, user name: {userName}, password: {password}");
            this.rabbitMqPublisher = rabbitMqPublisher;
        }

        public async Task ApplicationMessageReceivedHandler(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            try
            {
                messageQueue.Enqueue(eventArgs.ApplicationMessage);
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "MQTT AddNewMessage");
            }

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await mqttClient.ConnectAsync(optionsBuilder, CancellationToken.None);
            }
            catch (Exception ex)
            {
               // Log.Fatal(ex, $"MQTT not connected with: Server Address: {serverAddress} - Port: {serverPort} - UserName: {userName}");

            }
            await mqttClient.SubscribeAsync(subcribeTopic, MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce);
            mqttClient.UseApplicationMessageReceivedHandler(async e => await ApplicationMessageReceivedHandler(e));
            mqttClient.UseDisconnectedHandler(async e => await MqttDisconnectedEvent(e));

            while (!stoppingToken.IsCancellationRequested)
            {
                if (messageQueue.TryDequeue(out MqttApplicationMessage message))
                {
                    try
                    {
                        await ProcessNewMessage(message);
                    }
                    catch (Exception ex)
                    {
                       // Log.Error(ex, $"Topic: {message.Topic}, payload: {message.Payload}");
                    }
                }

                await Task.Delay(10);
            }
        }

        private async Task MqttDisconnectedEvent(MQTTnet.Client.Disconnecting.MqttClientDisconnectedEventArgs e)
        {
           // Log.Fatal($"MQTT disconnected: {JsonConvert.SerializeObject(e)}");

            await Task.Delay(5000);
            
            await mqttClient.ConnectAsync(optionsBuilder, CancellationToken.None);
            await mqttClient.SubscribeAsync(subcribeTopic, MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce);
        }


        // Nhận mes và gửi tin lên rabitMQ
        private async Task ProcessNewMessage(MqttApplicationMessage message)
        {
          
            Console.WriteLine("tappic: "+message.Topic );

            var payload = Encoding.UTF8.GetString(message.Payload);
            var batery = JsonConvert.DeserializeObject<Baterrys>(payload);
            Console.WriteLine("payload: "+ batery.Baterry);

            if (batery.Baterry < 10)
            {
                await rabbitMqPublisher.PublishData("Pin yếu");
            }

        }

       
    }
}

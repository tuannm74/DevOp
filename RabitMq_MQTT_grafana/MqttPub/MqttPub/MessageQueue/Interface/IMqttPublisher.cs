using System.Threading;
using System.Threading.Tasks;

namespace DeviceGateway.MessageQueue.Interface
{
    public interface IMqttPublisher
    {
        Task PublishMessageAsync(string topic, string payload, bool Retain = false);
    }
}

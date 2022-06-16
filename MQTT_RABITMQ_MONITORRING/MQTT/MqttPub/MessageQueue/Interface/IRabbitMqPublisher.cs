
using System.Threading;
using System.Threading.Tasks;

namespace DeviceGateway.MessageQueue.Interface
{
    public interface IRabbitMqPublisher
    {
        Task PublishData(string data, CancellationToken cancellationToken = default);
    }
}

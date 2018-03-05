using Ribe.Messaging;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Ribe.DotNetty.Client
{
    public class InvocationCompletionSource 
    {
        private ConcurrentDictionary<string, TaskCompletionSource<IMessage>> _map;

        public InvocationCompletionSource()
        {
            _map = new ConcurrentDictionary<string, TaskCompletionSource<IMessage>>();
        }

        public void SetResult(string requestId, IMessage message)
        {
            if (_map.TryRemove(requestId, out var tcs))
            {
                tcs.SetResult(message);
            }
        }

        public Task<IMessage> GetResult(string requestId)
        {
            return _map.GetOrAdd(requestId, (k) => new TaskCompletionSource<IMessage>()).Task;
        }
    }
}

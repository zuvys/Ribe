using System;
using System.Collections.Concurrent;

namespace Ribe.Rpc.Messaging.Formatting
{
    public class MessageFormatterManager : IMessageFormatterManager
    {
        private ConcurrentDictionary<Type, IMessageFormatter> _formatters;

        public MessageFormatterManager()
        {
            _formatters = new ConcurrentDictionary<Type, IMessageFormatter>();
        }

        public IMessageFormatter GetFormatter(Message message)
        {
            foreach (var formatter in _formatters.Values)
            {
                if (formatter.IsFormatSupported(message))
                {
                    return formatter;
                }
            }

            //log
            throw new NotSupportedException("not support the message's codecs!");
        }

        public void RemoveFormatter(IMessageFormatter formatter)
        {
            _formatters.TryRemove(formatter.GetType(), out var _);
        }

        public void AddFormatter(IMessageFormatter formatter)
        {
            _formatters.AddOrUpdate(formatter.GetType(), formatter, (k, v) => formatter);
        }
    }
}

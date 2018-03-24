using System;
using System.Collections.Generic;

namespace Ribe.Messaging
{
    public class MessageFormatterProvider : IMessageFormatterProvider
    {
        private IEnumerable<IMessageFormatter> _formatters;

        public MessageFormatterProvider(IEnumerable<IMessageFormatter> formatters)
        {
            _formatters = formatters;
        }

        public IMessageFormatter GetFormatter(Message message)
        {
            foreach (var formatter in _formatters)
            {
                if (formatter.IsFormatSupported(message))
                {
                    return formatter;
                }
            }

            //log
            throw new NotSupportedException("not support the message's codecs!");
        }
    }
}

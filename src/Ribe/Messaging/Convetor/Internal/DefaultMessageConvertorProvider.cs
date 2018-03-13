using System;
using System.Collections.Generic;

namespace Ribe.Messaging.Internal
{
    public class DefaultMessageConvertorProvider : IMessageConvertorProvider
    {
        private IEnumerable<IMessageConvertor> _convertors;

        public DefaultMessageConvertorProvider(IEnumerable<IMessageConvertor> convertors)
        {
            _convertors = convertors;
        }

        public IMessageConvertor GetConvertor(Message message)
        {
            foreach (var convertor in _convertors)
            {
                if (convertor.CanConvert(message))
                {
                    return convertor;
                }
            }

            //log
            throw new NotSupportedException("not support the message's codecs!");
        }
    }
}

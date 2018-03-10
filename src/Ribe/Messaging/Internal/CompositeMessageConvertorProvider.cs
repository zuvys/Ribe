using System;
using System.Collections.Generic;
using System.Linq;

namespace Ribe.Messaging.Internal
{
    public class CompositeMessageConvertorProvider : IMessageConvertorProvider
    {
        private List<IMessageConvertor> _convertors;

        public CompositeMessageConvertorProvider(IEnumerable<IMessageConvertor> convertors)
        {
            _convertors = convertors.ToList();
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

        public void AddConvertor(IMessageConvertor convertor)
        {
            if (!_convertors.All(i => i.GetType() == convertor.GetType()))
            {
                _convertors.Add(convertor);
            }
        }
    }
}

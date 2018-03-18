﻿using Ribe.Messaging;

namespace Ribe.Codecs
{
    public interface IEncoder
    {
        bool CanEncode(string formatType);

        byte[] Encode(Message message);
    }
}

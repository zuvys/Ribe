﻿using Ribe.Rpc.Core;
using Ribe.Rpc.Messaging;
using System;
using System.Threading.Tasks;

namespace Ribe.Rpc.Transport
{
    public interface IMessageListener
    {
        Task ReceiveAsync(Message message, Func<long, Response, Task> onCompleted);
    }
}

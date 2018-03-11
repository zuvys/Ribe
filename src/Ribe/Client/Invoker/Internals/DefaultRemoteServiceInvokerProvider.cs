﻿using Ribe.Core.Service.Address;
using Ribe.Infrustructure;
using Ribe.Serialize;
using System;

namespace Ribe.Client.Invoker.Internals
{
    public class DefaultRemoteServiceInvokerProvider : IRemoteServiceInvokerProvider
    {
        private IRpcClientFacotry _clientFacotry;

        public DefaultRemoteServiceInvokerProvider(IRpcClientFacotry clientFacotry)
        {
            _clientFacotry = clientFacotry;
        }

        public IRemoteServiceInvoker GetInvoker()
        {
            //Select one ServiceAddress
            return new DefaultRemoteServiceInvoker(_clientFacotry)
            {
                ServiceAddress = new ServiceAddress()
                {
                    Ip = "127.0.0.1",
                    Port = 8080
                }
            };
        }
    }
}

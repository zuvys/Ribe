﻿namespace Ribe.Serialize
{
    public interface ISerializerProvider
    {
        ISerializer GetSerializer();
    }
}
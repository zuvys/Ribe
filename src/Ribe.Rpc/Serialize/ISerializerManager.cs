namespace Ribe.Rpc.Serialize
{
    public interface ISerializerManager
    {
        ISerializer GetSerializer(string formatType);

        void AddSerializer(ISerializer serializer);

        void RemoveSerializer(ISerializer serializer);
    }
}

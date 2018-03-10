namespace Ribe.Messaging
{
    public interface IMessageConvertorProvider
    {
        IMessageConvertor GetConvertor(Message message);

        void AddConvertor(IMessageConvertor convertor);
    }
}

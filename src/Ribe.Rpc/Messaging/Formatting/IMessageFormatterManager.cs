namespace Ribe.Rpc.Messaging.Formatting
{
    public interface IMessageFormatterManager
    {
        IMessageFormatter GetFormatter(Message message);

        void AddFormatter(IMessageFormatter formatter);

        void RemoveFormatter(IMessageFormatter formatter);
    }
}

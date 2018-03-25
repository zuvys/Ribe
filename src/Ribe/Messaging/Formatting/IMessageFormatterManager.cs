namespace Ribe.Messaging
{
    public interface IMessageFormatterManager
    {
        IMessageFormatter GetFormatter(Message message);

        void AddFormatter(IMessageFormatter formatter);

        void RemoveFormatter(IMessageFormatter formatter);
    }
}

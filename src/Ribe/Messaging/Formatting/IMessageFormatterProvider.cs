namespace Ribe.Messaging
{
    public interface IMessageFormatterProvider
    {
        IMessageFormatter GetFormatter(Message message);
    }
}

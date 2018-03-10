using System.Threading.Tasks;

namespace Ribe
{
    public static class TaskExtensions
    {
        public static void WithNoWaiting(this Task _)
        {

        }

        public static void WithNoWaiting<T>(this Task<T> _)
        {

        }
    }
}

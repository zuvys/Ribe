namespace Ribe.Core
{
    public class Result
    {
        public string Error { get; set; }

        public object Data { get; set; }

        public string RequestId { get; set; }

        public Status Status { get; set; }
    }
}

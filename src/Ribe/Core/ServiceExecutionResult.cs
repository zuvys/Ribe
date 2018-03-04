namespace Ribe.Core
{
    public class ServiceExecutionResult
    {
        public string Error { get; set; }

        public object Data { get; set; }

        public string RequestId { get; set; }

        public ServiceExecutionStatus Status { get; set; }
    }
}

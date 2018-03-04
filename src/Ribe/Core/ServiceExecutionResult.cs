namespace Ribe.Core
{
    public class ServiceExecutionResult
    {
        public string Error { get; set; }

        public object Result { get; set; }

        public string RequestId { get; set; }

        public ServiceExecutionStatus Status { get; set; }
    }
}

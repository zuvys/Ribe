namespace Ribe.Core
{
    /// <summary>
    /// the response of service execution
    /// </summary>
    public class Response
    {
        public string Error { get; set; }

        public object Data { get; set; }

        public Status Status { get; set; }

        public static Response Ok(object data)
        {
            return Create(data, string.Empty, Status.Success);
        }

        public static Response Failed(string error)
        {
            return Create(null, error, Status.Failed);
        }

        public static Response Create(object data, string error, Status status)
        {
            return new Response()
            {
                Data = data,
                Error = error,
                Status = status
            };
        }
    }
}

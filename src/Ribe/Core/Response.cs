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
            return new Response()
            {
                Error = string.Empty,
                Data = data,
                Status = Status.Success
            };
        }

        public static Response Failed(string error)
        {
            return new Response()
            {
                Data = null,
                Error = error,
                Status = Status.Success
            };
        }
    }
}

namespace Ribe.Core
{
    /// <summary>
    /// the result of service execution
    /// </summary>
    public class Result
    {
        public string Error { get; set; }

        public object Data { get; set; }

        public Status Status { get; set; }

        public static Result Ok(object data)
        {
            return new Result()
            {
                Error = string.Empty,
                Data = data,
                Status = Status.Success
            };
        }

        public static Result Failed(string error)
        {
            return new Result()
            {
                Data = null,
                Error = error,
                Status = Status.Success
            };
        }
    }
}

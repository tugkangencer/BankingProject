using System;

namespace Core.Utilities.Results
{
    public class Result : IResult
    {
        public Result(bool success, string message) : this(success)
        {
            Message = message;
            ReferenceNumber = ulong.Parse(DateTime.Now.ToString("fffffff"));
        }

        public Result(bool success)
        {
            IsError = !success;
        }
        public bool IsError { get; }
        public string Message { get; }
        public int HttpCode { get; set; }
        public ulong ReferenceNumber { get; }
    }
}

namespace Core.Utilities.Results
{
    public interface IResult
    {
        bool IsError { get; }
        string Message { get; }
        int HttpCode { get; set; }
        ulong ReferenceNumber { get; }
    }
}

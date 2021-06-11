namespace Core.Utilities.Results
{
    public class DataResult<T> : Result, IDataResult<T>
    {
        public DataResult(T data, bool isError, string message) : base(isError, message)
        {
            Data = data;
        }

        public DataResult(T data, bool isError) : base(isError)
        {
            Data = data;
        }

        public T Data { get; }
    }
}

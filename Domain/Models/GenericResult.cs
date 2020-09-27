namespace Domain.Models
{
    public class GenericResult<T> : Result
    {
        public T Data { get; set; }
    }
}

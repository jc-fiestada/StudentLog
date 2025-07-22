namespace StudentLog.Models
{
    // use this whenever you are sending back response
    class ResponseAPI<T>
    {
        public required string Message { get; set; } // for console or notification
        public T? Data { get; set; }
    }
}
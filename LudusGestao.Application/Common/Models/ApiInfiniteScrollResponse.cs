namespace LudusGestao.Application.Common.Models
{
    public class ApiInfiniteScrollResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public string? NextCursor { get; set; }
        public bool HasMore { get; set; }

        public ApiInfiniteScrollResponse(T data, string? nextCursor, bool hasMore, string? message = null)
        {
            Success = true;
            Message = message;
            Data = data;
            NextCursor = nextCursor;
            HasMore = hasMore;
        }
    }
} 
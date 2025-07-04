namespace LudusGestao.Application.Common.Models
{
    public class ApiPagedResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

        public ApiPagedResponse(T data, int pageNumber, int pageSize, int totalCount, string message = null)
        {
            Success = true;
            Message = message;
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)System.Math.Ceiling(totalCount / (double)pageSize);
        }
    }
} 
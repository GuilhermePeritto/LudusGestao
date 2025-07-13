namespace LudusGestao.Application.Common.Models
{
    public class ApiPagedResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IEnumerable<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        public ApiPagedResponse(IEnumerable<T> items, int currentPage, int pageSize, int totalItems, string message = null)
        {
            Success = true;
            Message = message;
            Items = items;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalItems = totalItems;
            TotalPages = (int)System.Math.Ceiling(totalItems / (double)pageSize);
        }
    }
} 
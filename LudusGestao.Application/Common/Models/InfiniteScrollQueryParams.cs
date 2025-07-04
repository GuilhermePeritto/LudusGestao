namespace LudusGestao.Application.Common.Models
{
    public class InfiniteScrollQueryParams
    {
        public string? Cursor { get; set; }
        public int PageSize { get; set; } = 10;
    }
} 
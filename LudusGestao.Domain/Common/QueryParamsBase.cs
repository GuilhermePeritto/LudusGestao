namespace LudusGestao.Domain.Common
{
    public class QueryParamsBase
    {
        public string? Fields { get; set; }
        public int Page { get; set; } = 1;
        public int Start { get; set; } = 0;
        public int Limit { get; set; } = 10;
        public string? Sort { get; set; }
        public string? Filter { get; set; }
    }
} 
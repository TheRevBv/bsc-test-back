namespace BSC.Application.Commons.Bases.Request
{
    public class BasePaginationRequest
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
        public string Order { get; set; } = "desc";
        public string? Sort { get; set; } = null;
    }
}
using FluentValidation.Results;

namespace BSC.Application.Commons.Bases.Response
{
    public class BaseResponseAll<T> : BaseResponse<T>
    {
        public int Pages { get; set; }
        public int Page { get; set; }
        public int Total { get; set; }
        public int Limit { get; set; }
    }
}
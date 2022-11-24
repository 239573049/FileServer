namespace File.Application.Contract.Base
{
    public class PagedRequestInput
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public int SkipCount => (Page - 1) * MaxResultCount;
        
        public int MaxResultCount =>
            PageSize > 1000
                ? 1000
                : PageSize;
    }
}
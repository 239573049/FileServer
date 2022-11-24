using File.Application.Contract.Base;

namespace File.Application.Contract.Files.Input
{
    public class GetListInput : PagedRequestInput
    {
        /// <summary>
        /// 搜索名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        public GetListInput(string? name, string? path, int? page, int? pageSize)
        {
            Page = page ?? 1;
            PageSize = pageSize ?? 20;
            Name = name ?? string.Empty;
            Path = path ?? "/";
        }
    }
}

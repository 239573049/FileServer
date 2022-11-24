using System.Collections.Generic;

namespace File.Application.Contract.Base
{
    public class PagedResultDto<T>
    {
        public List<T> Items { get; set; }

        public int TotalCount { get; set; }

        public PagedResultDto()
        {
            Items = new List<T>();
        }

        public PagedResultDto(int totalCount, List<T> items)
        {
            TotalCount = totalCount;
            Items = items;
        }
    }
}
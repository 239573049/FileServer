using System.Collections.Generic;

namespace File.Application.Contract.Base
{
    public class PagedResultDto<T>
    {
        public IEnumerable<T> Items { get; set; }

        public int TotalCount { get; set; }

        public PagedResultDto()
        {
            Items = new List<T>();
        }

        public PagedResultDto(int totalCount, IEnumerable<T> items)
        {
            TotalCount = totalCount;
            Items = items;
        }
    }
}
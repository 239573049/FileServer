using File.Shared;
using System;

namespace File.Application.Contract.Files.Dto
{
    /// <summary>
    /// 文件列表dto
    /// </summary>
    public class FilesListDto
    {
        /// <summary>
        /// 类型
        /// </summary>
        public FileType Type { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        /// 文件图标
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string? FileType { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string? CreatedTime { get; set; }

        public string? FullName { get; set; }
    }
}

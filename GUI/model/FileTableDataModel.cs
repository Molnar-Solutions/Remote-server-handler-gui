using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.model
{
    public class FileTableDataModel
    {
        public string privilege { get; set; }
        public string owner { get; set; }
        public int sizeInBytes { get; set; }
        public string date { get; set; }
        public string fileName { get; set; }
    }

    public record struct GetFilesDto(
        string userName,
        string? excludedDirectories
    );

    public record struct CreateFileDto(
        string userName,
        string fileName
    );

    public record struct RemoveFileDto(
        string userName,
        string fileName
    );

    public record struct FileDownloadResponse(
        string type,
        byte[] data
    );
}

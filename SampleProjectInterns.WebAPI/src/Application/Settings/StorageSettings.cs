using System.ComponentModel.DataAnnotations;

namespace Application.Settings;
public class StorageSettings
{
    public string Type { get; set; } = "FileSystem";
    public int WriteBufferSize { get; init; } = 4096;

    public string? Path { get; set; }

    [Required] public bool UseWebRootPath { get; set; }

    [Required] public bool UseSsl { get; set; }
    [Required] public string FileApiRelativeUrl { get; set; }
}

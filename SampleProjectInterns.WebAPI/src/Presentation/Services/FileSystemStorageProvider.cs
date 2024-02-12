using Application.Interfaces;
using Application.Settings;
using Domain.Errors;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGeneration;

namespace Presentation.Services;

public class FileSystemStorageProvider : IStorageProvider
{
    private readonly StorageSettings _settings;
    private readonly IFileSystem _fileSystem;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FileSystemStorageProvider(IOptions<StorageSettings> settings,
        IFileSystem fileSystem, IHttpContextAccessor httpContextAccessor)
    {
        _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    private void CreateDirectoryIfNotExists(string path)
    {
        if (!_fileSystem.DirectoryExists(path))
        {
            _fileSystem.CreateDirectory(path);
        }
    }

    public async Task<long> Put(string path, Stream file, string extension, CancellationToken cancellationToken = default)
    {
        var filePath = GetPhysicalPath(path, extension);
        CreateDirectoryIfNotExists(Path.GetDirectoryName(filePath)!);
        await using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
        await file.CopyToAsync(fileStream, _settings.WriteBufferSize, cancellationToken);
        await fileStream.FlushAsync(cancellationToken);
        return fileStream.Length;
    }

    public Task<Stream> Get(string path, string extension, CancellationToken cancellationToken = default)
    {
        var filePath = GetPhysicalPath(path, extension);
        if (!_fileSystem.FileExists(filePath))
        {
            throw Errors.FileNotFound;
        }
        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

        return Task.FromResult(fileStream as Stream);
    }

    public Task Delete(IEnumerable<string> paths, CancellationToken cancellationToken = default)
    {
        foreach (var path in paths)
        {
            var filePath = GetPhysicalPath(path, string.Empty);
            var filePathWithExtension = GetFileWithExtensionByName(filePath);
            if (filePathWithExtension != null)
            {
                _fileSystem.DeleteFile(filePathWithExtension);
            }
        }

        return Task.CompletedTask;
    }

    public Task DeleteEmptyFolder(string path)
    {
        var folderPath = GetPhysicalPath(path, string.Empty);
        if (_fileSystem.DirectoryExists(folderPath))
        {
            _fileSystem.RemoveDirectory(folderPath, true);
        }
        return Task.CompletedTask;
    }

    public async Task Move(string sourcePath, string destinationPath, string extension, CancellationToken cancellationToken = default)
    {
        var fileStream = await Get(sourcePath, extension, cancellationToken);

        await Delete(new List<string> { $"{sourcePath}{extension}" }, cancellationToken);

        await Put(destinationPath, fileStream, extension, cancellationToken);
    }

    public Task Copy(string sourcePath, string destinationPath, string extension, CancellationToken cancellationToken = default)
    {
        File.Copy(GetPhysicalPath(sourcePath, extension),
            GetPhysicalPath(destinationPath, extension), true);

        return Task.CompletedTask;
    }

    public string GetPreSignedUrl(string path, string extension)
    {
        var scheme = _settings.UseSsl ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;

        return $"{scheme}://{_httpContextAccessor.HttpContext?.Request.Host}/{_settings.FileApiRelativeUrl}{path}{extension}";
    }

    public string GetObjectUrl(string path, string extension)
    {
        var scheme = _settings.UseSsl ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;

        return $"{scheme}://{_httpContextAccessor.HttpContext?.Request.Host}/{_settings.FileApiRelativeUrl}{path}{extension}";
    }

    private string GetPhysicalPath(string location, string extension)
    {
        return $"{_settings.Path}/{location}{extension}";
    }

    private string? GetFileWithExtensionByName(string filePath)
    {
        var directory = Path.GetDirectoryName(filePath)!;
        var fileName = Path.GetFileName(filePath);

        var files = _fileSystem.EnumerateFiles(directory, $"{fileName}.*", SearchOption.TopDirectoryOnly);
        var enumerable = files as string[] ?? files.ToArray();
        return enumerable.Any() ? enumerable.First() : null;
    }
}

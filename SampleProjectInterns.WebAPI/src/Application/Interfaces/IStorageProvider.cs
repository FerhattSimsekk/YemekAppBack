namespace Application.Interfaces;
public interface IStorageProvider
{
    Task<long> Put(string path, Stream file, string extension, CancellationToken cancellationToken = default);
    Task<Stream> Get(string path, string extension, CancellationToken cancellationToken = default);
    Task Delete(IEnumerable<string> paths, CancellationToken cancellationToken = default);
    Task DeleteEmptyFolder(string path);
    Task Move(string sourcePath, string destinationPath, string extension, CancellationToken cancellationToken = default);
    Task Copy(string sourcePath, string destinationPath, string extension, CancellationToken cancellationToken = default);
    string GetPreSignedUrl(string path, string extension);
    string GetObjectUrl(string path, string extension);
}

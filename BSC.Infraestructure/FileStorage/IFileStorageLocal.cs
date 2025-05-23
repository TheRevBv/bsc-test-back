namespace BSC.Infrastructure.FileStorage
{
    public interface IFileStorageLocal
    {
        Task<string> SaveFile(string container, string file, string webRootPath, string scheme, string host);
        Task<string> EditFile(string container, string file, string route, string webRootPath, string scheme, string host);
        Task RemoveFile(string route, string container, string webRootPath);
    }
}
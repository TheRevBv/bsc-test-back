using Microsoft.AspNetCore.Http;

namespace BSC.Application.Interfaces
{
        public interface IFileStorageLocalApplication
        {
            Task<string> SaveFile(string container, string file);
            Task<string> EditFile(string container, string file, string route);
            Task RemoveFile(string route, string container);
        }
}
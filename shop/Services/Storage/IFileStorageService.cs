namespace shop.Services.Storage
{
    public interface IFileStorageService
    {
        Task<bool> RemoveFileAsync(string fileName);
        Task<string> UploadFileAsync(IFormFile file);
    }
}

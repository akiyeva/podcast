using Microsoft.AspNetCore.Http;

namespace Podcast.B.Helpers;

public static class FileService
{
    public static bool CheckType(this IFormFile file, string type = "image")
    {
        return file.ContentType.Contains(type);
    }

    public static bool CheckSize(this IFormFile file, int mb)
    {
        return file.Length < mb * 1024 * 1024;
    }

    public static async Task<string> CreateImageAsync(this IFormFile file, string path)
    {
        string filename = Guid.NewGuid() + file.FileName;

        path = Path.Combine(path, filename);

        using (FileStream stream = new(path, FileMode.CreateNew))
        {
           await file.CopyToAsync(stream);
        }

        return filename;
    }

    public static bool DeleteFile(this string fileName,string path)
    {

        path= Path.Combine(path, fileName);
        if(File.Exists(path))
        {
            File.Delete(path);
            return true;
        }
        return false;
    }
}

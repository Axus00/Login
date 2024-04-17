using Login.Providers;

namespace Login.Helpers;

public class HelperUploadFiles
{
    private PathProviders _pathProviders;

    public HelperUploadFiles(PathProviders pathProviders)
    {
        _pathProviders = pathProviders;
    }
    
    public async Task<string> UploadFileAsync(IFormFile formFile, string nombreArchivo, Folders folder)
    {
        string path = _pathProviders.MapPath(nombreArchivo, folder);
        
        using (Stream stream = new FileStream(path, FileMode.Create))
        {
            await formFile.CopyToAsync(stream);
        }
        
        return path;
    }
}
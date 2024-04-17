namespace Login.Providers;

public enum Folders
{
    Images = 0, Documents = 1, Uploads = 2, Temp = 3
}
public class PathProviders
{
    private readonly IWebHostEnvironment _hostEnvironment;

    public PathProviders(IWebHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
    }

    public string MapPath(string fileName, Folders folder)
    {
        string carpeta = "";

        if (folder == Folders.Images)
        {
            carpeta = "images";
        }
        else if (folder == Folders.Documents)
        {
            carpeta = "documents";
        }
        else if (folder == Folders.Uploads)
        {
            carpeta = "uploads";
        }
        else if (folder == Folders.Temp)
        {
            carpeta = "temp";
        }
        string path = Path.Combine(_hostEnvironment.WebRootPath, carpeta, fileName);

        if (folder == Folders.Temp)
        {
            path = Path.Combine(Path.GetTempPath(), fileName);
        }

        return path;
    }
}
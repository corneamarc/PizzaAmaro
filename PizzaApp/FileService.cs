namespace PizzaApp;

public class FileService : IFileService
{
    public bool Exists(string path)
    {
        return File.Exists(path);
    }

    public string ReadAllText(string path)
    {
        return File.ReadAllText(path);
    }

    public void WriteAllText(string path, string contents)
    {
        try
        {
            // Asigurăm-ne că directorul există
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
                
            File.WriteAllText(path, contents);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to file: {ex.Message}");
        }
    }
}
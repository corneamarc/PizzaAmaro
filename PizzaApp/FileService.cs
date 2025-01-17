namespace PizzaApp;

/// <summary>
/// Implementare concretă a serviciului de fișiere
/// Gestionează toate operațiile de citire/scriere în fișiere
/// Asigură persistența datelor aplicației
/// </summary>
public class FileService : IFileService
{
    /// <summary>
    /// Verifică existența unui fișier
    /// </summary>
    public bool Exists(string path)
    {
        return File.Exists(path);
    }

    /// <summary>
    /// Citește conținutul unui fișier text
    /// </summary>
    public string ReadAllText(string path)
    {
        return File.ReadAllText(path);
    }

    /// <summary>
    /// Scrie conținut într-un fișier text
    /// Creează directorul dacă nu există
    /// Gestionează erorile de scriere
    /// </summary>
    public void WriteAllText(string path, string contents)
    {
        try
        {
            // Asigură existența directorului înainte de scriere
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
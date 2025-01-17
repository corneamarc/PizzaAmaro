namespace PizzaApp;

/// <summary>
/// Interfață pentru operațiile cu fișiere
/// Permite abstractizarea și mockarea operațiilor de I/O cu fișiere
/// Facilitează testarea și schimbarea implementării fără a modifica restul codului
/// </summary>
public interface IFileService
{
    /// <summary>
    /// Verifică dacă un fișier există la calea specificată
    /// </summary>
    bool Exists(string path);

    /// <summary>
    /// Citește tot conținutul unui fișier text
    /// </summary>
    string ReadAllText(string path);

    /// <summary>
    /// Scrie conținut text într-un fișier
    /// Suprascrie fișierul dacă există sau îl creează dacă nu există
    /// </summary>
    void WriteAllText(string path, string contents);
}
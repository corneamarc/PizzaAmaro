namespace PizzaApp;

/// <summary>
/// Interfață pentru operațiile de I/O cu consola
/// Permite mockarea și testarea operațiilor de consolă
/// </summary>
public interface IConsoleService
{
    /// <summary>
    /// Afișează un mesaj în consolă
    /// </summary>
    void WriteLine(string message);

    /// <summary>
    /// Citește o linie de text din consolă
    /// </summary>
    string ReadLine();

    /// <summary>
    /// Citește o parolă în mod securizat, afișând * pentru fiecare caracter
    /// </summary>
    string ReadPassword();
}
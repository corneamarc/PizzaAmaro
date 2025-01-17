namespace PizzaApp;

/// <summary>
/// Implementare concretă a serviciului de consolă
/// Gestionează interacțiunea cu utilizatorul prin intermediul consolei
/// </summary>
public class ConsoleService : IConsoleService
{
    /// <summary>
    /// Afișează un mesaj în consolă folosind Console.WriteLine
    /// </summary>
    public void WriteLine(string message)
    {
        Console.WriteLine(message);
    }

    /// <summary>
    /// Citește o linie de text din consolă folosind Console.ReadLine
    /// </summary>
    public string ReadLine()
    {
        return Console.ReadLine();
    }

    /// <summary>
    /// Implementează citirea securizată a parolei
    /// - Afișează * pentru fiecare caracter introdus
    /// - Permite ștergerea caracterelor cu Backspace
    /// - Ascunde parola în timpul introducerii
    /// </summary>
    public string ReadPassword()
    {
        string password = "";
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true); // true ascunde caracterul introdus

            // Adaugă caracterul la parolă dacă nu este Backspace sau Enter
            if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
            {
                password += key.KeyChar;
                Console.Write("*"); // Afișează * în loc de caracterul real
            }
            // Gestionează ștergerea caracterelor cu Backspace
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Remove(password.Length - 1);
                Console.Write("\b \b"); // Șterge ultimul * afișat
            }
        }
        while (key.Key != ConsoleKey.Enter); // Continuă până la apăsarea Enter

        Console.WriteLine();
        return password;
    }
}
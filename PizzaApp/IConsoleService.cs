namespace PizzaApp;

public interface IConsoleService
{
    void WriteLine(string message);
    string ReadLine();
    string ReadPassword();
}
namespace PizzaApp;

public interface IFileService
{
    bool Exists(string path);
    string ReadAllText(string path);
    void WriteAllText(string path, string contents);
}
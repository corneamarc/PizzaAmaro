namespace PizzaAmaro;

public class Utilizator
{
    public string Nume { get; set; }
    public string Parola { get; set; }
    public bool EsteAdministrator { get; set; }

    public Utilizator(string nume, string parola, bool esteAdministrator)
    {
        Nume = nume;
        Parola = parola;
        EsteAdministrator = esteAdministrator;
    }
}
namespace PizzaApp
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Clienti> clienti = new List<Clienti>();
            List<Comenzile> comenzi = new List<Comenzile>();

            while (true)
            {
                Console.WriteLine("Meniu:");
                Console.WriteLine("1. Adauga client");
                Console.WriteLine("2. Comanda pizza");
                Console.WriteLine("3. Iesi");
                Console.Write("Alege o optiune: ");
                string optiune = Console.ReadLine();

                switch (optiune)
                {
                    case "1":
                        AdaugaClient(clienti);
                        break;
                    case "2":
                        ComandaPizza(clienti, comenzi);
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Optiune invalida. Te rog sa incerci din nou.");
                        break;
                }
            }
        }

        static void AdaugaClient(List<Clienti> clienti)
        {
            Console.Write("Introdu numele clientului: ");
            string nume = Console.ReadLine();

            Console.Write("Introdu numarul de telefon (format: 07XXXXXXXX): ");
            string numarDeTelefon = Console.ReadLine();

            if (!Clienti.ValidareTelefon(numarDeTelefon))
            {
                Console.WriteLine("Numar de telefon invalid.");
                return;
            }

            clienti.Add(new Clienti(nume, numarDeTelefon, new List<Comenzile>()));
            Console.WriteLine("Client adaugat cu succes.");
        }

        static void ComandaPizza(List<Clienti> clienti, List<Comenzile> comenzi)
        {
            if (clienti.Count == 0)
            {
                Console.WriteLine("Nu exista clienti. Te rog sa adaugi un client mai intai.");
                return;
            }

            Console.WriteLine("Selecteaza un client:");
            for (int i = 0; i < clienti.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {clienti[i].Nume}");
            }

            int clientIndex = int.Parse(Console.ReadLine()) - 1;
            if (clientIndex < 0 || clientIndex >= clienti.Count)
            {
                Console.WriteLine("Client invalid.");
                return;
            }

            Clienti client = clienti[clientIndex];
            Comenzile comanda = new Comenzile(client, "Livrare");

            while (true)
            {
                Console.WriteLine("Alege tipul de pizza:");
                Console.WriteLine("1. Pizza Standard");
                Console.WriteLine("2. Pizza Personalizata");
                Console.WriteLine("3. Terminare comanda");
                string optiunePizza = Console.ReadLine();

                if (optiunePizza == "3")
                {
                    break;
                }

                Console.Write("Introdu numele pizzei: ");
                string numePizza = Console.ReadLine();

                Console.Write("Alege dimensiunea (1. Mica, 2. Medie, 3. Mare): ");
                Dimensiune dimensiune = (Dimensiune)(int.Parse(Console.ReadLine()) - 1);

                Pizza pizza;
                if (optiunePizza == "1")
                {
                    pizza = new PizzaStandard(numePizza, dimensiune, 0);
                }
                else if (optiunePizza == "2")
                {
                    pizza = new PizzaPersonalizata(numePizza, dimensiune);
                    Console.WriteLine("Adauga ingrediente (introduceti 'stop' pentru a termina):");
                    while (true)
                    {
                        Console.Write("Nume ingredient: ");
                        string numeIngredient = Console.ReadLine();
                        if (numeIngredient.ToLower() == "stop") break;

                        Console.Write("Cost ingredient: ");
                        decimal costIngredient = decimal.Parse(Console.ReadLine());

                        pizza.AdaugaIngrediente(new Ingrediente(numeIngredient, costIngredient));
                    }
                }
                else
                {
                    Console.WriteLine("Optiune invalida.");
                    continue;
                }

                comanda.AfisarePizza(pizza);
            }

            comanda.CalculeazaCostTotal();
            comenzi.Add(comanda);
            client.Comenzi.Add(comanda);
            Console.WriteLine("Comanda a fost plasata cu succes.");
        }
        
    }
}        
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PizzaConsole.Exports;
using PizzaConsole.Interface;

namespace PizzaConsole;

public class BillFormat
{
    public static void BillXml(List<(int quantity, Pizza pizza)> pizzas)
    {
        Visitor visitor = new XmlVisitor();
        String command = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<command>";
                
        foreach (var pizzaElement in pizzas)
        {
            command += $"\n<pizza>\n<quantity>{pizzaElement.quantity}</quantity>{pizzaElement.pizza.Accept(visitor)}<supplement>{pizzaElement.pizza.GetCost()}</supplement>\n</pizza>";
        }
        command += "\n</command>";
        Console.WriteLine(command);
        Console.WriteLine("Souhaitez vous l'enregistrer ?");
        Console.Write("Oui (o), Non (n): ");
        var choiceRegistering = Console.ReadLine();
        if (choiceRegistering == "o")
        {
            File.WriteAllText("bill.xml", command);
            Console.WriteLine("Commande enregistrée dans le fichier bill.xml");
        }
    }

    public static void BillJson(List<(int quantity, Pizza pizza)> pizzas)
    {
        Visitor visitor = new JsonVisitor();
        String command2 = "{\n\"command\": [";
        foreach (var pizzaElement in pizzas)
        {
            command2 += $"\n{{\n\"quantity\":{pizzaElement.quantity},{pizzaElement.pizza.Accept(visitor)}, \"supplement\": {pizzaElement.pizza.GetCost().ToString(CultureInfo.InvariantCulture)}}},";
        }
        command2 = command2.Remove(command2.Length - 1);
        command2 += "\n]\n}";
        String jsonFormatted = JToken.Parse(command2).ToString(Formatting.Indented);
        Console.WriteLine(jsonFormatted);
        Console.WriteLine("Souhaitez vous l'enregistrer ?");
        Console.Write("Oui (o), Non (n): ");
        var choiceRegistering2 = Console.ReadLine();
        if (choiceRegistering2 == "o")
        {
            File.WriteAllText("bill.json", jsonFormatted);
            Console.WriteLine("Commande enregistrée dans le fichier bill.json");
        }
    }
    
    
}
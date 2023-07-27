using System.Globalization;
using PizzaConsole;
using PizzaConsole.Exports;
using PizzaConsole.Interface;
using PizzaConsole.Parser;

class Program
{
    private static void Main(string[] args) {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        var pizza1 = new Pizza("Regina", 8m, new List<Ingredient>() {
            new("tomate", new Quantity(150, "g")),
            new("mozzarella", new Quantity(125, "g")),
            new("fromage râpé", new Quantity(100, "g")),
            new("jambon", new Quantity(2, "tranches")),
            new("champignons frais", new Quantity(4, null)),
            new("huile d’olive", new Quantity(2, "cuillères à soupe"))
        });
        var pizza2 = new Pizza("4 Saisons", 9m, new List<Ingredient>() {
            new("tomate", new Quantity(150, "g")),
            new("mozzarella", new Quantity(125, "g")),
            new("jambon", new Quantity(2, "tranches")),
            new("champignons frais", new Quantity(100, "g")),
            new("poivron", new Quantity(0.5m, null)),
            new("olives", new Quantity(1, "poignée"))
        });
        var pizza3 = new Pizza("Végétarienne", 7.5m, new List<Ingredient>() {
            new("tomate", new Quantity(150, "g")),
            new("mozzarella", new Quantity(100, "g")),
            new("courgette", new Quantity(0.5m, null)),
            new("poivron jaune", new Quantity(1, null)),
            new("tomates cerises", new Quantity(6, null)),
            new("olives", new Quantity(null, "quelques"))
        });

        var availablePizzas = new List<Pizza>() { pizza1, pizza2, pizza3 };
        
        Loop(availablePizzas);
    }


    private static List<Pizza> parseFile(String value)
    {
        Parser parser;
        if (value.Contains(".json"))
        {
            parser = new JsonParser();
        }
        else if (value.Contains(".xml"))
        {
            parser = new XmlParser();
        }
        else
        {
            Console.WriteLine("Le fichier n'est pas au bon format");
            return new List<Pizza>();
        }
        parser.readFile(value);
        return parser.parse();
    }
    
    private static void Loop(List<Pizza> availablePizzas) {
        while (true) {
            Console.WriteLine();
            Console.Write("Entrez une commande (votre commande doit être sous la forme : \"6 Pizza1, 2 Pizza2, {quantité} {nom}\", ou un nom de fichier : \"Regina.json\") : ");
            var order = Console.ReadLine()?.Split(',');

            if (order == null) continue;
            var pizzas = new List<(int quantity, Pizza pizza)>();
            if (order[0].Contains("."))
            {
                var parsefile = parseFile(order[0]);
                if (parsefile.Count == 0)
                {
                    continue;
                }
                foreach (var pizza in parsefile)
                {
                    pizzas.Add((1, pizza));
                }
                HandleChoices(pizzas);
                continue;
            }
            var error = false;
            
            foreach (var input in order) {
                try {
                    var (quantity, pizza) = ParsePizzaInput(input, availablePizzas);
                    
                    var pizzaInList = pizzas.Find(p => p.pizza.Name == pizza.Name);
                    
                    if (pizzaInList != default) {
                        pizzas.Remove(pizzaInList);
                        pizzas.Add((pizzaInList.quantity + quantity, pizza));
                    } else {
                        pizzas.Add((quantity, pizza));
                    }
                } catch (Exception e) {
                    error = true;
                    Console.WriteLine();
                    Console.WriteLine(e.Message);
                    break;
                }
            }
            
            if (error) continue;
            
            HandleChoices(pizzas);
        }
    }

    private static (int quantity, Pizza pizza) ParsePizzaInput(string pizza, List<Pizza> availablePizzas) {
        var pizzaOrder = pizza.Trim();
        var elements = pizzaOrder.Split(' ', 2);

        if (elements.Length != 2 || !int.TryParse(elements[0], out var quantity)) {
            throw new ArgumentException($"Impossible de traiter la commande \"{pizzaOrder}\"");
        }

        var pizzaName = elements[1];
        var pizzaToOrder = availablePizzas.Find(p =>
            string.Equals(p.Name, pizzaName, StringComparison.CurrentCultureIgnoreCase));

        if (pizzaToOrder != null) return (quantity, pizzaToOrder);
        
        throw new ArgumentException($"La pizza \"{pizzaName}\" n'existe pas");
    }

    private static List<Ingredient> getIngredientsFromPizzas(List<(int quantity, Pizza pizza)> pizzas)
    {
        var ingredients = new List<Ingredient>();
        
        foreach (var (pizzaQuantity, pizzaItem) in pizzas) {
            foreach (var ingredient in pizzaItem.Ingredients) {
                var ingredientInList = ingredients.Find(i => i.Name == ingredient.Name);
                            
                if (ingredientInList != default) {
                    ingredients.Remove(ingredientInList);
                    ingredients.Add(new Ingredient(ingredient.Name, new Quantity(ingredientInList.Quantity.Number + ingredient.Quantity.Number * pizzaQuantity, ingredient.Quantity.Unit)));
                } else {
                    ingredients.Add(new Ingredient(ingredient.Name, new Quantity(ingredient.Quantity.Number * pizzaQuantity, ingredient.Quantity.Unit)));
                }
            }
        }
        
        return ingredients;
    }

    private static void GetBillFormat(String choice, List<(int quantity, Pizza pizza)> pizzas)
    {
        switch (choice)
        {
            case "2":
                Visitor visitor = new XmlVisitor();
                String command = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<command>";
                
                foreach (var pizzaElement in pizzas)
                {
                    command += $"\n<pizza>\n<quantity>{pizzaElement.quantity}</quantity>{pizzaElement.pizza.Accept(visitor)}</pizza>";
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
                break;
            case "3":
                Visitor visitor2 = new JsonVisitor();
                String command2 = "{\n\"command\": [";
                foreach (var pizzaElement in pizzas)
                {
                    command2 += $"\n{{\n\"quantity\":\"{pizzaElement.quantity}\",{pizzaElement.pizza.Accept(visitor2)}}},";
                }
                command2 = command2.Remove(command2.Length - 1);
                command2 += "\n]\n}}";
                Console.WriteLine(command2);
                Console.WriteLine("Souhaitez vous l'enregistrer ?");
                Console.Write("Oui (o), Non (n): ");
                var choiceRegistering2 = Console.ReadLine();
                if (choiceRegistering2 == "o")
                {
                    File.WriteAllText("bill.json", command2);
                    Console.WriteLine("Commande enregistrée dans le fichier bill.json");
                }
                break;
            default:
                Console.WriteLine("Choix non compris, veuillez réessayer");
                break;
        }
    }
    
    private static void HandleChoices(List<(int quantity, Pizza pizza)> pizzas) {
        while (true) {
            Console.WriteLine();
            Console.WriteLine("1 : Afficher la facture");
            Console.WriteLine("2 : Afficher les recettes");
            Console.WriteLine("3 : Afficher les ingrédients utilisés");
            Console.WriteLine("4 : Nouvelle commande");
            Console.WriteLine();
            Console.Write("Entrez votre choix : ");
            var choice = Console.ReadLine();
                    
            switch (choice) {
                case "1": {
                    Console.WriteLine("Vous souhaitez afficher la facture sous quel format ?");
                    Console.WriteLine("1. Console");
                    Console.WriteLine("2. XML");
                    Console.WriteLine("3. JSON");
                    Console.Write("Entrez votre choix: ");
                    var choiceBill = Console.ReadLine();

                    if (choiceBill != "1" && choiceBill != null)
                    {
                        GetBillFormat(choiceBill, pizzas);
                        break;
                    }
                    
                    var total = 0m;
                    foreach (var (pizzaQuantity, pizzaItem) in pizzas) {
                        Console.WriteLine();
                        total += pizzaQuantity * pizzaItem.Price;
                        pizzaItem.DisplayBill(pizzaQuantity);
                    }
                    
                    Console.WriteLine();
                    Console.WriteLine($"Prix total : {total:0.00}€");
                    
                    break;
                }
                case "2": {
                    Console.WriteLine();
                    
                    foreach (var (_, pizzaItem) in pizzas) {
                        Console.WriteLine();
                        pizzaItem.DisplayRecipe();
                    }

                    break;
                }
                case "3":
                    Console.WriteLine();
                    
                    var ingredients = getIngredientsFromPizzas(pizzas);
                    
                    foreach (var ingredient in ingredients) {
                        ingredient.Display();
                        
                        foreach (var (pizzaQuantity, pizzaItem) in pizzas) {
                            var ingredientInPizza = pizzaItem.Ingredients.Find(i => i.Name == ingredient.Name);
                            
                            if (ingredientInPizza != default) {
                                Console.WriteLine($" - {pizzaItem.Name} : {ingredientInPizza.Quantity.Number * pizzaQuantity} {ingredientInPizza.Quantity.Unit}");
                            }
                        }
                    }
                    
                    break;
                case "4":
                    return;
            }
        }
    }
}
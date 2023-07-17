using PizzaConsole;

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

    private static void Loop(List<Pizza> availablePizzas) {
        while (true) {
            Console.WriteLine();
            Console.Write("Entrez une commande (votre commande doit être sous la forme : \"6 Pizza1, 2 Pizza2, {quantité} {nom}\") : ");
            var order = Console.ReadLine()?.Split(',');

            if (order == null) continue;
            
            var pizzas = new List<(int quantity, Pizza pizza)>();
            
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

    private static void HandleChoices(List<(int quantity, Pizza pizza)> pizzas) {
        while (true) {
            Console.WriteLine();
            Console.WriteLine("1 : Afficher la facture");
            Console.WriteLine("2 : Afficher les recettes");
            Console.WriteLine("3 : Nouvelle commande");
            Console.WriteLine();
            Console.Write("Entrez votre choix : ");
            var choice = Console.ReadLine();
                    
            switch (choice) {
                case "1": {
                    Console.WriteLine();
                    
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
                    return;
            }
        }
    }
}
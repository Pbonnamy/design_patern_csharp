using PizzaConsole.Interface;

namespace PizzaConsole;

public class Pizza: IElement
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public List<Ingredient> Ingredients { get; set; }
    
    public Pizza(string name, decimal price, List<Ingredient> ingredients)
    {
        Name = name;
        Price = price;
        Ingredients = ingredients;
    }
    
    public void DisplayBill(int quantity)
    {
        Console.WriteLine($"{quantity} {Name} : {quantity} * {Price:0.00}€");
        foreach (var ingredient in Ingredients) {
            Console.WriteLine($"{ingredient.CapitalizedName()} {ingredient.Quantity.Number} {ingredient.Quantity.Unit}");
        }
    }
    
    public void DisplayRecipe()
    {
        Console.WriteLine(Name);
        Console.WriteLine("Préparer la pâte");
        foreach (var ingredient in Ingredients) {
            Console.WriteLine($"Ajouter {ingredient.Name}");
        }
        Console.WriteLine("Cuire la pizza");
    }

    public String Accept(Visitor visitor)
    {
        return visitor.visit(this);
    }
}
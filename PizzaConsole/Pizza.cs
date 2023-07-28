using PizzaConsole.Interface;
using PizzaConsole.Parser;

namespace PizzaConsole;

public class Pizza: IElement, Composite
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public List<Ingredient> Ingredients { get; set; }
    
    public Pizza(){}
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

    public void Accept(VisitorParser visitor, Pizza value)
    {
        visitor.visit(this, value);
    }

    public void Accept(VisitorParser visitor, Ingredient value)
    {
        throw new NotImplementedException();
    }


    public double GetCost()
    {
        double cost = 0;
        foreach (var ingredient in Ingredients)
        {
            cost += ingredient.Cost;
        }

        return cost;
    }

    public string GetDescription()
    {
        string description = "My Pizza : ";
        foreach (var ingredient in Ingredients)
        {
            description += ingredient.GetDescription() + ", ";
        }
        description = description.TrimEnd(',', ' ');
        return description;
    }
}
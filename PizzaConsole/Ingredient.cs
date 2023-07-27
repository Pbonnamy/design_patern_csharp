using Newtonsoft.Json;
using PizzaConsole.Interface;

namespace PizzaConsole;

public class Ingredient: IElement, Composite
{
    public string Name { get; set; }
    public Quantity Quantity { get; set; }
    public double Cost { get; set; }
    
    [JsonConstructor]
    public Ingredient(string name, Quantity quantity)
    {
        Name = name;
        Quantity = quantity;
    }
    
    public Ingredient(string name, Quantity quantity, double cost)
    {
        Name = name;
        Quantity = quantity;
        Cost = cost;
    }
    
    public void Display()
    {
        Console.WriteLine($"{CapitalizedName()} : {Quantity.Number} {Quantity.Unit}");
    }
    
    public string CapitalizedName()
    {
        return char.ToUpper(Name[0]) + Name[1..];
    }

    public string Accept(Visitor visitor)
    {
        return visitor.visit(this);
    }

    public double GetCost()
    {
        return Cost;
    }

    public string GetDescription()
    {
        return Name;
    }
}
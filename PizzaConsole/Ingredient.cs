namespace PizzaConsole;

public class Ingredient
{
    public string Name { get; set; }
    public Quantity Quantity { get; set; }
    
    public Ingredient(string name, Quantity quantity)
    {
        Name = name;
        Quantity = quantity;
    }
    
    public void Display()
    {
        Console.WriteLine($"{CapitalizedName()} : {Quantity.Number} {Quantity.Unit}");
    }
    
    public string CapitalizedName()
    {
        return char.ToUpper(Name[0]) + Name[1..];
    }
}
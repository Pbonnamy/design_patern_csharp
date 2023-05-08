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
}
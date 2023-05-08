namespace PizzaConsole;

public class Quantity
{
    public decimal? Number { get; set; }
    public string? Unit { get; set; }
    
    public Quantity(decimal? number, string? unit)
    {
        Number = number;
        Unit = unit;
    }
}
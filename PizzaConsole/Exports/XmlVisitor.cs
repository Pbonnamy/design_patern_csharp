using System.Globalization;

namespace PizzaConsole.Exports;

public class XmlVisitor: Visitor
{
    public String visit(Pizza pizza)
    {
        String ingredients = "<ingredients>";

        foreach (var ingredient in pizza.Ingredients)
        {
            ingredients += $"\n<ingredient>\n<name>{ingredient.Name}</name>\n<quantity>{ingredient.Quantity.Number}</quantity>\n<unit>{ingredient.Quantity.Unit}</unit>\n</ingredient>";
        }

        ingredients += "</ingredients>";
        return $"\n<name>{pizza.Name}</name>\n{ingredients}<price>{pizza.Price.ToString(CultureInfo.CurrentCulture)}</price>\n";
    }
    
}
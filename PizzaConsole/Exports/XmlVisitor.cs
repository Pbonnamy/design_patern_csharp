using System.Globalization;
using PizzaConsole.Interface;

namespace PizzaConsole.Exports;

public class XmlVisitor: Visitor
{
    public String visit(Pizza pizza)
    {
        String ingredients = "";

        foreach (var ingredient in pizza.Ingredients)
        {
            ingredients += ingredient.Accept(this);
        }
        
        return $"\n<name>{pizza.Name}</name>\n<ingredients>{ingredients}</ingredients>\n<price>{pizza.Price.ToString(CultureInfo.CurrentCulture)}</price>\n";
    }

    public string visit(Ingredient ingredient)
    {
        return $"\n<ingredient>\n<name>{ingredient.Name}</name>\n<quantity>{ingredient.Quantity.Number}</quantity>\n<unit>{ingredient.Quantity.Unit}</unit>\n</ingredient>";
    }
}
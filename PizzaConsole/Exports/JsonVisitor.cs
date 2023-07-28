using System.Globalization;
using PizzaConsole.Interface;

namespace PizzaConsole.Exports;

public class JsonVisitor: Visitor{
    
    public String visit(Pizza pizza)
    {
        String ingredients = "";

        foreach (var ingredient in pizza.Ingredients)
        {
            ingredients += ingredient.Accept(this) + ",";
        }
        return $"\n\"name\":\"{pizza.Name}\",\n\"ingredients\":[{ingredients}],\n\"price\":{pizza.Price.ToString(CultureInfo.InvariantCulture)}\n";
    }

    public string visit(Ingredient ingredient)
    {
        return $"{{\n\"name\":\"{ingredient.Name}\",\n\"quantity\":{ingredient.Quantity.Number.Value.ToString(CultureInfo.InvariantCulture)},\n\"unit\":\"{ingredient.Quantity.Unit}\"\n}}";
    }
}
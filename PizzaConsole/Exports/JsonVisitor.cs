using System.Globalization;

namespace PizzaConsole.Exports;

public class JsonVisitor: Visitor{
    
    public String visit(Pizza pizza)
    {
        String ingredients = "";

        foreach (var ingredient in pizza.Ingredients)
        {
            ingredients += ingredient.Accept(this);
        }
        ingredients = ingredients.Remove(ingredients.Length - 1);
        return $"\n\"name\":\"{pizza.Name}\",\n\"ingredients\":[{ingredients}],\n\"price\":\"{pizza.Price.ToString(CultureInfo.CurrentCulture)}\"\n";
    }

    public string visit(Ingredient ingredient)
    {
        return $"{{\n\"name\":\"{ingredient.Name}\",\n\"quantity\":\"{ingredient.Quantity.Number}\",\n\"unit\":\"{ingredient.Quantity.Unit}\"\n}},";
    }
}
using Newtonsoft.Json;

namespace PizzaConsole.Parser;

public class JsonVisitorParser: VisitorParser
{
    private string _path;

    public void visit(Pizza pizza, Pizza pizzaData)
    {
        pizza.Name = pizzaData.Name;
        pizza.Price = pizzaData.Price;
        pizza.Ingredients = pizzaData.Ingredients;
    }

    public void visit(Ingredient ingredient)
    {
        throw new NotImplementedException();
    }
}
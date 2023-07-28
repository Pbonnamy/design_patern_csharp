using System.Xml;

namespace PizzaConsole.Parser;

public interface VisitorParser
{
    void visit(Pizza pizza, Pizza pizzaData);

    void visit(Ingredient ingredient);
}
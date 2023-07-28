using PizzaConsole.Interface;
using PizzaConsole.Parser;

namespace PizzaConsole;

interface IElement
{
    String Accept(Visitor visitor);

    void Accept(VisitorParser visitor, Pizza value);
    void Accept(VisitorParser visitor, Ingredient value);

}
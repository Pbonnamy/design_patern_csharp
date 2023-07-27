using PizzaConsole.Interface;

namespace PizzaConsole;

interface IElement
{
    String Accept(Visitor visitor);
}
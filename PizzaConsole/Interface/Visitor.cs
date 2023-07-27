namespace PizzaConsole.Interface;

public interface Visitor
{
    String visit(Pizza pizza);
    String visit(Ingredient ingredient);
    
}
namespace PizzaConsole.Parser;

public interface Parser
{
    void readFile(string path);
    List<Pizza> parse();
}
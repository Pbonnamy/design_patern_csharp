using Newtonsoft.Json;

namespace PizzaConsole.Parser;

public class JsonParser: Parser
{
    
    private String _path;
    
    public void readFile(string path)
    {
        _path = path;
    }

    public List<Pizza> parse()
    {
        List<Pizza> pizzas = new List<Pizza>();
        String absolutePath = Path.GetFullPath(_path);
        using (StreamReader r = new StreamReader(absolutePath))
        {
            string json = r.ReadToEnd();
            pizzas = JsonConvert.DeserializeObject<List<Pizza>>(json) ?? new List<Pizza>();
        }
        return pizzas;
    }
}
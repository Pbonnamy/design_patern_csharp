using System.Xml;

namespace PizzaConsole.Parser;

public class XmlParser: Parser
{
    private String _path;
    
    public void readFile(string path)
    {
        _path = path;
    }

    public List<Pizza> parse()
    {
        XmlDocument xmlDocument = new XmlDocument();
        List<Pizza> pizzas = new List<Pizza>();
        using (StreamReader r = new StreamReader(_path))
        {
            xmlDocument.Load(r);
            XmlNodeList xmlNodeList = xmlDocument.GetElementsByTagName("pizza");
            foreach (var variable in xmlNodeList)
            {
                XmlNode xmlNode = (XmlNode) variable;
                string name = xmlNode.Attributes["name"].Value;
                decimal price = decimal.Parse(xmlNode.Attributes["price"].Value);
                List<Ingredient> ingredients = new List<Ingredient>();
                foreach (XmlNode ingredientNode in xmlNode.ChildNodes)
                {
                    string ingredientName = ingredientNode.Attributes["name"].Value;
                    decimal ingredientQuantity = decimal.Parse(ingredientNode.Attributes["quantity"].Value);
                    string ingredientUnit = ingredientNode.Attributes["unit"].Value;
                    ingredients.Add(new Ingredient(ingredientName, new Quantity(ingredientQuantity, ingredientUnit)));
                }
                pizzas.Add(new Pizza(name, price, ingredients));
            }
            // pizzas = JsonConvert.DeserializeObject<List<Pizza>>(xml) ?? new List<Pizza>();
        }
        return pizzas;
    }
}
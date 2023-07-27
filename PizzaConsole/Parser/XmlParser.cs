using System.Xml;

namespace PizzaConsole.Parser;

public class XmlParser: Parser
{
    private String? _path;
    
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
            XmlNodeList xmlNodeList = xmlDocument.GetElementsByTagName("Pizza");
            foreach (XmlNode pizzaNode in xmlNodeList)
            {
                try
                {
                    var name = "";
                    decimal price = 0m;
                    List<Ingredient> ingredients = new List<Ingredient>();
                    foreach (XmlElement childNode in pizzaNode.ChildNodes)
                    {
                        if (childNode.Name == "Name")
                        {
                            name += childNode.InnerText;
                        }
                        else if (childNode.Name == "Price")
                        {
                            price = decimal.Parse(childNode.InnerText.Replace('.', ','));
                        }
                        else if (childNode.Name == "Ingredients")
                        {
                            foreach (XmlElement ingredientNode in childNode.ChildNodes)
                            {
                                String ingredientName = "";
                                String ingredientQuantity = "";
                                String ingredientUnit = "";
                                foreach (XmlElement ingredientChildNode in ingredientNode.ChildNodes)
                                {
                                    if (ingredientChildNode.Name == "Name")
                                    {
                                        ingredientName = ingredientChildNode.InnerText;
                                    }
                                    else if (ingredientChildNode.Name == "Quantity")
                                    {
                                        foreach (var quantityNode in ingredientChildNode.ChildNodes)
                                        {
                                            if (quantityNode is XmlElement quantityElement)
                                            {
                                                if (quantityElement.Name == "Number")
                                                {
                                                    ingredientQuantity = quantityElement.InnerText;
                                                }
                                                else if (quantityElement.Name == "Unit")
                                                {
                                                    ingredientUnit = quantityElement.InnerText;
                                                }
                                            }
                                            else if (quantityNode is XmlText quantityText)
                                            {
                                                throw new Exception("Impossible de parser le fichier XML");
                                            }
                                        }
                                        
                                    }
                                }
                                ingredients.Add(
                                    new Ingredient(ingredientName, new Quantity(decimal.Parse(ingredientQuantity.Replace('.', ',')), ingredientUnit)));
                            }
                        }
                    }

                    pizzas.Add(new Pizza(name, price, ingredients));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Impossible de parser le fichier XML");
                }
                finally
                {
                    Console.WriteLine("Fichier XML parsé");
                }
            }
            // pizzas = JsonConvert.DeserializeObject<List<Pizza>>(xml) ?? new List<Pizza>();
        }
        return pizzas;
    }
}
using System.Xml;
using Newtonsoft.Json;
using PizzaConsole.Interface;
using PizzaConsole.Parser;

namespace PizzaConsole;

public class ParserClass
{

    public static List<Pizza> ParseJson(string jsonData)
    {
        var parser = new JsonVisitorParser();
        var pizzas = new List<Pizza>();
        try
        {
            using (StreamReader r = new StreamReader(jsonData))
            {
                string json = r.ReadToEnd();
                List<Pizza> pizzaJson = JsonConvert.DeserializeObject<List<Pizza>>(json) ?? new List<Pizza>();
                if (pizzaJson.Count != 0)
                {
                    foreach (var pizzaData in pizzaJson)
                    {
                        Pizza pizza = new Pizza();
                        pizza.Accept(parser, pizzaData);
                        pizzas.Add(pizza);
                    }
                }
                else
                {
                    pizzas = new List<Pizza>();
                }
                
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Impossible de parser le fichier JSON");
        }
        return pizzas;
    }

    public static List<Pizza> ParseXml(string xmlData)
    {
        var xmlParser = new XmlVisitorParser();
        var pizzas = new List<Pizza>();

        // Parser chaque objet XML dans la liste et remplir les propriétés de la pizza
        try
        {
            XmlDocument doc = new XmlDocument();
            using (StreamReader reader = new StreamReader(xmlData))
            {
                doc.Load(reader);
                XmlNodeList pizzaNodes = doc.SelectNodes("//Pizzas/Pizza");
                foreach (XmlNode pizzaNode in pizzaNodes)
                {
                    string name = pizzaNode.SelectSingleNode("Name").InnerText;
                    decimal price = decimal.Parse(pizzaNode.SelectSingleNode("Price").InnerText.Replace('.', ','));
                            
                    List<Ingredient> ingredients = new List<Ingredient>();
                    XmlNodeList ingredientNodes = pizzaNode.SelectNodes("Ingredients/Ingredient");
                    foreach (XmlNode ingredientNode in ingredientNodes)
                    {
                        string ingredientName = ingredientNode.SelectSingleNode("Name").InnerText;
                        decimal ingredientQuantityNumber = decimal.Parse(ingredientNode.SelectSingleNode("Quantity/Number").InnerText.Replace('.', ','));
                        string ingredientQuantityUnit = ingredientNode.SelectSingleNode("Quantity/Unit").InnerText;
                        ingredients.Add(new Ingredient(ingredientName, new Quantity(ingredientQuantityNumber, ingredientQuantityUnit)));
                    }
                            
                    var pizza = new Pizza();
                    pizza.Accept(xmlParser, new Pizza(name, price, ingredients));
                    pizzas.Add(pizza);
                }
            };

        }catch(Exception e)
        {
            Console.WriteLine(e);
            Console.WriteLine("Impossible de parser le fichier XML");
        }
        return pizzas;
    }
}
/*
 * Name: [Logan Brooks]
 * South Hills Username: [lbrooks81]
 */

using ConsoleTables;
using System.ComponentModel;
using static PizzaHAL.Item;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace PizzaHAL
{   
    /*TODO things to add: 
     * File that stores orders
     * Fix casing issues
     * Center properly
     * Customize items at checkout
     * Add SOUP OF THE DAY!!! next to the soup of the day
     * Press Q at any point during ordering to go back to main menu
     * Allow user to input multiple toppings
     * Separate classes
     */
    public class Program
    {
        private static char[] ValidAnswers = { 'y', 'n', 'Y', 'N' };
        
        public enum Categories
        {
            [Display(Name = "Pizzas")] pizzas,
            [Display(Name = "Drinks")] drinks,
            [Display(Name = "Sandwiches")] sandwiches,
            [Display(Name = "Soups")] soups,
            [Display(Name = "Desserts")] desserts,
            [Display(Name = "Checkout")] checkout
        }
        
        private static List<(Object, Double)> Items = [];

       
        public static double DELIVERY_FEE = 8.00;
        public static double SALES_TAX_RATE = 0.06;

        private static double total = 0;
        public static void Main()
        {
            Console.Title = "PizzaHAL";
            PrintMenu();
            int categorySelection = SelectCategory();
            while (categorySelection == 0)
            {
                PrintMenu();
                categorySelection = SelectCategory();
            }
            bool delivery = Checkout();
            bool orderUnfinished = PrintReceipt(delivery);
            while (orderUnfinished)
            {
                RemoveItemsFromCart();
                orderUnfinished = PrintReceipt(delivery);
            }

        }

        public static void PrintMenu()
        {
            Console.WriteLine(Item.bar);
            CenterText(Categories.pizzas.GetDisplayName());

            Console.WriteLine(Item.bar);
            CenterText(Categories.drinks.GetDisplayName());

            Console.WriteLine(Item.bar);
            CenterText(Categories.sandwiches.GetDisplayName());

            Console.WriteLine(Item.bar);
            CenterText(Categories.soups.GetDisplayName());

            Console.WriteLine(Item.bar);
            CenterText(Categories.desserts.GetDisplayName());

            Console.WriteLine(Item.bar);
            CenterText(Categories.checkout.GetDisplayName());
            Console.WriteLine(Item.bar);
        }
        public static int SelectCategory()
        {
            CenterTextNoNewLine("Enter category name or type \"Checkout\": ");
            String input = Console.ReadLine();
            if (input.Equals(Categories.pizzas.GetDisplayName(), StringComparison.OrdinalIgnoreCase))
            {
                Items.Add(Item.Pizza.OrderPizza(ref total));
                return 0;
            }
            if (input.Equals(Categories.drinks.GetDisplayName(), StringComparison.OrdinalIgnoreCase))
            {
                Items.Add(Item.Drink.OrderDrink(ref total));
                return 0;
            }
            if (input.Equals(Categories.desserts.GetDisplayName(), StringComparison.OrdinalIgnoreCase))
            {
                Items.Add(Item.Dessert.OrderDessert(ref total));
                return 0;
            }
            if (input.Equals(Categories.sandwiches.GetDisplayName(), StringComparison.OrdinalIgnoreCase))
            {
                Items.Add(Item.Sandwich.OrderSandwich(ref total));
                return 0;
            }
            if (input.Equals(Categories.soups.GetDisplayName(), StringComparison.OrdinalIgnoreCase))
            {
                Items.Add(Item.Soup.OrderSoup(ref total));
                return 0;
            }
            if (input.Equals(Categories.checkout.GetDisplayName(), StringComparison.OrdinalIgnoreCase)) return 1;
            CenterText("Invalid Input.");
            return 0;
        }        
        public static bool Checkout() //Returns true if delivery was added
        {
            Console.Clear();
            if (Items.Count != 0)
            {
                CenterTextNoNewLine($"Would you like to order delivery for ${DELIVERY_FEE}.00? (y/n): ");

                Char input = Console.ReadKey().KeyChar;
                Console.WriteLine(Environment.NewLine);

                while (!ValidAnswers.Contains(input))
                {
                    CenterText("Invalid Input.");
                    input = Console.ReadKey().KeyChar;
                    Console.WriteLine(Environment.NewLine);
                }
                if (char.ToLower(input).Equals('y'))
                {
                    CenterText("Delivery Added.");
                    Console.WriteLine(Environment.NewLine);
                    total += DELIVERY_FEE;
                    return true;
                }
            }
            return false;
        }
        public static bool PrintReceipt(bool delivery)
        {
            Console.WriteLine(bar);
            int counter = 1;
            foreach ((Object item, double price) in Items) 
            {
                if (item is Pizza)
                {
                    Pizza pizza = item as Pizza;
                    CenterText(counter + ". " + pizza.ToString(), "$" + price.ToString());
                    counter++;
                }
                if (item is Drink)
                {
                    Drink drink = item as Drink;
                    CenterText(counter + ". " + drink.ToString(), "$" + price.ToString());
                    counter++;
                }
                if (item is Sandwich)
                {
                    Sandwich sandwich = item as Sandwich;
                    CenterText(counter + ". " + sandwich.GetName(), "$" + price.ToString());
                    counter++;
                }
                if (item is Dessert)
                {
                    Dessert dessert = item as Dessert;
                    CenterText(counter + ". " + dessert.GetName(), "$" + price.ToString());
                    counter++;
                }
                if (item is Soup)
                {
                    Soup soup = item as Soup;
                    CenterText(counter + ". " + soup.GetName(), "$" + price.ToString());
                    counter++;
                }
            }
            if(delivery)
            {
                CenterText("Delivery fee",  "$" + DELIVERY_FEE + ".00");
            }
            CenterText($"Subtotal: ${Double.Round(total, 2)}");
            CenterText("Tax: $" + (Double.Round(total * SALES_TAX_RATE, 2)).ToString());
            CenterText($"Total: ${Double.Round(total + total * SALES_TAX_RATE, 2)}");

            if (Items.Count != 0)
            {
                List<String> validAnswers = ["y", "n"];
                CenterTextNoNewLine("Would you like to customize or remove any items from your cart? (y/n): ");
                String input = Console.ReadLine();
                while (!validAnswers.Contains(input, StringComparer.OrdinalIgnoreCase))
                {
                    CenterText("Invalid input.");
                    input = Console.ReadLine();
                }
                if (input.Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        public static void RemoveItemsFromCart()
        {
            CenterTextNoNewLine("Enter the item number to customize/remove: ");

            String input = Console.ReadLine();
            int itemNumber = -1;
            while (true)
            {
                while (!int.TryParse(input, out itemNumber))
                {
                    CenterText("Invalid input.");
                    input = Console.ReadLine();
                }
                while (itemNumber < 0 || itemNumber > Items.Count)
                {
                    CenterText("Invalid number.");
                    input = Console.ReadLine();
                }
                if (itemNumber > 0 || itemNumber < Items.Count)
                {
                    break;
                }
            }

            CenterTextNoNewLine("Would you like to customize (1) or remove (2) this item?: ");
            input = Console.ReadLine();
            int selection;
            while (true)
            {
                while (!int.TryParse(input, out selection))
                {
                    CenterText("Invalid input.");
                    input = Console.ReadLine();
                }
                if (selection == 1)
                {
                    //TODO
                    break;
                }
                if (selection == 2)
                {
                    total -= Items[itemNumber - 1].Item2;
                    Items.RemoveAt(itemNumber - 1);
                    CenterText("Item removed.");
                    break;
                }
                CenterText("Invalid input.");
                input = Console.ReadLine();
            }
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace PizzaHAL
{
    public abstract class Item
    {
        public static String bar = (new String('=', Console.WindowWidth));

        private String name;
        private double price;
        public Item(String name, double price)
        {
            this.name = name;
            this.price = price;
        }
        public Item()
        {

        }

        public static void CenterText(String text)
        {
            Console.Write(String.Format("{0," + ((Console.WindowWidth / 2) + (text.Length / 2)) + "}" + Environment.NewLine, text));
        }
        public static void CenterText(String text, String text2)
        {
            Console.Write(String.Format("{0," + ((Console.WindowWidth / 4) + (text.Length / 2)) + "}" +
                "{1," + ((Console.WindowWidth / 2) + (text2.Length / 2)) + "}\n", text, text2));
        }
        public static void CenterTextNoNewLine(String text)
        {
            Console.Write(String.Format("{0," + ((Console.WindowWidth / 2) + (text.Length / 2)) + "}", text));
        }
        public String GetName()
        {
            return name;
        }
        public double GetPrice()
        {
            return price;
        }
        public class Pizza : Item
        {
            private const double SMALL_PRICE = 5.99;
            private const double MEDIUM_PRICE = 7.99;
            private const double LARGE_PRICE = 9.99;
            private const double XLARGE_PRICE = 11.99;
            private const double SMALL_TOPPING_PRICE = 0.50;
            private const double MEDIUM_TOPPING_PRICE = 0.70;
            private const double LARGE_TOPPING_PRICE = 0.90;
            private const double XLARGE_TOPPING_PRICE = 1.00;

            private String type;
            private static List<String> validAnswers = ["Regular", "Veggie"];
            private List<String> toppings;
            private enum Toppings
            {
                Pepperoni, 
                Bacon, 
                Pineapple, 
                Sausage,
                Mushrooms,
                Peppers,
            }
            private enum VeggieToppings
            {
                Pineapple, 
                Mushrooms, 
                Peppers
            }
            public Pizza(String name, double price)
                : base(name, price)
            {
                toppings = [];
            }
            public static (Pizza, double) OrderPizza(ref double total)
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine(bar);
                    CenterText("Sizes", "Topping Costs");
                    Console.WriteLine(bar);
                    CenterText("Small: $" + SMALL_PRICE.ToString(), "$" + SMALL_TOPPING_PRICE.ToString() + "0");
                    CenterText("Medium: $" + MEDIUM_PRICE.ToString(), "$" + MEDIUM_TOPPING_PRICE.ToString() + "0");
                    CenterText("Large: $" + LARGE_PRICE.ToString(), "$" + LARGE_TOPPING_PRICE.ToString() + "0");
                    CenterText("X-Large: $" + XLARGE_PRICE.ToString(), "$" + XLARGE_TOPPING_PRICE.ToString() + ".00");
                    Console.WriteLine(bar);
                    CenterTextNoNewLine("Please Select a Size: ");
                    String input = Console.ReadLine();

                    //Validation
                    while (!InputIsValid(input, 0))
                    {
                        CenterText("Invalid Input.");
                        Console.WriteLine(bar);
                        CenterTextNoNewLine("Please Select a Size: ");
                        input = Console.ReadLine();
                    }

                    Pizza pizza = new Pizza(input, 0);
                    
                    Console.WriteLine(bar);
                    CenterTextNoNewLine("Would you like a regular or veggie pizza?: ");
                    input = Console.ReadLine();

                    //Validation
                    while (!validAnswers.Contains(input, StringComparer.OrdinalIgnoreCase))
                    {
                        CenterText("Invalid Input.");
                        Console.WriteLine(bar);
                        CenterTextNoNewLine("Would you like a regular or veggie pizza?: ");
                        input = Console.ReadLine();
                    }

                    pizza.SetType(input);
                    
                    Console.WriteLine(bar);
                    CenterText("Toppings");
                    Console.WriteLine(bar);
                   
                    List<String> ToppingsAvailable = new List<String>();

                    if (pizza.GetType().Equals(validAnswers[0], StringComparison.OrdinalIgnoreCase)) //Regular
                    {
                        //Sets list of toppings available to the names of the Topping enums
                        //For use in validation
                        foreach (Toppings topping in Enum.GetValues(typeof(Toppings)))
                        {
                            ToppingsAvailable.Add(topping.ToString());
                        }
                        //Prints and centers 2 toppings per line to the console
                        int j = 1;
                        for(int i = 0; i < ToppingsAvailable.Count; i+=2)
                        {
                            CenterText(ToppingsAvailable[i], ToppingsAvailable[j]);
                            j+=2;
                        }
                        Console.WriteLine(bar);
                    }
                    else //Veggie
                    {
                        
                        foreach (VeggieToppings topping in Enum.GetValues(typeof(VeggieToppings)))
                        {
                            ToppingsAvailable.Add(topping.ToString());
                        }

                        CenterText(ToppingsAvailable[0], ToppingsAvailable[1]);
                        CenterText(ToppingsAvailable[2]);
                        Console.WriteLine(bar);
                    }                   
                    CenterTextNoNewLine("Select toppings or enter \"N\" to continue: ");
                    input = Console.ReadLine();
                    
                    while (!input.Equals("N", StringComparison.OrdinalIgnoreCase) && ToppingsAvailable.Count != 0) //TODO modularize
                    {
                        if (pizza.GetType().Equals("Veggie", StringComparison.OrdinalIgnoreCase)) //Veggie
                        {
                            while (!input.Equals("N", StringComparison.OrdinalIgnoreCase) 
                                && !InputIsValid(input, 1, true) || pizza.GetToppings().Contains(input))
                            {
                                CenterText("Invalid Input.");
                                Console.WriteLine(bar);
                                CenterTextNoNewLine("Select toppings or enter \"N\" to continue: ");
                                input = Console.ReadLine();

                            }
                            foreach (VeggieToppings topping in Enum.GetValues(typeof(VeggieToppings)))
                            {
                                if(input.Equals(topping.ToString(), StringComparison.OrdinalIgnoreCase))
                                {
                                    pizza.AddTopping(topping.ToString());
                                    ToppingsAvailable.Remove(topping.ToString());
                                    CenterText($"Topping added: {input}");
                                    Console.WriteLine(bar);
                                    CenterTextNoNewLine("Select toppings or enter \"N\" to continue: ");
                                    input = Console.ReadLine();
                                }
                            }
                        }
                        else //Regular
                        {
                            while (!input.Equals("N", StringComparison.OrdinalIgnoreCase) 
                                && !InputIsValid(input, 1) || pizza.GetToppings().Contains(input) )
                            {
                                CenterText("Invalid Input.");
                                Console.WriteLine(bar);
                                CenterTextNoNewLine("Select toppings or enter \"N\" to continue: ");
                                input = Console.ReadLine();
                            }
                            foreach (Toppings topping in Enum.GetValues(typeof(Toppings)))
                            {
                                if (input.Equals(topping.ToString(), StringComparison.OrdinalIgnoreCase))
                                {
                                    pizza.AddTopping(topping.ToString());
                                    ToppingsAvailable.Remove(topping.ToString());
                                    CenterText($"Topping added: {input}");
                                    Console.WriteLine(bar);
                                    CenterTextNoNewLine("Select toppings or enter \"N\" to continue: ");
                                    input = Console.ReadLine();
                                }
                            }
                        }
                        
                    }
                    pizza.CalculatePrice();

                    Console.WriteLine(bar);
                    CenterText(pizza.ToString());
                    CenterText("If this is incorrect, enter \"I\".");
                    CenterTextNoNewLine("Press any key to add to cart:");
                    Char c = Console.ReadKey().KeyChar;

                    if (c != 'I' && c != 'i')
                    {
                        total += pizza.GetPrice();
                        
                        Console.WriteLine(Environment.NewLine + bar);
                        CenterText($"Subtotal:${pizza.GetPrice()} Total:${total}");
                        CenterTextNoNewLine("Press any key to continue: ");
                        Console.ReadKey();
                        Console.Clear();
                        return (pizza, pizza.GetPrice());
                    }
                }
            }
            public static bool InputIsValid(String input, int section, bool IsVeggie = false)
            {
                if (section == 0)
                {
                    if (int.TryParse(input, out _))
                    {
                    return false;
                    }
                    if (!input.Equals("Small", StringComparison.OrdinalIgnoreCase)
                        && !input.Equals("Medium", StringComparison.OrdinalIgnoreCase)
                        && !input.Equals("Large", StringComparison.OrdinalIgnoreCase)
                        && !input.Equals("X-Large", StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                }
                if (section == 1)
                {
                    if(IsVeggie)
                    {
                        foreach (VeggieToppings topping in Enum.GetValues(typeof(VeggieToppings)))
                        {
                            if (input.Equals(topping.ToString(), StringComparison.OrdinalIgnoreCase))
                            {
                                return true;                            

                            }
                        }
                        return false;
                    }
                    else
                    {
                        foreach (Toppings topping in Enum.GetValues(typeof(Toppings)))
                        {
                            if (input.Equals(topping.ToString(), StringComparison.OrdinalIgnoreCase))
                            {
                                return true;
                            }
                        }
                        return false;

                    }
                }
                return true;
            }

            public override string ToString()
            {
                if(toppings.Count == 0)
                {
                    return $"{name} {type} sized pizza";

                }
                return $"{name} sized {type} pizza with " + String.Join(", ", toppings);
            }
            public void CalculatePrice()
            {
                if (name.Equals("Small", StringComparison.OrdinalIgnoreCase))
                {
                    price = (SMALL_PRICE + SMALL_TOPPING_PRICE * toppings.Count);
                    return;
                }
                if (name.Equals("Medium", StringComparison.OrdinalIgnoreCase))
                {
                    price = (MEDIUM_PRICE + MEDIUM_TOPPING_PRICE * toppings.Count);
                    return;
                }
                if (name.Equals("Large", StringComparison.OrdinalIgnoreCase))
                {
                    price = (LARGE_PRICE + LARGE_TOPPING_PRICE * toppings.Count);
                    return;
                }
                if (name.Equals("X-Large", StringComparison.OrdinalIgnoreCase))
                {
                    price = (XLARGE_PRICE + XLARGE_TOPPING_PRICE * toppings.Count);
                    return;
                }
                
            }
           
            public void AddTopping(String topping)
            {
                toppings.Add(topping);
            }
            public List<String> GetToppings()
            {
                return toppings;
            }
            public void SetType(String type)
            {   
                this.type = type;
            }
            public new String GetType()
            {
                return type;
            }
        }

        public class Drink : Item
        {

            public const double SMALL_PRICE = 1.19;
            public const double MEDIUM_PRICE = 1.59;
            public const double LARGE_PRICE = 1.99;

            
            private String size;
            private String ice;

            private enum Flavors
            {
                [Display(Name = "Dr. Dynamite")] dynamite,
                [Display(Name = "Dr. Thunder")] thunder,
                [Display(Name = "Lemon Lime")] lemonlime,
                [Display(Name = "Mountain Lightning")] lightning,
                [Display(Name = "Mr. Pibb")] pibb,
                [Display(Name = "7Dew")] dew
            }

            
            public Drink(String size)
            {
                this.size = size;
            }
            public static (Drink, double) OrderDrink(ref double total)
            {
                while (true)
                {

                    Console.Clear();
                    Console.WriteLine(bar);
                    CenterText("Sizes");
                    Console.WriteLine(bar);
                    CenterText("Small (12 fl. oz):", "$" + SMALL_PRICE);
                    CenterText("Medium (16 fl. oz):", "$" + MEDIUM_PRICE);
                    CenterText("Large (20 fl. oz):", "$" + LARGE_PRICE);
                    Console.WriteLine(bar);
                    CenterTextNoNewLine("Please Select a Size: ");
                    String input = Console.ReadLine();

                    while (!InputIsValid(input, 0))
                    {
                        CenterText("Invalid Input.");
                        Console.WriteLine(bar);
                        CenterTextNoNewLine("Please Select a Size");
                        input = Console.ReadLine();
                    }
                    Drink drink = new Drink(input);
                    drink.SetPrice(input);

                    Console.WriteLine(bar);
                    CenterText("Flavor");
                    Console.WriteLine(bar);
                    CenterText((Flavors.dynamite.GetDisplayName()), (Flavors.thunder.GetDisplayName()));
                    CenterText((Flavors.lemonlime.GetDisplayName()), (Flavors.lightning.GetDisplayName()));
                    CenterText((Flavors.pibb.GetDisplayName()), (Flavors.dew.GetDisplayName()));
                    Console.WriteLine(bar);
                    CenterTextNoNewLine("Please Select a Flavor: ");
                    input = Console.ReadLine();

                    while (!InputIsValid(input, 1))
                    {
                        CenterText("Invalid Input.");
                        Console.WriteLine(bar);
                        CenterTextNoNewLine("Please Select a Flavor: ");
                        input = Console.ReadLine();
                    }
                    foreach (Flavors flavor in Enum.GetValues(typeof(Flavors)))
                    {
                        String Flavor = flavor.GetDisplayName();
                        if (input.Equals(Flavor, StringComparison.OrdinalIgnoreCase))
                        {
                            drink.SetName(Flavor);
                        }
                    }
                    Console.WriteLine(bar);
                    CenterText("Ice");
                    Console.WriteLine(bar);
                    CenterText("None");
                    CenterText("Regular");
                    CenterText("Extra");
                    Console.WriteLine(bar);
                    CenterTextNoNewLine("Please Select an Option: ");

                    input = Console.ReadLine();
                    while (!InputIsValid(input, 2))
                    {
                        CenterText("Invalid Input.");
                        Console.WriteLine(bar);
                        CenterTextNoNewLine("Please Select an Option: ");
                        input = Console.ReadLine();
                    }

                    switch (input)
                    {
                        case "None":
                            drink.SetIce("no");
                            break;
                        case "Regular":
                            drink.SetIce("regular");
                            break;
                        case "Extra":
                            drink.SetIce("extra");
                            break;
                        default:
                            break;
                    }
                    Console.WriteLine(bar);

                    CenterText($"You entered: " + drink.ToString());
                    CenterText("If this is incorrect, enter \"I\".");
                    CenterTextNoNewLine("Press any key to add to cart: ");

                    Char c = Console.ReadKey().KeyChar;
                    if (c != 'I' && c != 'i')
                    {
                        total += drink.GetPrice();

                        CenterText($"Subtotal:${drink.GetPrice()} Total:${total}");
                        Console.WriteLine(Environment.NewLine + bar);
                        CenterTextNoNewLine("Press any key to continue: ");
                        Console.ReadKey();
                        Console.Clear();
                        return (drink, drink.GetPrice());
                    }
                }
            }
            public static bool InputIsValid(String input, int section)
            {
                if (int.TryParse(input, out _))
                {
                    return false;
                }
                if (section == 0) //Size Validation
                {
                    if (!input.Equals("Small", StringComparison.OrdinalIgnoreCase)
                        && !input.Equals("Medium", StringComparison.OrdinalIgnoreCase)
                        && !input.Equals("Large", StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                }
                if (section == 1) //Flavor Validation
                {
                    foreach (Flavors flavor in Enum.GetValues(typeof(Flavors)))
                    {
                        String Flavor = flavor.GetDisplayName();
                        if (input.Equals(Flavor, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                    return false;

                }
                if (section == 2) //Ice Validation
                {
                    if (!input.Equals("None", StringComparison.OrdinalIgnoreCase)
                        && !input.Equals("Regular", StringComparison.OrdinalIgnoreCase)
                        && !input.Equals("Extra", StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                }
                return true;

            }

            public void SetPrice(String name)
            {
                if(name.Equals("Small", StringComparison.OrdinalIgnoreCase))
                {
                    price = SMALL_PRICE;
                }
                if (name.Equals("Medium", StringComparison.OrdinalIgnoreCase))
                {
                    price = MEDIUM_PRICE;
                }
                if (name.Equals("Large", StringComparison.OrdinalIgnoreCase))
                {
                    price = LARGE_PRICE;
                }
            }
            public override string ToString()
            {
                return $"{size} {name} with {ice} ice";
            }
            public String GetSize()
            {
                return size;
            }
            public void SetName(String flavor)
            {
                name = flavor;
            }
            public String GetIce()
            {
                return ice;
            }
            public void SetIce(String ice)
            {
                this.ice = ice;
            }
        }
        public class Dessert : Item
        {
            public const double BROWNIE_PRICE = 6.49;
            public const double COOKIE_PRICE = 2.49;
            public const double TIRAMISU_PRICE = 4.99;

            public Dessert(String name, double price)
                : base(name, price)
            {
            }
            private enum DessertNames
            {
                [Display(Name = ("Cookie Brownie"))]
                Dessert1,
                [Display(Name = "Chocolate Chip Cookie")]
                Dessert2,
                [Display(Name = "Tiramisu")]
                Dessert3,
            }

            public static (Dessert, double) OrderDessert(ref double total)
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine(bar);
                    CenterText("Desserts");
                    Console.WriteLine(bar);

                    CenterText(DessertNames.Dessert1.GetDisplayName(), "$" + Dessert.BROWNIE_PRICE.ToString());
                    CenterText(DessertNames.Dessert2.GetDisplayName(), "$" + Dessert.COOKIE_PRICE.ToString());
                    CenterText(DessertNames.Dessert3.GetDisplayName(), "$" + Dessert.TIRAMISU_PRICE.ToString());


                    Console.WriteLine(bar);
                    CenterTextNoNewLine("Please enter a dessert: ");

                    String input = Console.ReadLine();

                    while (!InputIsValid(input, 0))
                    {
                        CenterText("Invalid Input.");
                        Console.WriteLine(bar);
                        CenterTextNoNewLine("Please enter a dessert: ");
                        input = Console.ReadLine();
                    }

                    Dessert dessert = new Dessert(input, 0);
                    dessert.SetPrice();

                    CenterText($"You entered: {dessert.GetName()}");
                    CenterText("If this is incorrect, enter \"I\".");
                    CenterTextNoNewLine("Press any key to add to cart: ");

                    Char c = Console.ReadKey().KeyChar;

                    if (c != 'I')
                    {
                        total += dessert.GetPrice();
                        CenterText($"Subtotal:${dessert.GetPrice()} Total:${total}");
                        Console.WriteLine(Environment.NewLine + bar);
                        CenterTextNoNewLine("Press any key to continue: ");
                        Console.ReadKey();
                        Console.Clear();

                        return (dessert, dessert.GetPrice());
                    }
                }
            }
            public static bool InputIsValid(String input, int section)
            {
                if (int.TryParse(input, out _))
                {
                    return false;
                }
                if (!input.Equals(DessertNames.Dessert1.GetDisplayName(), StringComparison.OrdinalIgnoreCase)
                    && !input.Equals(DessertNames.Dessert2.GetDisplayName(), StringComparison.OrdinalIgnoreCase)
                    && !input.Equals(DessertNames.Dessert3.GetDisplayName(), StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
                return true;
            }
            public void SetPrice()
            {
                if(name.Equals("Cookie Brownie", StringComparison.OrdinalIgnoreCase))
                {
                    price = BROWNIE_PRICE;
                }
                if (name.Equals("Chocolate Chip Cookie", StringComparison.OrdinalIgnoreCase))
                {
                    price = COOKIE_PRICE;
                }
                if (name.Equals("Tiramisu", StringComparison.OrdinalIgnoreCase))
                {
                    price = TIRAMISU_PRICE;
                }
                return;
            }
        }

        public class Sandwich : Item
        {
            public const double SANDWICH_PRICE = 4.99;
            public Sandwich(String name, double price = SANDWICH_PRICE) : base (name, price)
            {

            }
            public enum SandwichNames
            {
                [Display(Name = ("Buffalo Chicken"))]
                Sandwich1,
                [Display(Name = ("Mediterranean Veggie"))]
                Sandwich2,
                [Display(Name = ("Philly Cheese Steak"))]
                Sandwich3,
                [Display(Name = ("Chicken Parm"))]
                Sandwich4,
            }
            public static (Sandwich, double) OrderSandwich(ref double total)
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine(bar);
                    CenterText("Sandwiches");
                    Console.WriteLine(bar);

                    foreach (SandwichNames name in Enum.GetValues(typeof(SandwichNames)))
                    {
                        CenterText(name.GetDisplayName(), "$" + SANDWICH_PRICE.ToString());
                    }
                    Console.WriteLine(bar);

                    CenterTextNoNewLine("Please select a sandwich: ");
                    String input = Console.ReadLine();


                    while (!InputIsValid(input, 0))
                    {
                        CenterText("Invalid Input.");
                        Console.WriteLine(bar);
                        CenterTextNoNewLine("Please select a sandwich: ");
                        input = Console.ReadLine();
                    }
                    Sandwich sandwich = new Sandwich(input);
                    
                    Console.WriteLine(bar);

                    CenterText($"You entered: {sandwich.GetName()}.");
                    CenterText("If this is incorrect, enter \"I\".");
                    CenterTextNoNewLine("Press any key to add to cart: ");


                    Char c = Console.ReadKey().KeyChar;

                    if (c != 'I')
                    {
                        total += sandwich.GetPrice();

                        CenterText($"Subtotal:${sandwich.GetPrice()} Total:${total}");
                        Console.WriteLine(Environment.NewLine + bar);
                        CenterTextNoNewLine("Press any key to continue: ");
                        Console.ReadKey();
                        Console.Clear();
                        return (sandwich, sandwich.GetPrice());
                    }

                }
            }
            public static bool InputIsValid(String input, int section)
            {
                if (int.TryParse(input, out _))
                {
                    return false;
                }
                if (!input.Equals(SandwichNames.Sandwich1.GetDisplayName(), StringComparison.OrdinalIgnoreCase)
                    && !input.Equals(SandwichNames.Sandwich2.GetDisplayName(), StringComparison.OrdinalIgnoreCase)
                    && !input.Equals(SandwichNames.Sandwich3.GetDisplayName(), StringComparison.OrdinalIgnoreCase)
                    && !input.Equals(SandwichNames.Sandwich4.GetDisplayName(), StringComparison.OrdinalIgnoreCase))
                {
                    return false;

                }
                return true;
            }
        }
         public class Soup : Item
         {
            private enum SoupNames
            {
                [Display(Name = "Tomato Basil")]
                Soup1,
                [Display(Name = "Broccoli Cheddar")]
                Soup2,
                [Display(Name = "Chicken Noodle")]
                Soup3,
                [Display(Name = "Italian Wedding")]
                Soup4,
                [Display(Name = "Clam Chowder")]
                Soup5,
            }
            private static List<(String name, double price)> SoupsAvailable = [];
            private const double SOUP_PRICE = 4.49;
            private const double SOUP_OF_THE_DAY_PRICE = 3.49;
            static Soup() //Static Constructor, will call before first object is initialized. Useful for performing instructions that only need to occur once
                //Replaces RandomizeSoups() method.
            {
                bool soupOfTheDayIsSelected = false;
                Random rand = new Random();
                while (SoupsAvailable.Count < 3)
                {
                    foreach (SoupNames name in Enum.GetValues(typeof(SoupNames)))
                    {
                        if (SoupsAvailable.Count == 3)
                        {
                            break;
                        }
                        if (rand.Next(0, 5) == 0)
                        {
                            if (SoupsAvailable.Contains((name.GetDisplayName(), SOUP_PRICE))
                                || SoupsAvailable.Contains((name.GetDisplayName(), SOUP_OF_THE_DAY_PRICE)))
                            {
                                break;
                            }
                            else if (soupOfTheDayIsSelected)
                            {
                                SoupsAvailable.Add((name.GetDisplayName(), SOUP_PRICE));
                            }
                            else if (rand.Next(0, 2) == 0)
                            {
                                SoupsAvailable.Add((name.GetDisplayName(), SOUP_OF_THE_DAY_PRICE));
                                soupOfTheDayIsSelected = true;
                            }
                        }
                    }
                }
            }
                
            public Soup (String name, double price = SOUP_PRICE) : base (name, price)
            {

            }
            public static (Soup, double) OrderSoup(ref double total)
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine(bar);
                    CenterText("Soups");
                    Console.WriteLine(bar);

                    String extraSpace = String.Empty;
                    for (int i = 0; i < SoupsAvailable.Count; i++)
                    {
                        CenterText(SoupsAvailable[i].name, "$" + SoupsAvailable[i].price.ToString());
                    }
                    CenterTextNoNewLine("Please Select a Soup: ");

                    String input = Console.ReadLine();

                    while (!InputIsValid(input))
                    {
                        CenterText("Invalid Input");
                        Console.WriteLine(bar);
                        CenterTextNoNewLine("Please Select a Soup: ");
                        input = Console.ReadLine();
                    }
                    Soup soup = new Soup(input);
                    Console.WriteLine(bar);

                    CenterText($"You entered: {soup.GetName()}.");
                    CenterText("If this is incorrect, enter \"I\".");
                    CenterTextNoNewLine("Press any key to add to cart: ");
                    Char c = Console.ReadKey().KeyChar;

                    if (c != 'I')
                    {
                        total += Soup.SOUP_PRICE;

                        CenterText($"Subtotal: ${Soup.SOUP_PRICE} Total: ${total}");
                        Console.WriteLine(Environment.NewLine + bar);
                        CenterTextNoNewLine("Press any key to continue: ");
                        Console.ReadKey();
                        Console.Clear();
                        return (soup, Soup.SOUP_PRICE);
                    }
                }
            }
            public static bool InputIsValid(String input)
            {
                foreach (SoupNames name in Enum.GetValues(typeof(SoupNames)))
                {
                    if (input.Equals(name.GetDisplayName(), StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                return false;
            }       
         }
    }
}


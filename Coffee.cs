using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

namespace Coffee;

public class Identity
{
    Dictionary<string, string> users = new Dictionary<string, string>();

    public Identity(string usersFile)
    {
        LoadUsers(usersFile);
        SaveUsers(usersFile);
    }
    void SaveUsers(string usersFile)
    {
        string json = JsonSerializer.Serialize(users);
        File.WriteAllText(usersFile, json);
    }


    void LoadUsers(string usersFile)
    {
        if (File.Exists(usersFile))
        {
            string json = File.ReadAllText(usersFile);
            users = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        }
        else
        {
            users.Add("admin", "password"); // Default user
            SaveUsers(usersFile);
        }
    }


    public string Login(string username, string password)
    {
        if (users.ContainsKey(username) && users[username] == password)
        {
            return username;
        }
        return string.Empty;
    }
}

public class PointOfSale
{


    public void CalculateTotal(List<int> orderItems, List<decimal> menuPrices, string currentUser)
    {
        decimal total = 0;
        foreach (int itemIndex in orderItems)
        {
            total += menuPrices[itemIndex];
        }
        Console.WriteLine($"Total Amount Payable by {currentUser}: {total:C}");
    }
}

public class Menu
{
    public Menu(string menuFile)
    {
        LoadMenu(menuFile);

    }

    public List<string> MenuItems { get; private set; }
    public List<decimal> MenuPrices { get; private set; }

    void LoadMenu(string menuFile)
    {
        if (File.Exists(menuFile))
        {
            string json = File.ReadAllText(menuFile);
            var menu = JsonSerializer.Deserialize<Dictionary<string, decimal>>(json);
            MenuItems = new List<string>(menu.Keys);
            MenuPrices = new List<decimal>(menu.Values);
        }
    }

}
internal class Program
{
    public static void Main(string[] args)
    {
        List<string> menuItems = [];
        List<decimal> menuPrices = [];
        List<int> orderItems = [];
        Dictionary<string, string> users = [];
        string? currentUser = null;
        PointOfSale pos = new PointOfSale();


        string dataDirectory = "data";
        string usersFile = Path.Combine(dataDirectory, "users.json");
        string menuFile = Path.Combine(dataDirectory, "menu.json");
        string ordersFile = Path.Combine(dataDirectory, "orders.json");

        Directory.CreateDirectory(dataDirectory);


        Console.WriteLine("Enter username: ");
        string username = Console.ReadLine();

        Console.WriteLine("Enter password: ");
        string password = Console.ReadLine();

        currentUser = new Identity(usersFile).Login(username, password);
        if (string.IsNullOrEmpty(currentUser))
        {
            Console.WriteLine("Invalid login. Exiting...");
            return;
        }

        Menu menu = new Menu(menuFile);
        menuItems = menu.MenuItems;
        menuPrices = menu.MenuPrices;

        LoadOrders();

        bool running = true;

        while (running)
        {
            Console.WriteLine("Welcome to the Coffee Shop!");
            Console.WriteLine("1. Add Menu Item");
            Console.WriteLine("2. View Menu");
            Console.WriteLine("3. Place Order");
            Console.WriteLine("4. View Order");
            Console.WriteLine("5. Calculate Total");
            Console.WriteLine("6. Exit");
            Console.Write("Select an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddMenuItem();
                    break;
                case "2":
                    ViewMenu();
                    break;
                case "3":
                    PlaceOrder();
                    break;
                case "4":
                    ViewOrder();
                    break;
                case "5":
                    pos.CalculateTotal(orderItems, menuPrices, currentUser);
                    break;
                case "6":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }

        SaveMenu();
        SaveOrders();






        void SaveMenu()
        {
            var menu = new Dictionary<string, decimal>();
            for (int i = 0; i < menuItems.Count; i++)
            {
                menu.Add(menuItems[i], menuPrices[i]);
            }
            string json = JsonSerializer.Serialize(menu);
            File.WriteAllText(menuFile, json);
        }

        void LoadOrders()
        {
            if (File.Exists(ordersFile))
            {
                string json = File.ReadAllText(ordersFile);
                orderItems = JsonSerializer.Deserialize<List<int>>(json);
            }
        }

        void SaveOrders()
        {
            string json = JsonSerializer.Serialize(orderItems);
            File.WriteAllText(ordersFile, json);
        }

        void AddMenuItem()
        {
            Console.Write("Enter item name: ");
            string itemName = Console.ReadLine();
            Console.Write("Enter item price: ");
            decimal itemPrice = decimal.Parse(Console.ReadLine());

            menuItems.Add(itemName);
            menuPrices.Add(itemPrice);

            Console.WriteLine("Item added successfully!");
        }

        void ViewMenu()
        {
            Console.Clear();
            Console.WriteLine("Menu:");
            for (int i = 0; i < menuItems.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {menuItems[i]} - {menuPrices[i]:C}");
            }
        }

        void PlaceOrder()
        {
            ViewMenu();
            Console.Write("Enter the item number to order: ");
            int itemNumber = int.Parse(Console.ReadLine()) - 1;

            if (itemNumber >= 0 && itemNumber < menuItems.Count)
            {
                orderItems.Add(itemNumber);
                Console.WriteLine("Item added to order!");
            }
            else
            {
                Console.WriteLine("Invalid item number. Please try again.");
            }
        }

        void ViewOrder()
        {
            Console.WriteLine("Your Order:");
            foreach (int itemIndex in orderItems)
            {
                Console.WriteLine($"{menuItems[itemIndex]} - {menuPrices[itemIndex]:C}");
            }
        }


    }

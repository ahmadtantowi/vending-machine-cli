
// Title: Vending Machine on CLI
// Author: Ahmad Tantowi
// Language: C#
// Framework: .NET 6

var _products = new List<Product>
{
    new Product(1, "Biskuit", 6_000, 5),
    new Product(2, "Chips", 8_000, 7),
    new Product(3, "Oreo", 10_000, 9),
    new Product(4, "Tango", 12_000, 11),
    new Product(5, "Cokelat", 15_000, 14),
};
var _acceptedMoneys = new[]
{
    2_000d,
    5_000d,
    10_000d,
    20_000d,
    50_000d
};

while (true)
{
    Console.WriteLine();
    Console.WriteLine("=== Welcome to Snack & Food Vending Machine ===");

    // print menus
    foreach(var product in _products)
    {
        Console.WriteLine("[{0}] {1} - {2} ({3})", product.Id, product.Name, product.Price, product.Stock > 0 ? "in stock" : "out of stock");
    }
    Console.WriteLine($"[0] Exit{Environment.NewLine}");

    // get selected menu
    var selectedMenu = GetSelectedMenu();
    if (selectedMenu == 0)
        break;
    
    // check product stock based on selected menu
    var selectedProduct = _products.Find(x => x.Id == selectedMenu);
    if (selectedProduct!.Stock == 0)
    {
        Console.WriteLine($"Menu {selectedProduct.Name} is out of stock");
        continue;
    }

    // get amount
    var amount = GetAmountOfMenu(selectedProduct.Stock);
    var totalPrice = selectedProduct.Price * amount;
    Console.WriteLine($"Total price: {totalPrice}");
    
    // get money
    var money = PutMoney(totalPrice);

    // calculate money back
    var moneyChanges = money - totalPrice;

    // update stock
    var selectedProductIndex = _products.IndexOf(selectedProduct);
    _products[selectedProductIndex] = selectedProduct with { Stock = selectedProduct.Stock - amount };

    // print summary
    Console.WriteLine($"{Environment.NewLine}*** Summary ***");
    Console.WriteLine($"Menu: {selectedProduct.Name}");
    Console.WriteLine($"Amount: {amount}");
    Console.WriteLine($"Total price: {totalPrice}");
    Console.WriteLine($"Money entered: {money}");
    Console.WriteLine($"Money changes: {moneyChanges}");
    Console.WriteLine($"Remaining stock: {_products[selectedProductIndex].Stock}");
    
    Console.Write($"{Environment.NewLine}Press any key to continue...");
    Console.ReadKey();
    Console.WriteLine();
}

int GetSelectedMenu()
{
    int? selectedMenu = null;
    while(!selectedMenu.HasValue)
    {
        Console.Write("Please select the menu based on numbers above: ");
        var read = Console.ReadLine();

        if (!int.TryParse(read, out var menuNumber))
        {
            Console.WriteLine("Inputted menu is not correct!");
            continue;
        }
        else if (!_products.Any(x => x.Id == menuNumber) && menuNumber != 0)
        {
            System.Console.WriteLine("Menu not found!");
            continue;
        }

        selectedMenu = menuNumber;
    }

    return selectedMenu.Value;
}

int GetAmountOfMenu(int stock)
{
    int? amountOfMenu = null;
    while(!amountOfMenu.HasValue)
    {
        Console.Write($"Amount of menu (1-{stock}): ");
        var read = Console.ReadLine();

        if (!int.TryParse(read, out var amount) || amount <= 0)
        {
            Console.WriteLine("Inputted amount is not correct!");
            continue;
        }
        else if (amount > stock)
        {
            Console.WriteLine("Inputted amount is bigger than stock!");
            continue;
        }

        amountOfMenu = amount;
    }

    return amountOfMenu.Value;
}

double PutMoney(double totalPrice)
{
    double? amountOfMoney = null;
    while(!amountOfMoney.HasValue)
    {
        Console.Write("Please input your money: ");
        var read = Console.ReadLine();

        if (!double.TryParse(read, out var money) || money <= 0)
        {
            Console.WriteLine("Inputted money is not correct!");
            continue;
        }
        else if (!_acceptedMoneys!.Contains(money))
        {
            Console.WriteLine($"Amount of money is not correct. Allowed money is {string.Join(", ", _acceptedMoneys!)}.");
            continue;
        }
        else if (money < totalPrice)
        {
            Console.WriteLine("Your money is not enough. Please input bigger money.");
            continue;
        }

        amountOfMoney = money;
    }

    return amountOfMoney.Value;
}

record Product(int Id, string Name, double Price, int Stock);
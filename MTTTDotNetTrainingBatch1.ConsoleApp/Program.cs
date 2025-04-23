using MTTTDotNetTrainingBatch1.ConsoleApp;

InventoryService inventoryService = new InventoryService();

Console.WriteLine("Inventory Management System");
BeforeOption:
Console.WriteLine("1. Create Product \n2. View Product \n3. Update Product \n4. Delete Product \n5. Exit");
Console.Write("Select an option: ");
string option = Console.ReadLine()!;

bool IsInt = int.TryParse(option, out int choice);
if (!IsInt)
{
    Console.WriteLine("Invalid Input!");
    goto BeforeOption;
}

switch (choice)
{
    case 1:
        Console.WriteLine("Create the Product!");
        inventoryService.CreateProduct();
        goto BeforeOption;

    case 2:
        Console.WriteLine("View the Product!");
        inventoryService.ViewProduct();
        goto BeforeOption;

    case 3:
        Console.WriteLine("Update the Product!");
        inventoryService.UpdateProduct();
        goto BeforeOption;

    case 4:
        Console.WriteLine("Delete the Product!");
        inventoryService.DeleteProduct();
        goto BeforeOption;

    case 5:
        Console.WriteLine("Exiting...");
        goto Exit;

    default:
        Console.WriteLine("Invalid Input!");
        goto BeforeOption;
}

Exit:
Console.ReadKey();




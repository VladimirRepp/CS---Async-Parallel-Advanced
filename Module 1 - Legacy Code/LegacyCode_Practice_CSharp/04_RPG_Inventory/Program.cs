using System;
using System.Collections.Generic;

var player = new Player("Леон", 60);
var manager = new InventoryManager();

manager.AddItem(player, new Item("Малое зелье", "Potion", 50));
manager.AddItem(player, new Item("Стальной меч", "Weapon", 120));

manager.UseItem(player, player.Inventory[0]);
manager.SaveInventory(player);

public class Item
{
    public string Name, Type;
    public int Value;

    public Item(string n, string t, int v)
    {
        Name = n;
        Type = t;
        Value = v;
    }
}

public class Player
{
    public string Name;
    public int Health;
    public List<Item> Inventory = new();

    public Player(string n, int h)
    {
        Name = n;
        Health = h;
    }
}

public class InventoryManager
{
    public void AddItem(Player p, Item i)
    {
        if (i == null)
        {
            Console.WriteLine("Нет предмета");
            return;
        }

        if (p.Inventory.Count >= 20)
        {
            Console.WriteLine("Инвентарь полон");
            return;
        }

        p.Inventory.Add(i);
        Console.WriteLine("Добавлен " + i.Name);
    }

    public void UseItem(Player p, Item i)
    {
        if (i == null)
            return;

        if (i.Type == "Potion")
        {
            new PotionManager().UseHealthPotion(p, i);
            p.Inventory.Remove(i);
        }
        else if (i.Type == "Weapon")
        {
            Console.WriteLine("Экипировано " + i.Name);
        }
        else
        {
            Console.WriteLine("Нельзя использовать");
        }
    }

    public void SaveInventory(Player p)
    {
        new InventoryRepository().Save(p.Inventory);
    }
}

public class PotionManager
{
    public void UseHealthPotion(Player p, Item i)
    {
        p.Health += i.Value;

        if (p.Health > 100)
            p.Health = 100;

        Console.WriteLine("HP: " + p.Health);
    }
}

public class InventoryRepository
{
    public void Save(List<Item> items)
    {
        Console.WriteLine("Сохранено предметов: " + items.Count);
    }
}
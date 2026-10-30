using System;
using System.Collections.Generic;

var player = new Player("Элла", 7000, 12);
var shop = new GameShop();

shop.BuyItem(player, new ShopItem("Зелье", "Potion", 1200));
shop.BuyItem(player, new ShopItem("Меч", "Weapon", 5200));

shop.SellItem(player, player.Inventory[0]);
shop.PrintPlayerInfo(player);

public class Player
{
    public string Name;
    public decimal Gold;
    public int Level;
    public List<ShopItem> Inventory = new();

    public Player(string n, decimal g, int l)
    {
        Name = n;
        Gold = g;
        Level = l;
    }
}

public class ShopItem
{
    public string Name, Type;
    public decimal Price;

    public ShopItem(string n, string t, decimal p)
    {
        Name = n;
        Type = t;
        Price = p;
    }
}

public class GameShop
{
    List<string> log = new();

    public void BuyItem(Player p, ShopItem i)
    {
        if (i == null)
        {
            Console.WriteLine("Нет предмета");
            return;
        }

        decimal price = i.Price;

        if (p.Level >= 10)
            price -= price * .10m;

        if (i.Type == "Potion" && p.Level >= 5)
            price -= 50;

        if (price < 0)
            price = 0;

        if (p.Gold < price)
        {
            Console.WriteLine("Недостаточно золота");
            return;
        }

        p.Gold -= price;
        p.Inventory.Add(i);

        if (price >= 5000)
            p.Gold += 100;

        if (i.Type == "Potion")
            Console.WriteLine("Зелье добавлено");
        else if (i.Type == "Weapon")
            Console.WriteLine("Оружие добавлено");
        else
            Console.WriteLine("Предмет добавлен");

        log.Add(p.Name + " купил " + i.Name);
        Console.WriteLine("Остаток: " + p.Gold);
    }

    public void SellItem(Player p, ShopItem i)
    {
        if (p.Inventory.Contains(i))
        {
            decimal price = i.Price * .5m;

            if (i.Type == "Weapon")
                price = i.Price * .6m;

            p.Inventory.Remove(i);
            p.Gold += price;

            log.Add(p.Name + " продал " + i.Name);
            Console.WriteLine("Получено: " + price);
        }
        else
        {
            Console.WriteLine("Предмет отсутствует");
        }
    }

    public void PrintPlayerInfo(Player p)
    {
        Console.WriteLine(p.Name + " | уровень " + p.Level + " | золото " + p.Gold);

        foreach (var i in p.Inventory)
            Console.WriteLine(i.Name + " / " + i.Type);

        Console.WriteLine("Операций: " + log.Count);
    }
}
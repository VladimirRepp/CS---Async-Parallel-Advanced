using System;

var billing = new BillingService();

billing.CreateInvoice(new Client("ООО Вектор", true), 3, 1200);
billing.CreateInvoice(new Client("ИП Соколов", false), 12, 800);

public class Client
{
    public string Name;
    public bool Business;

    public Client(string n, bool b)
    {
        Name = n;
        Business = b;
    }
}

public class BillingService
{
    public void CreateInvoice(Client c, int months, decimal monthlyPrice)
    {
        decimal amount = months * monthlyPrice;

        if (c.Business)
            amount -= amount * .15m;

        if (months >= 12)
            amount -= amount * .10m;

        if (c.Business && months >= 12)
            amount -= amount * .05m;

        Console.WriteLine("Счёт " + c.Name);
        Console.WriteLine("Период " + months);
        Console.WriteLine("Стоимость " + amount);
        Console.WriteLine("НДС " + amount * .20m);
        Console.WriteLine("Итого " + (amount + amount * .20m));
    }

    public void PrintRenewal(Client c, int months, decimal monthlyPrice)
    {
        decimal amount = months * monthlyPrice;

        if (c.Business)
            amount -= amount * .15m;

        if (months >= 12)
            amount -= amount * .10m;

        if (c.Business && months >= 12)
            amount -= amount * .05m;

        Console.WriteLine("Продление " + c.Name + ": " + (amount + amount * .20m));
    }
}
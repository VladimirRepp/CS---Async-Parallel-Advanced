using System;
using System.Collections.Generic;
var customer = new Customer("Анна", "anna@example.com", true);
var products = new List<Product> { new("Клавиатура",3500,1), new("Мышь",1800,2) };
new OrderProcessor().ProcessOrder(customer, products);
public class Product { public string Name; public decimal Price; public int Quantity; public Product(string n,decimal p,int q){Name=n;Price=p;Quantity=q;} }
public class Customer { public string Name; public string Email; public bool Regular; public Customer(string n,string e,bool r){Name=n;Email=e;Regular=r;} }
public class OrderProcessor {
 public void ProcessOrder(Customer c,List<Product> ps) {
  decimal total=0,discount=0,delivery=500;
  Console.WriteLine("Проверка заказа...");
  if(c==null){Console.WriteLine("Нет покупателя");return;}
  if(string.IsNullOrWhiteSpace(c.Email)){Console.WriteLine("Нет email");return;}
  if(ps==null||ps.Count==0){Console.WriteLine("Корзина пуста");return;}
  foreach(var p in ps){if(p==null||p.Price<=0||p.Quantity<=0){Console.WriteLine("Некорректный товар");return;} total+=p.Price*p.Quantity;}
  if(c.Regular) discount+=total*.10m;
  if(total>=10000) discount+=total*.05m;
  total-=discount;
  if(total>=5000) delivery=0;
  Console.WriteLine("ЧЕК"); Console.WriteLine("Покупатель: "+c.Name);
  foreach(var p in ps) Console.WriteLine(p.Name+" x "+p.Quantity+" = "+p.Price*p.Quantity);
  Console.WriteLine("Сумма: "+(total+discount)); Console.WriteLine("Скидка: "+discount);
  Console.WriteLine("Доставка: "+delivery); Console.WriteLine("Итого: "+(total+delivery));
 }
}

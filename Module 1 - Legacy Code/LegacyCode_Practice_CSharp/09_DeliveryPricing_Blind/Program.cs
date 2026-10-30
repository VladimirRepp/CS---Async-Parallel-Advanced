using System;
var calc=new DeliveryCalculator();calc.PrintPrice(8.5,"Express","Moscow");calc.PrintPrice(3,"Standard","Kazan");
public class DeliveryCalculator {
 public void PrintPrice(double weight,string type,string city){double price=300;if(weight>5)price+=(weight-5)*40;if(type=="Express")price*=1.8;if(type=="Pickup")price=0;if(city=="Moscow")price+=250;if(city=="Kazan")price+=150;if(weight>30)Console.WriteLine("Крупный груз");Console.WriteLine(type+" / "+city+" / "+weight+" кг / "+price);}
 public void PrintInvoice(double weight,string type,string city){double price=300;if(weight>5)price+=(weight-5)*40;if(type=="Express")price*=1.8;if(type=="Pickup")price=0;if(city=="Moscow")price+=250;if(city=="Kazan")price+=150;Console.WriteLine("СЧЁТ: "+city+", "+price);}
}

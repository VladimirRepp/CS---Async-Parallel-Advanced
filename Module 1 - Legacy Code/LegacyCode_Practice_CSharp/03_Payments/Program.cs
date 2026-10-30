using System;
using System.Collections.Generic;
var history=new List<string>(); var pay=new PaymentProcessor(history);
pay.PayWithAudit(new Payment(101,"client@example.com",1250)); pay.PayWithAudit(new Payment(102,"client@example.com",0));
Console.WriteLine("Записей: "+history.Count);
public class Payment { public int Id; public string Email; public decimal Amount; public bool Success; public Payment(int i,string e,decimal a){Id=i;Email=e;Amount=a;} }
public class PaymentProcessor {
 List<string> history; public PaymentProcessor(List<string> h){history=h;}
 public void Pay(Payment p) {
  if(p.Amount<=0){Console.WriteLine("Некорректная сумма");p.Success=false;return;}
  if(p.Amount>100000){Console.WriteLine("Превышен лимит");p.Success=false;return;}
  Console.WriteLine("Подключение к шлюзу..."); Console.WriteLine("Списание "+p.Amount);
  p.Success=true; Console.WriteLine("Оплата выполнена");
 }
 public void PayWithAudit(Payment p){Console.WriteLine("Начало "+p.Id);history.Add("START "+p.Id);Pay(p);history.Add("RESULT "+p.Id+" "+p.Success);Console.WriteLine("Завершение "+p.Id);}
 public void PayWithNotification(Payment p){Console.WriteLine("Уведомление "+p.Email);Pay(p);Console.WriteLine("Уведомление отправлено");}
}

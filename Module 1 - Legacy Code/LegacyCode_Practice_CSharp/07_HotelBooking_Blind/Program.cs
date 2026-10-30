using System;
using System.Collections.Generic;
var hotel=new HotelManager(); hotel.BookRoom("Иван",2,"Standard");hotel.BookRoom("Мария",3,"Suite");hotel.PrintReport();
public class HotelManager {
 List<string> guests=new(),rooms=new();List<decimal> payments=new();
 public void BookRoom(string guest,int days,string roomType){decimal price=0;if(roomType=="Standard")price=2500;if(roomType=="Suite")price=6000;if(roomType=="Family")price=4500;decimal total=price*days;if(days>=7)total-=total*.12m;if(roomType=="Suite"&&days>=3)total+=1500;guests.Add(guest);rooms.Add(roomType);payments.Add(total);Console.WriteLine("Бронь "+guest+" / "+roomType+" / "+days+" дней / "+total);}
 public void CancelBooking(int index){if(index>=0&&index<guests.Count){Console.WriteLine("Отмена "+guests[index]);guests.RemoveAt(index);rooms.RemoveAt(index);payments.RemoveAt(index);}}
 public void PrintReport(){decimal sum=0;for(int i=0;i<guests.Count;i++){Console.WriteLine(guests[i]+" | "+rooms[i]+" | "+payments[i]);sum+=payments[i];}Console.WriteLine("Броней: "+guests.Count+"; выручка: "+sum);}
}

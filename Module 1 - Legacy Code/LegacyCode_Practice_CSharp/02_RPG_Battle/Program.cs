using System;
var hero=new Character("Рыцарь",100,18,12); var orc=new Monster("Орк",75,9,40);
new BattleManager().ProcessBattle(hero,orc);
public class Character { public string Name; public int Health,Attack,Level,Experience; public Character(string n,int h,int a,int l){Name=n;Health=h;Attack=a;Level=l;} }
public class Monster { public string Name; public int Health,Attack,Reward; public Monster(string n,int h,int a,int r){Name=n;Health=h;Attack=a;Reward=r;} }
public class BattleManager {
 Random random=new Random(7);
 public void ProcessBattle(Character p,Monster m) {
  Console.WriteLine("Бой: "+p.Name+" против "+m.Name);
  while(p.Health>0&&m.Health>0) {
   int damage=p.Attack;
   if(random.Next(1,101)<=20){if(p.Level>10)damage*=3;else damage*=2;Console.WriteLine("Критический удар!");}
   m.Health-=damage; Console.WriteLine(p.Name+" наносит "+damage);
   if(m.Health<=0){m.Health=0;Console.WriteLine("Монстр побеждён");p.Experience+=m.Reward;Console.WriteLine("Опыт: "+m.Reward);break;}
   p.Health-=m.Attack; Console.WriteLine(m.Name+" наносит "+m.Attack);
   if(p.Health<=0){p.Health=0;Console.WriteLine("Герой погиб");break;}
   Console.WriteLine("HP героя: "+p.Health+"; HP монстра: "+m.Health);
  }
  Console.WriteLine("Итог: "+p.Health+" / "+m.Health);
 }
}

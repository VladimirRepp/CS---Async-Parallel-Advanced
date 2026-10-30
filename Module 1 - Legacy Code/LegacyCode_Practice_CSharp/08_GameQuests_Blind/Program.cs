using System;
using System.Collections.Generic;
var manager=new QuestManager();manager.AddQuest("Победить волков",3,100);manager.AddQuest("Собрать травы",5,60);
manager.AddProgress("Победить волков",2);manager.AddProgress("Победить волков",2);manager.PrintQuests();
public class Quest { public string Title;public int Required,Progress,Reward;public bool Completed;public Quest(string t,int r,int x){Title=t;Required=r;Reward=x;} }
public class QuestManager {
 List<Quest> quests=new();int playerGold=0;
 public void AddQuest(string t,int r,int reward){quests.Add(new Quest(t,r,reward));Console.WriteLine("Добавлено "+t);}
 public void AddProgress(string title,int amount){foreach(var q in quests){if(q.Title==title){q.Progress+=amount;if(q.Progress>=q.Required){q.Completed=true;playerGold+=q.Reward;Console.WriteLine("Выполнено "+title+"; награда "+q.Reward);}}}}
 public void PrintQuests(){foreach(var q in quests)Console.WriteLine(q.Title+": "+q.Progress+"/"+q.Required+"; done="+q.Completed);Console.WriteLine("Золото: "+playerGold);}
}

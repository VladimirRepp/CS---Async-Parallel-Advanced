using System;
using System.Collections.Generic;

var library = new LibraryManager();

library.Add("Основы C#", false);
library.Add("История игр", false);

library.Borrow("Основы C#");
library.PrintAvailable();
library.Return("Основы C#");

public static class MediaStorage
{
    public static List<string> Titles = new();
    public static List<bool> Borrowed = new();
}

public class LibraryManager
{
    public void Add(string title, bool borrowed)
    {
        MediaStorage.Titles.Add(title);
        MediaStorage.Borrowed.Add(borrowed);
        Console.WriteLine("Добавлено " + title);
    }

    public void Borrow(string title)
    {
        for (int i = 0; i < MediaStorage.Titles.Count; i++)
        {
            if (MediaStorage.Titles[i] == title && !MediaStorage.Borrowed[i])
            {
                MediaStorage.Borrowed[i] = true;
                Console.WriteLine("Выдано " + title);
            }
        }
    }

    public void Return(string title)
    {
        for (int i = 0; i < MediaStorage.Titles.Count; i++)
        {
            if (MediaStorage.Titles[i] == title && MediaStorage.Borrowed[i])
            {
                MediaStorage.Borrowed[i] = false;
                Console.WriteLine("Возвращено " + title);
            }
        }
    }

    public void PrintAvailable()
    {
        for (int i = 0; i < MediaStorage.Titles.Count; i++)
        {
            if (!MediaStorage.Borrowed[i])
                Console.WriteLine(MediaStorage.Titles[i]);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

class Program
{
    public static int state = 0;
    static void Main()
    {
        Console.CursorVisible = false;

        string filePath = @"C:\Users\Kevin\Documents\erm\erm\questions.json"; // fungerar endast på min maskin huehue >:9
        Quiz.LoadQuestions(filePath);

        Menu.Start();
        Quiz.Play();
    }
}

public class Json1
{
    public string Question { get; set; }
    public List<string> Options { get; set; }
    public string Answer { get; set; }
}


public static class Menu {
    private static int index = 0;
    private static bool john = false;
    public static IReadOnlyList<string> Options { get; } = new List<string>
    {
        "play",
        "quit"
    };
    public static void Start() {
        while (Program.state == 0)
        {
            Console.Clear();
            for (int i = 0; i < Options.Count; i++) {
                if (index == i) {
                    Console.Write("\x1b[47m\x1b[30m");
                    Console.Write(Options[i]);
                    Console.WriteLine("\x1b[0m");
                }
                else {
                    Console.WriteLine(Options[i]);
                }

            }

            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.UpArrow)
            {
                index = (index > 0) ? index - 1 : Options.Count - 1;
            }
            else if (key == ConsoleKey.DownArrow)
            {
                index = (index + 1) % Options.Count;
            }
            else if (key == ConsoleKey.Enter)
            {
                if (index == 0) {
                    Program.state = 1;
                }
                if (index == 1) {
                    System.Environment.Exit(0);
                }
            }
        }
    }
}
public static class Quiz
{
    private static int currentQ = 0; 
    private static int index = 0;   
    private static bool john = false; 
    public static List<Json1> Questions;
    public static List<Json1> LoadQuestions(string filePath)
    {
        try
        {
            string json = File.ReadAllText(filePath);
            Questions = JsonSerializer.Deserialize<List<Json1>>(json);
            return Questions;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public static void Play()
    {
        while (Program.state == 1)
        {
        while (!john)
        {
            Console.Clear();

            Console.WriteLine(Questions[currentQ].Question + "\n");

            for (int i = 0; i < Questions[currentQ].Options.Count; i++)
            {
                if (i == index)
                {
                    Console.Write("\x1b[47m\x1b[30m");
                }
                else
                {
                    Console.Write("\x1b[0m");
                }

                Console.WriteLine(Questions[currentQ].Options[i]);
            }

            Console.WriteLine("\x1b[0m");

            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.UpArrow)
            {
                index = (index > 0) ? index - 1 : Questions[currentQ].Options.Count - 1;
            }
            else if (key == ConsoleKey.DownArrow)
            {
                index = (index + 1) % Questions[currentQ].Options.Count;
            }
            else if (key == ConsoleKey.Enter)
            {
                if (Questions[currentQ].Options[index] == Questions[currentQ].Answer)
                {
                    Console.Clear();
                    Console.WriteLine("Correct Answer!");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Wrong Answer!");
                }

                currentQ++;
                if (currentQ >= Questions.Count)
                {
                    john = true;
                }

                index = 0; 
                Console.ReadKey();
            }
        }

        Console.Clear();
        Console.WriteLine("Quiz Over! Thanks for playing.");
        System.Threading.Thread.Sleep(500);
        Program.state = 0;
    }
    }
}

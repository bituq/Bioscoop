using System;
using System.Collections.Generic;
using System.Text;
using CinemaUI;
using CinemaUI.Builder;

namespace CinemaApplication
{
    partial class Program
    {
        public class Person
        {
            public Window window = new Window();
            public string Name;

            public Person(string name, string age, string height)
            {
                this.Name = name;

                var title = new TextBuilder(window, 3, 2)
                    .Color(ConsoleColor.DarkMagenta)
                    .Text($"Informatie over {name}")
                    .Result();

                var information = new TextListBuilder(window, 3, 5)
                    .Color(ConsoleColor.Gray)
                    .SetItems("Naam: " + name, "Age: " + age, "Height: " + height)
                    .Result();

                var menu = new TextListBuilder(window, 3, 12)
                    .SetItems("Go back")
                    .Selectable(ConsoleColor.Black, ConsoleColor.White)
                    .LinkWindows(startScreen)
                    .Result();
            }
        }

        static Window startScreen = new Window(true);
        static void StartScreen()
        {
            var people = new List<Person>();

            people.Add(new Person("John", "47", "1.82m"));

            var names = new List<string>();
            var personWindows = new List<Window>();
            foreach (Person p in people)
            {
                personWindows.Add(p.window);
                names.Add(p.Name);
            }

            var textListTest = new TextListBuilder(startScreen, 3, 2)
                .Color(ConsoleColor.Cyan)
                .SetItems(names.ToArray())
                .UseNumbers()
                .Selectable(ConsoleColor.DarkBlue, ConsoleColor.White)
                .LinkWindows(personWindows.ToArray())
                .Result();

            var inputInformation = new TextListBuilder(startScreen, 25, 2)
                .Color(ConsoleColor.DarkGray)
                .SetItems("Name: ", "Age: ", "Height: ")
                .Result();

            var inputList = new TextListBuilder(startScreen, 33, 2)
                .SetItems("", "", "")
                .AsInput(ConsoleColor.White, ConsoleColor.Black)
                .Result();

            var submitButton = new TextListBuilder(startScreen, 25, 6)
                .Color(ConsoleColor.Green)
                .SetItems("Submit")
                .Selectable(ConsoleColor.White, ConsoleColor.DarkGreen)
                .Result();

            submitButton.Items[0].OnClick = () =>
            {
                if (!inputList.Items.Exists(item => item.Value == ""))
                {
                    people.Add(new Person(inputList[0].Value, inputList[1].Value, inputList[2].Value));

                    var names = new List<string>();
                    var personWindows = new List<Window>();
                    foreach (Person p in people)
                    {
                        personWindows.Add(p.window);
                        names.Add(p.Name);
                    }

                    textListTest.Replace(new TextListBuilder(startScreen, 3, 2)
                    .Color(ConsoleColor.Cyan)
                    .SetItems(names.ToArray())
                    .UseNumbers()
                    .Selectable(ConsoleColor.DarkBlue, ConsoleColor.White)
                    .LinkWindows(personWindows.ToArray())
                    .Result());
                }
            };
        }
    }
}

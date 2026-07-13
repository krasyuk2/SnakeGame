using SnakeGame.Domain.Models;

namespace SnakeGame.Client.Applications;

/// <summary>
///     Класс главного меню.
/// </summary>
public class MainMenu
{
    /// <summary>
    ///     Перечисление полей.
    /// </summary>
    private readonly string[] _menuItems = 
    {
        "[] - Создать лобби.",
        "[] - Подключится к существующему."
    };
    
    public async Task<ChoiceMainMenuEnum> Start()
    {
        var countChoicer = 0;
        RenderMenu(countChoicer);
        while (true)
        {
            if (Console.KeyAvailable)
            {
                var userInput = Console.ReadKey(true);
                switch (userInput.Key)
                {
                    case ConsoleKey.DownArrow:
                        countChoicer++;
                        break;
                    case ConsoleKey.UpArrow:
                        countChoicer--;
                        break;
                    case ConsoleKey.Enter:
                        Console.Clear();
                        return (ChoiceMainMenuEnum)countChoicer;
                    
                }
                if(countChoicer >= _menuItems.Length) countChoicer = 0;
                if(countChoicer < 0) countChoicer = _menuItems.Length - 1;
                RenderMenu(countChoicer);
            }
            await Task.Delay(10);
        }
    }

    /// <summary>
    ///     Отрисовать меню выбора.
    /// </summary>
    /// <param name="counter"> Число выбора. </param>
    private void RenderMenu(int counter)
    {
        Console.Clear();
        Console.WriteLine("Выберете:");
        for (int i = 0; i < _menuItems.Length; i++)
        {
            var item = _menuItems[i];
            if (i == counter)
                item = item.Insert(1, "x");
            Console.WriteLine(item);
        } 
        Console.WriteLine("Управление: Стрелочки)");
    }
}
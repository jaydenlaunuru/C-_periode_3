using System;

namespace Balatro1
{
    public class ViewModel
    {
        private GameEngine _engine;
        private int _cursorIndex = 0;

        public ViewModel(Model model)
        {
            _engine = new GameEngine(model);
        }

        public void Run()
        {
            _engine.Reset();
            bool playing = true;

            while (playing)
            {
                if (_engine.CurrentHand.Count == 0)
                    _cursorIndex = 0;
                else if (_cursorIndex >= _engine.CurrentHand.Count)
                    _cursorIndex = _engine.CurrentHand.Count - 1;

                Console.Clear();
                Console.WriteLine("Balatro C# Clone");
                Console.WriteLine($"Score: {_engine.TotalScore}");
                Console.WriteLine($"Ronde: {_engine.RoundsPlayed}/{_engine.MaxRounds}");
                Console.WriteLine($"Deck: {_engine.RemainingCards} Kaarten over");
                Console.WriteLine("-----------------------------------");

                if (_engine.IsGameOver)
                {
                    Console.WriteLine("GAME OVER");
                    Console.WriteLine($"Eindscore: {_engine.TotalScore}");
                    Console.WriteLine("Wil je doorgaan?");
                    Console.WriteLine("[C] Opnieuw spelen | [Q] Stoppen");

                    string? gameOverInput = Console.ReadLine()?.ToUpper();

                    if (gameOverInput == "C")
                    {
                        _engine.Reset();
                        continue;
                    }

                    if (gameOverInput == "Q")
                    {
                        playing = false;
                        continue;
                    }

                    continue;
                }

                for (int i = 0; i < _engine.CurrentHand.Count; i++)
                {
                    string cursor = i == _cursorIndex ? ">" : " ";
                    string check = _engine.SelectedCards.Contains(_engine.CurrentHand[i]) ? "[X]" : "[ ]";
                    Console.Write($"{cursor} {check} {i + 1}: ");
                    PrintCardWithColor(_engine.CurrentHand[i]);
                    Console.WriteLine();
                }

                Console.WriteLine("-----------------------------------");

                if (_engine.SelectedCards.Count > 0)
                    ShowScore();
                else
                    Console.WriteLine("Kies een kaart");

                Console.WriteLine("\n[↑/↓] Scroll | [Enter] Kies | [S]peel | [D]iscard | [R]eset | [Q]uit");
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Q)
                    playing = false;
                else if (keyInfo.Key == ConsoleKey.S)
                {
                    _engine.PlayHand();
                    Console.WriteLine("Gespeeld!");
                    Console.ReadKey(true);
                }
                else if (keyInfo.Key == ConsoleKey.D)
                    _engine.DiscardCards();
                else if (keyInfo.Key == ConsoleKey.R)
                {
                    _engine.Reset();
                    _cursorIndex = 0;
                }
                else if (keyInfo.Key == ConsoleKey.UpArrow && _cursorIndex > 0)
                    _cursorIndex--;
                else if (keyInfo.Key == ConsoleKey.DownArrow && _cursorIndex < _engine.CurrentHand.Count - 1)
                    _cursorIndex++;
                else if (keyInfo.Key == ConsoleKey.Enter && _engine.CurrentHand.Count > 0)
                    _engine.ToggleSelection(_engine.CurrentHand[_cursorIndex]);
            }
        }

        private void PrintCardWithColor(Card card)
        {
            string typePrefix = card.GetTypePrefix();
            string valueStr = card.GetValueString();
            string suitSymbol = card.GetSuitSymbol();

            if (!string.IsNullOrEmpty(typePrefix))
            {
                string typeName = card.DisplayTypeName();
                ConsoleColor typeColor = typeName switch
                {
                    "BonusCard" => ConsoleColor.Yellow,
                    "ExtraCard" => ConsoleColor.Cyan,
                    "GlassCard" => ConsoleColor.Magenta,
                    "WildCard" => ConsoleColor.DarkYellow,
                    "SteelCard" => ConsoleColor.Gray,
                    _ => ConsoleColor.White
                };

                Console.ForegroundColor = typeColor;
                Console.Write(typePrefix);
                Console.ResetColor();
            }

            Console.Write(valueStr);

            ConsoleColor suitColor = card.Suit switch
            {
                Suit.Hearts => ConsoleColor.Red,
                Suit.Diamonds => ConsoleColor.Red,
                Suit.Clubs => ConsoleColor.White,
                Suit.Spades => ConsoleColor.White,
                _ => ConsoleColor.White
            };

            Console.ForegroundColor = suitColor;
            Console.Write(suitSymbol);
            Console.ResetColor();
        }

        private void ShowScore()
        {
            var (chips, multi) = _engine.GetCurrentScore();
            var combo = _engine.GetCurrentCombination();

            if (combo != null)
                Console.WriteLine($"Combo: {combo.Name} (+{combo.BaseChips}, x{combo.BaseMultiplier})");

            Console.WriteLine($"Score: {chips * multi}");
        }
    }
}
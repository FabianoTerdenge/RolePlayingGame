using RPG.Library.Models.Characters;
using RPG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Library
{
    public class CombatMap
    {
        private List<Character> _combatants = new List<Character>();
        private int _padding = 2;

        public CombatMap(List<Character> combatants)
        {
            _combatants = combatants.ToList();
        }
        public void AddCombatant(Character character, Vector3 position)
        {
            _combatants.Add(character);
        }

        public void DrawMap()
        {
            if (_combatants.Count == 0) return;

            // Calculate map bounds
            float minX = _combatants.Min(c => c.Position.X);
            float maxX = _combatants.Max(c => c.Position.X);
            float minY = _combatants.Min(c => c.Position.Y);
            float maxY = _combatants.Max(c => c.Position.Y);

            // Calculate grid dimensions
            int gridWidth = (int)Math.Ceiling((maxX - minX)) + _padding * 2;
            int gridHeight = (int)Math.Ceiling((maxY - minY)) + _padding * 2;
            int gridWidthMinValue = (int)Math.Floor(minX - 2);
            int gridWidthMaxValue = (int)Math.Ceiling(maxX + 2);
            int gridHeightMinValue = (int)Math.Floor(minY - 2);
            int gridHeightMaxValue = (int)Math.Ceiling(maxY + 2);


            string text = "Position\n";
            string text2 = "Combat Map\n";
            int totalWidth = gridWidth * 8;

            // Calculate left padding to center the text
            int leftPadding = (totalWidth - text.Length) / 2;
            int leftPadding2 = (totalWidth - text2.Length) / 2;
            string centeredTextPos = text.PadLeft(leftPadding + text.Length).PadRight(totalWidth);
            string centeredTextCombatMap = text2.PadLeft(leftPadding2 + text2.Length).PadRight(totalWidth);
            Console.WriteLine();
            Console.WriteLine(centeredTextPos);
            _combatants.ForEach(c => Console.WriteLine("{0,-20} Pos: {1,-15} HP: {2,-5}", c.Name, c.Position, c.CurrentHealth));
            Console.WriteLine();
            Console.WriteLine(centeredTextCombatMap);

            // Draw the grid
            for (int i = gridWidthMinValue; i <= gridWidthMaxValue; i++)
            {
                Console.Write("{0,6}", i);
            }
            Console.WriteLine();

            for (int y = gridHeightMinValue; y <= gridHeightMaxValue; y++)
            {
                for (int i = gridWidthMinValue; i <= gridWidthMaxValue; i++)
                {
                    Console.Write(i == gridWidthMinValue ? "   ┌────┐" : "┌────┐");
                }
                Console.WriteLine();

                Console.Write("{0,3}", y);

                for (int x = gridWidthMinValue; x <= gridWidthMaxValue; x++)
                {
                    Console.Write(x == gridWidthMinValue ? "|" : "|");
                    List<Character> characters = _combatants.FindAll(c => c.Position == new Vector3(x, y, 0));
                    string charsymbolsOnCoords = GetSymbolForCharacter(characters);
                    // if (charsymbolsOnCoords != "")
                        //Console.WriteLine("Char at : {0},{1},{2}", y, x, 0);

                    Console.Write("{0,4}", charsymbolsOnCoords != "" ? charsymbolsOnCoords : "    ");
                    Console.Write("|");
                }
                Console.WriteLine();
                for (int i = gridWidthMinValue; i <= gridWidthMaxValue; i++)
                {
                    Console.Write(i == gridWidthMinValue ? "   └────┘" : "└────┘");
                }
                Console.WriteLine();
            }
        }

        private string GetSymbolForCharacter(List<Character> characters)
        {
            string symbol = "";
            // Return different symbols based on character type
            characters.ForEach(c =>
            {
                if (c is Player)
                {
                    symbol += "P";
                }
                if (c is Monster)
                {
                    symbol += "M";
                }
                if (c is Merchant)
                {
                    symbol += "!";
                }
            });
            return symbol;
        }
    }
}

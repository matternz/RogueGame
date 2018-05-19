using RLNET;
using RogueGame.Interfaces;
using RogueSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueGame.Core
{
    public class Actor : IActor, IDrawable
    {
        // IActor
        public string Name { get; set; }
        public int Awareness { get; set; }

        // IDrawable
        public RLColor Color { get; set; }
        public char Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public void Draw( RLConsole console, IMap map)
        {
            // Don't draw actors in unexplored cells
            if( !map.GetCell( X, Y).IsExplored)
            {
                return;
            }

            // only draw actor with color and symbol when they are in fov
            if (map.IsInFov( X, Y))
            {
                console.Set(X, Y, Color, Colors.FloorBackgroundFov, Symbol);
            }
            else
            {
                // When not in fov draw normal floor
                console.Set(X, Y, Colors.Floor, Colors.FloorBackground, '.');
            }
        }


    }
}

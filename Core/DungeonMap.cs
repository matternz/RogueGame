using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueSharp;
using RLNET;

namespace RogueGame.Core
{
    // dungeon extends map
    public class DungeonMap : Map
    {
        //renders all symbols/colors for each cell
        public void draw( RLConsole mapConsole)
        {
            foreach(Cell cell in GetAllCells())
            {
                SetConsoleSymbolForCell(mapConsole, cell);
            }
        }

        private void SetConsoleSymbolForCell(RLConsole mapConsole, Cell cell)
        {
            // don't draw unexplored cells
            if (!cell.IsExplored)
            {
                return;
            }
            // when the cell is in the current fov draw with lighter colors 
            if( IsInFov( cell.X, cell.Y))
            {
                // '.' for floor '#' for walls
                if (cell.IsWalkable)
                {
                    mapConsole.Set(cell.X, cell.Y, Colors.FloorFov, Colors.FloorBackgroundFov, '.');
                }
                else
                {
                    mapConsole.Set(cell.X, cell.Y, Colors.Floor, Colors.FloorBackground, '#');
                }
            }
            // when a cell is out of the current fov draw with darker colors
            else
            {
                if (cell.IsWalkable)
                {
                    mapConsole.Set(cell.X, cell.Y, Colors.Floor, Colors.FloorBackground, '.');
                }
                else
                {
                    mapConsole.Set(cell.X, cell.Y, Colors.Wall, Colors.WallBackground, '#');
                }
            }

        }

        public void UpdatePlayerFieldOfView()
        {
            Player player = Game.Player;
            // Compute the fov based on players location and awareness
            ComputeFov(player.X, player.Y, player.Awareness, true);
            // Mark all celsl in fov as explored
            foreach( Cell cell in GetAllCells())
            {
                if (IsInFov(cell.X, cell.Y))
                {
                    SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                }
            }
        }
    }
}

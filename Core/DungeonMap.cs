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

        public List<Rectangle> Rooms;

        public DungeonMap()
        {
            Rooms = new List<Rectangle>();
        }

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

        // Returns true when able to place actor on the cell else false
        public bool SetActorPosition( Actor actor, int x, int y)
        {
            // Onlt allow actor placement if cell is walkable
            if ( GetCell( x, y).IsWalkable)
            {
                // Previous cell actor was on is now walkable
                SetIsWalkable(actor.X, actor.Y, true);
                // Update actors position
                actor.X = x;
                actor.Y = y;
                // Cell actor is on is no longer walkable
                SetIsWalkable(actor.X, actor.Y, false);
                // update fov if player has moved
                if ( actor is Player)
                {
                    UpdatePlayerFieldOfView();
                }
                return true;
            }
            return false;
        }

        public void SetIsWalkable( int x, int y, bool isWalkable)
        {
            Cell cell = GetCell(x, y);
            SetCellProperties(cell.X, cell.Y, cell.IsTransparent, isWalkable, cell.IsExplored);
        }

        // called by MapGenerator after new map is created and adds player to the map
        public void AddPlayer(Player player)
        {
            Game.Player = player;
            SetIsWalkable(player.X, player.Y, false);
            UpdatePlayerFieldOfView();
        }
    }
}

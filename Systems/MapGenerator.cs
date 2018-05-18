using RogueGame.Core;
using RogueSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueGame.Systems
{
    public class MapGenerator
    {
        private readonly int _width;
        private readonly int _height;

        private readonly DungeonMap _map;

        // creates new map generator takes in width and height of the map to be created
        public MapGenerator( int width, int height)
        {
            _width = width;
            _height = height;
            _map = new DungeonMap();
        }

        // generates new open floor map with wals around outside
        public DungeonMap CreateMap()
        {
            // intialize every cell by setting walkable, transparency, and explored to true
            _map.Initialize(_width, _height);
            foreach(Cell cell in _map.GetAllCells())
            {
                _map.SetCellProperties(cell.X, cell.Y, true, true, true);
            }

            // set first and last row to not be transparent or walkable
            foreach (Cell cell in _map.GetCellsInRows(0, _height - 1))
            {
                _map.SetCellProperties(cell.X, cell.Y, false, false, true);
            }

            // set first and last column to not be transparent or walkable
            foreach(Cell cell in _map.GetCellsInColumns(0, _width - 1))
            {
                _map.SetCellProperties(cell.X, cell.Y, false, false, true);
            }

            return _map;
        }
    }
}

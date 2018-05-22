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
        private readonly int _maxRooms;
        private readonly int _roomMaxSize;
        private readonly int _roomMinSize;

        private readonly DungeonMap _map;

        // creates new map generator takes in width and height of the map to be created
        public MapGenerator( int width, int height, int maxRooms, int roomMaxSize, int roomMinSize)
        {
            _width = width;
            _height = height;
            _map = new DungeonMap();
            _maxRooms = maxRooms;
            _roomMaxSize = roomMaxSize;
            _roomMinSize = roomMinSize;
        }

        // generates new open floor map with wals around outside
        public DungeonMap CreateMap()
        {
            // intialize every cell by setting walkable, transparency, and explored to true
            _map.Initialize(_width, _height);

            for(int r = _maxRooms; r > 0; r--)
            {
                int roomWidth = Game.Random.Next(_roomMinSize, _roomMaxSize);
                int roomHeight = Game.Random.Next(_roomMinSize, _roomMaxSize);
                int roomXPostion = Game.Random.Next(0, _width - roomWidth - 1);
                int roomYPostion = Game.Random.Next(0, _height - roomHeight - 1);

                var newRoom = new Rectangle(roomXPostion, roomYPostion, roomWidth, roomHeight);

                bool newRoomIntersects = _map.Rooms.Any(room => newRoom.Intersects(room));

                if (!newRoomIntersects)
                {
                    _map.Rooms.Add(newRoom);
                }
            }

            foreach( Rectangle room in _map.Rooms)
            {
                CreateRoom(room);
            }

            return _map;
        }

        // given Rectangle on map set cell properties to true
        private void CreateRoom(Rectangle room)
        {
            for(int x = room.Left + 1; x < room.Right; x++)
            {
                for(int y = room.Top + 1; y < room.Bottom; y++)
                {
                    _map.SetCellProperties(x, y, true, true, true);
                }
            }
        }
    }
}

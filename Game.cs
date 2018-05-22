using RLNET;
using RogueGame.Core;
using RogueGame.Systems;
using RogueSharp.Random;
using System;

namespace RogueGame
{
    public class Game
    {
        // Screen console width/height measured in number of tiles
        private static readonly int _screenWidth = 100;
        private static readonly int _screenHeight = 70;
        private static RLRootConsole _rootConsole;

        // Map console width/height measured in number of tiles
        private static readonly int _mapWidth = 80;
        private static readonly int _mapHeight = 48;
        private static RLConsole _mapConsole;

        // Message console below map width/height measured in number of tiles.
        private static readonly int _messageWidth = 80;
        private static readonly int _messageHeight = 11;
        private static RLConsole _messageConsole;

        // Stat console right of map width/height measured in number of tiles. 
        private static readonly int _statWidth = 20;
        private static readonly int _statHeight = 70;
        private static RLConsole _statConsole;

        // Inventory console above map width/height measured in number of tiles. 
        private static readonly int _inventoryWidth = 80;
        private static readonly int _inventoryHeight = 11;
        private static RLConsole _inventoryConsole;

        public static DungeonMap DungeonMap { get; private set; }

        public static Player Player { get; private set; }

        private static bool _renderRequired = true;
        
        public static CommandSystem CommandSystem { get; private set; }

        // Singleton of IRandom used to generate random numbers
        public static IRandom Random { get; private set; }

        public static void Main(string[] args)
        {
            int seed = (int) DateTime.UtcNow.Ticks;
            Random = new DotNetRandom(seed);

            string consoleTitle = "RogueSharp Game";

            string fontFileName = "terminal8x8.png";
            string windowName = "Rogue Game";

            _rootConsole = new RLRootConsole(fontFileName, _screenWidth, _screenHeight, 8, 8, 1f, windowName);

            _mapConsole = new RLConsole(_mapWidth, _screenHeight);
            _messageConsole = new RLConsole(_messageWidth, _messageHeight);
            _statConsole = new RLConsole(_statWidth, _statHeight);
            _inventoryConsole = new RLConsole(_inventoryWidth, _inventoryHeight);

            Player = new Player();

            MapGenerator mapGenerator = new MapGenerator(_mapWidth, _mapHeight, 20, 13, 7);
            DungeonMap = mapGenerator.CreateMap();
            DungeonMap.UpdatePlayerFieldOfView();

            CommandSystem = new CommandSystem();

            // Set up a handler for RLNet's Update event
            _rootConsole.Update += OnRootConsoleUpdate;

            // Set up a handler for RLNet's Render event
            _rootConsole.Render += OnRootConsoleRender;

            _messageConsole.SetBackColor(0, 0, _messageWidth, _messageHeight, Swatch.DbDeepWater);
            _messageConsole.Print(1, 1, "Messages", Colors.TextHeading);

            _statConsole.SetBackColor(0, 0, _statWidth, _statHeight, Swatch.DbOldStone);
            _statConsole.Print(1, 1, "Stats", Colors.TextHeading);

            _inventoryConsole.SetBackColor(0, 0, _inventoryWidth, _inventoryHeight, Swatch.DbWood);
            _inventoryConsole.Print(1, 1, "Invetory", Colors.TextHeading);

            // Starts RLNET's game loop
            _rootConsole.Run();
        }

        // Event handler for RLNet's Update event
        private static void OnRootConsoleUpdate(object sender, UpdateEventArgs e)
        {
            bool didPlayerAct = false;
            RLKeyPress keyPress = _rootConsole.Keyboard.GetKeyPress();

            if( keyPress != null)
            {
                switch (keyPress.Key)
                {
                    case (RLKey.Up):
                        {
                            didPlayerAct = CommandSystem.MovePlayer(Direction.Up);
                            break;
                        }
                    case (RLKey.Down):
                        {
                            didPlayerAct = CommandSystem.MovePlayer(Direction.Down);
                            break;
                        }
                    case (RLKey.Left):
                        {
                            didPlayerAct = CommandSystem.MovePlayer(Direction.Left);
                            break;
                        }
                    case (RLKey.Right):
                        {
                            didPlayerAct = CommandSystem.MovePlayer(Direction.Right);
                            break;
                        }
                    case (RLKey.Escape):
                        {
                            _rootConsole.Close();
                            break;
                        }
                }
            }

            if (didPlayerAct)
            {
                _renderRequired = true;
            }
        }

        private static void OnRootConsoleRender(object sender, UpdateEventArgs e)
        {
            if (_renderRequired)
            {
                DungeonMap.draw(_mapConsole);
                Player.Draw(_mapConsole, DungeonMap);
              
                // Blits other consoles to root console
                RLConsole.Blit(_mapConsole, 0, 0, _mapWidth, _mapHeight, _rootConsole, 0, _inventoryHeight);
                RLConsole.Blit(_statConsole, 0, 0, _statWidth, _statHeight, _rootConsole, _mapWidth, 0);
                RLConsole.Blit(_messageConsole, 0, 0, _messageWidth, _messageHeight, _rootConsole, 0, _screenHeight - _messageHeight);
                RLConsole.Blit(_inventoryConsole, 0, 0, _inventoryWidth, _inventoryHeight, _rootConsole, 0, 0);

                _rootConsole.Draw();

                _renderRequired = false;
            }
        }

    }
}

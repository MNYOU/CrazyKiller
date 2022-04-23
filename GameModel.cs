using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using СrazyKiller;

namespace CrazyKiller
{
    public class GameModel
    {
        public readonly Player player;
        public readonly MapCell[,] map;
        public readonly Point startPosition;
        public Size Size;

        public GameModel(Player player)
        {
            this.player = player;
            Size = new Size(37, 19);
            map = new MapCell[Size.Height, Size.Width];
            GenerateMap();
            startPosition =  player.Position;
        }

        private void GenerateMap() // игрок может появиться в стене
        {
            var rdn = new Random();
            for (var i = 0; i < Size.Height; i++)
            {
                for (var j = 0; j < Size.Width; j++)
                {
                    var number = rdn.Next(0, 4);
                    if (number == 3 && rdn.Next(0, 4) == 1)
                        map[i, j] = MapCell.Wall;
                    else
                    {
                        number = rdn.Next(0, 3);
                        map[i, j] = (MapCell)number;
                    }
                }
            }

            var percent = map.Cast<MapCell>().Count(x => x == MapCell.Wall) * 100.0 / (map.GetLength(0) * map.GetLength(1));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using СrazyKiller;

namespace CrazyKiller
{

    public class Player : IPerson
    {
        private readonly int speed;
        public Point Position { get; private set; }

        public Player(int speed, Point position)
        {
            this.speed = speed;
            Position = position;
        }

        public void Move(object sender, KeyEventArgs e, MapCell[,] map)
        {
            var pressedKey = e.KeyCode;
            var form = (GameForm)sender;
            switch (pressedKey)
            {
                case Keys.W:
                    MoveIfCan(map, form, new Point(Position.X, Position.Y - speed));
                    break;
                case Keys.S:
                    MoveIfCan(map, form, new Point(Position.X, Position.Y + speed));
                    break;
                case Keys.A:
                    MoveIfCan(map, form, new Point(Position.X - speed, Position.Y));
                    break;
                case Keys.D:
                    MoveIfCan(map, form, new Point(Position.X + speed, Position.Y));
                    break;
            }
        }

        private void MoveIfCan(MapCell[,] map, GameForm form, Point destination)
        {
            // если вода, то замедлить игрока
            // также использовать для врагов
            if (
                destination.X + form.panelSize.Width <= form.ClientSize.Width &&
                destination.X >= 0 &&
                destination.Y + form.panelSize.Height <= form.ClientSize.Height &&
                destination.Y >= 0 &&
                !IsInsideWall(map, form, destination)
                )
                Position = destination;
        }

        private static bool IsInsideWall(MapCell[,] map, GameForm form, Point destination)
        {
            var i = destination.X / form.panelSize.Width;
            var j = destination.Y / form.panelSize.Height;
            var points = new[] // границы иконки
            {
                new Point(i, j),
                new Point(i + 1, j),
                new Point(i, j + 1),
                new Point(i + 1, j + 1)
            };

            if (destination.Y % form.panelSize.Height == 0)
            {
                points[2] = new Point(i, j);
                points[3] = new Point(i + 1, j);
            }
            if (destination.X % form.panelSize.Width == 0)
            {
                points[1] = new Point(i, j);
                points[3] = new Point(i, j);
            }

            return points.Select(point => map[point.Y, point.X]).Any(cell => cell == MapCell.Wall);
        }

        public string GetImageFileName()
        {
            throw new NotImplementedException();
        }
    }
}

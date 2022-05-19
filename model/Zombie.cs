using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyKiller
{
    public class Zombie : IObjectInMap
    {
        public int Hp;
        public bool IsPenetration;
        public Point Position { get; private set; }
        public Point PreviousPosition { get; private set; }
        private static Size size;

        public Size Size
        {
            get => size;
            set => size = value;
        }

        public int Speed { get; }
        public int Damage { get; }
        public Random rnd = GameModel.rnd;

        public Zombie()
        {
            Hp = 100;
            Speed = 2;
            Damage = 1;
            GeneratePosition();
        }

        public void GeneratePosition()
        {
            var distance = 400;
            var x = rnd.Next(GameModel.WindowSize.Width);
            var y = rnd.Next(GameModel.WindowSize.Height);
            switch (rnd.Next(3))
            {
                case 0:
                    x = rnd.Next(2) == 0
                        ? rnd.Next(-distance, -Size.Width)
                        : rnd.Next(GameModel.WindowSize.Width + distance,
                            GameModel.WindowSize.Width + distance * 2);
                    break;
                case 1:
                    y = rnd.Next(2) == 0
                        ? rnd.Next(-distance, -Size.Height)
                        : rnd.Next(GameModel.WindowSize.Height + distance,
                            GameModel.WindowSize.Height + distance * 2);
                    break;
                case 2:
                    x = rnd.Next(2) == 0
                        ? rnd.Next(-distance, -Size.Width)
                        : rnd.Next(GameModel.WindowSize.Width + distance,
                            GameModel.WindowSize.Width + distance * 2);
                    y = rnd.Next(2) == 0
                        ? rnd.Next(-distance, -Size.Height)
                        : rnd.Next(GameModel.WindowSize.Height + distance,
                            GameModel.WindowSize.Height + distance * 2);
                    break;
            }

            Position = new Point(x, y);
        }

        public void Move(Player player)
        {
            var playerPos = player.Position;
            PreviousPosition = Position;
            if (GameModel.IsInteract(player, this)) return;
            var offset = new Point(playerPos.X - Position.X, playerPos.Y - Position.Y);
            var distance = Math.Sqrt(offset.X * offset.X + offset.Y * offset.Y);
            if (distance == 0) return;
            var nextPoint = new Point((int) (Position.X + offset.X / distance * Speed),
                (int) (Position.Y + offset.Y / distance * Speed));
            // if (GameModel.CanMove(nextPoint, this.Size))
            Position = nextPoint;
        }
    }
}
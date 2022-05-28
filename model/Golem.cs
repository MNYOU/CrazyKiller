using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrazyKiller
{
    public class Golem : IObjectInMap
    {
        private static Size size;
        public int Hp;
        public bool IsPenetration;
        public bool IsAttack;

        public Size Size
        {
            get => size;
            set => size = value;
        }

        private int Speed { get; }
        public int Damage { get; }
        public bool IsDead => Hp <= 0;
        public Point Position { get; private set; }
        public Point PreviousPosition { get; private set; }

        public Golem()
        {
            Hp = 100;
            Speed = 5;
            Damage = 1;
            GeneratePosition();
        }

        private void GeneratePosition()
        {
            var rnd = GameModel.Random;
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
            PreviousPosition = Position;
            var playerPos = player.Position;
            if (GameModel.IsInteract(player, this)) return;
            var offset = new Point(playerPos.X - Position.X, playerPos.Y - Position.Y);
            var distance = Math.Sqrt(offset.X * offset.X + offset.Y * offset.Y);
            if (distance == 0) return;
            var absOffset = new Point((int) Math.Abs(offset.X / distance * Speed),
                (int) Math.Abs(offset.Y / distance * Speed));
            Position = new Point(Position.X + absOffset.X * Math.Sign(offset.X),
                Position.Y + absOffset.Y * Math.Sign(offset.Y));
        }
    }
}
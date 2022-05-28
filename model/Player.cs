using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrazyKiller;

namespace CrazyKiller
{
    public class Player : IObjectInMap
    {
        public Point MousePosition;
        public bool IsAttacked;
        private readonly Dictionary<Keys, bool> keysClicks;
        private int Speed { get; }
        public Point Position { get; private set; }
        public Point PreviousPosition { get; private set; }
        public Size Size { get; set; }
        private Size Offset { get; set; }
        public int MaxHp { get; }
        public int Hp { get; private set; }
        public Gun Gun { get; set; }

        public Player()
        {
            Speed = 5;
            Position = new Point(GameModel.WindowSize.Width / 2, GameModel.WindowSize.Height / 2);
            PreviousPosition = Position;
            MaxHp = 1000;
            Hp = MaxHp;
            Gun = new Pistol(this);
            keysClicks = new Dictionary<Keys, bool>
            {
                {Keys.W, false},
                {Keys.S, false},
                {Keys.A, false},
                {Keys.D, false}
            };
        }



        public void Move()
        {
            Offset = new Size(keysClicks[Keys.D] ? Speed : 0 - (keysClicks[Keys.A] ? Speed : 0),
                keysClicks[Keys.S] ? Speed : 0 - (keysClicks[Keys.W] ? Speed : 0));
            PreviousPosition = Position;
            var distance = Math.Sqrt(Offset.Width * Offset.Width + Offset.Height * Offset.Height);
            if (distance == 0) return;
            var nextPoint = new Point(Position.X + (int) (Offset.Width / distance * Speed),
                Position.Y + (int) (Offset.Height / distance * Speed));
            if (GameModel.CanMove(nextPoint, Size))
                Position = nextPoint;
        }

        public void AddOffset(Keys key)
        {
            ChangeOffset(key, true);
        }

        public void RemoveOffset(Keys key)
        {
            ChangeOffset(key, false);
        }

        private void ChangeOffset(Keys key, bool click)
        {
            if (!keysClicks.ContainsKey(key)) throw new NullReferenceException();
            if (click != keysClicks[key])
                keysClicks[key] = !keysClicks[key];
        }

        public void AddHeath(int heath)
        {
            Hp += heath;
            if (Hp > MaxHp) Hp = MaxHp;
            else if (Hp < 0) Hp = 0;
        }
    }
}
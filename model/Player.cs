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
        public Player()
        {
            Speed = 5;
            Position = new Point(700, 400);
            MaxHp = 100;
            Hp = MaxHp;
            Gun = new Pistol(this);
        }

        public Point MousePosition;
        private int Speed { get; }
        public Point Position { get; set; }
        public Point PreviousPosition { get; private set; }
        public Size Size { get; set; }
        public Size Offset { get; private set; }
        public int MaxHp { get; }
        public int Hp { get; private set; }

        public Gun Gun { get; set; }
        public void Move()
        {
            PreviousPosition = Position;
            var distance = Math.Sqrt(Offset.Width * Offset.Width + Offset.Height * Offset.Height);
            if (distance == 0) return;
            if (Offset.Width != 0 && Offset.Height != 0)
            {

            }
            var nextPoint = new Point(Position.X + (int)(Offset.Width / distance * Speed), Position.Y + (int)(Offset.Height / distance * Speed));
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

        private void ChangeOffset(Keys key, bool add)
        {
            switch (key)
            {
                case Keys.W:
                    if (Offset.Height != -Speed && add || Offset.Height == -Speed && !add)
                        Offset = new Size(Offset.Width, Offset.Height - Speed * (add ? 1 : -1));
                    break;
                case Keys.S:
                    if (Offset.Height != Speed && add || Offset.Height == Speed && !add)
                        Offset = new Size(Offset.Width, Offset.Height + Speed * (add ? 1 : -1));
                    break;
                case Keys.A:
                    if (Offset.Width != -Speed && add || Offset.Width == -Speed && !add)
                        Offset = new Size(Offset.Width - Speed * (add ? 1 : -1), Offset.Height);
                    break;
                case Keys.D:
                    if (Offset.Width != Speed && add || Offset.Width == Speed && !add)
                        Offset = new Size(Offset.Width + Speed * (add ? 1 : -1), Offset.Height);
                    break;
            }
        }


        public void AddHeath(int heath)
        {
            Hp += heath;
            if (Hp > MaxHp) Hp = MaxHp;
            else if (Hp < 0) Hp = 0;
        }
    }
}

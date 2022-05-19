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
    public class GameModel
    {
        public readonly Player Player;
        public Size Size;
        public static Size WindowSize;
        public Timer Timer;
        public bool IsPause;
        public MedKit MedKit;
        public Box Box;
        public List<Zombie> Zombies;
        public static Random rnd = new Random();
        public Point MousePosition;

        public GameModel()
        {
            Player = new Player();
            InitializeTimer();
            Zombies = new List<Zombie>();
            for (var i = 0; i < 20; i++)
                Zombies.Add(new Zombie());
            MedKit = new MedKit();
            Box = new Box();
        }

        public void Start()
        {
            Timer.Start();
        }

        private void InitializeTimer()
        {
            Timer = new Timer();
            Timer.Interval = 16;
            Timer.Tick += TimerOnTick;
        }

        private void TimerOnTick(object sender, EventArgs e)
        {
            if (IsPause) return;

            if (Zombies.Count == 0 || Player.Hp == 0)
                Application.Exit();

            Player.Move();
            foreach (var zombie in Zombies)
            {
                zombie.Move(Player);
                if (IsInteract(zombie, Player)) Player.AddHeath(-zombie.Damage);
            }

            Player.Gun.Shoot(Zombies);
            MedKit.Interact(Player);
            Box.Interact(Player);
        }

        public static bool IsInteract(object one, object two)
        {
            if (!(one is IObjectInMap && two is IObjectInMap))
                throw new InvalidCastException();
            var first = (IObjectInMap) one;
            var second = (IObjectInMap) two;
            return Math.Abs(second.Position.X - first.Position.X) <= (first.Size.Width + second.Size.Width) / 2 &&
                   Math.Abs(second.Position.Y - first.Position.Y) <= (first.Size.Height + second.Size.Height) / 2;
        }

        public static bool CanMove(Point destination, Size size)
        {
            if (destination.X - size.Width / 2 > WindowSize.Width ||
                destination.X - size.Width / 2 < 0 ||
                destination.Y + size.Height / 2 > WindowSize.Height ||
                destination.Y - size.Height / 2 < 0) 
                return false;
            return true;
        }

        public bool IsOccupied() // чтобы зомби не могли ходить в себя
        {
            return false;
        }
    }
}
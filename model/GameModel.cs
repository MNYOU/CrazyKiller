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
        public List<Golem> Zombies;
        public Level[] levels;
        public Level currentLevel;
        public MedKit MedKit;
        public Box Box;
        public Size Size;
        public static Size WindowSize;
        public Timer Timer;
        public bool IsPause;
        private int counter;
        public bool IsOver { get; private set; }
        public bool IsWon { get; private set; }
        public bool IsStarted { get; private set; }
        public static Random rnd = new Random();
        public Point MousePosition;

        public GameModel()
        {
            Player = new Player();
            InitializeTimer();
            Zombies = new List<Golem>();
            MedKit = new MedKit();
            Box = new Box();
            levels = new Level[9].Select((x, i) => new Level(i + 1)).ToArray();
            currentLevel = levels?.FirstOrDefault();
            InitializeZombies();
        }

        private void InitializeZombies()
        {
            for (var i = 0; i < currentLevel?.CountZombies; i++)
            // for (var i = 0; i < 1; i++)
                Zombies.Add(new Golem());
        }

        public void Start()
        {
            Timer.Start();
            IsStarted = true;
        }

        private void InitializeTimer()
        {
            Timer = new Timer();
            Timer.Interval = 45;
            Timer.Tick += TimerOnTick;
        }

        private void TimerOnTick(object sender, EventArgs e)
        {
            if (IsPause) return;

            Player.Move();
            Player.IsAttacked = false;
            foreach (var zombie in Zombies)
            {
                zombie.IsAttack = false;
                // zombie.IsPenetration = false;
                zombie.Move(Player);
                if (IsInteract(zombie, Player))
                {
                    zombie.IsAttack = true;
                    Player.AddHeath(-zombie.Damage);
                    Player.IsAttacked = true;
                }
            }

            Player.Gun.Shoot(Zombies);
            MedKit.Interact(Player);
            Box.Interact(Player);

            ChangeLevel();

            if (Player.Hp == 0) IsOver = true;
        }

        private void ChangeLevel()
        {
            if (Zombies.Count == 0)
            {
                if (levels is null || levels.Length == 0 || currentLevel.Number == levels.Length)
                {
                    IsOver = true;
                    IsWon = true;
                    return;
                }

                counter++;
                if (counter < 100) return;
                currentLevel = levels[currentLevel.Number];
                InitializeZombies();
                counter = 0;
            }
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
    }
}
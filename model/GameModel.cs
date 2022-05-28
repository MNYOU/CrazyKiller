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
        public static Size WindowSize;
        public static readonly Random Random = new Random();
        public readonly Player Player;
        public readonly List<Golem> Golems;
        private readonly Level[] levels;
        public readonly MedKit MedKit;
        public readonly Box Box;
        public Timer Timer;
        public bool IsPause;
        private int counter;
        public Level CurrentLevel { get; private set; }
        public bool IsOver { get; private set; }
        public bool IsWon { get; private set; }
        public bool IsStarted { get; private set; }

        public GameModel()
        {
            Player = new Player();
            InitializeTimer();
            Golems = new List<Golem>();
            MedKit = new MedKit();
            Box = new Box();
            levels = new Level[9].Select((x, i) => new Level(i + 1)).ToArray();
            CurrentLevel = levels?.FirstOrDefault();
            InitializeZombies();
        }

        private void InitializeZombies()
        {
            for (var i = 0; i < CurrentLevel?.CountZombies; i++)
                Golems.Add(new Golem());
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
            foreach (var zombie in Golems)
            {
                zombie.IsAttack = false;
                zombie.Move(Player);
                if (IsInteract(zombie, Player))
                {
                    zombie.IsAttack = true;
                    Player.AddHeath(-zombie.Damage);
                    Player.IsAttacked = true;
                }
            }

            Player.Gun.Shoot(Golems);
            MedKit.Interact(Player);
            Box.Interact(Player);

            if (Golems.Count == 0) ChangeLevel();
            if (Player.Hp == 0) IsOver = true;
        }

        private void ChangeLevel()
        {
            if (levels is null || levels.Length == 0 || CurrentLevel.Number == levels.Length)
            {
                IsOver = true;
                IsWon = true;
                return;
            }

            counter++;
            if (counter < 100) return;
            CurrentLevel = levels[CurrentLevel.Number];
            InitializeZombies();
            counter = 0;
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
            return destination.X - size.Width / 2 <= WindowSize.Width && destination.X - size.Width / 2 >= 0 &&
                   destination.Y + size.Height / 2 <= WindowSize.Height && destination.Y - size.Height / 2 >= 0;
        }
    }
}
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CrazyKiller
{
    public sealed class GameForm : Form
    {
        private GameModel game;
        private Player player;
        public View View;
        public Control Control;
        public Timer Timer;
        public readonly List<Button> buttons;

        public GameForm()
        {
            InitializeComponent();
            GameModel.WindowSize = Size;
            // GameModel.WindowSize = Size;
            game = new GameModel();
            player = game.Player;
            buttons = new List<Button>();
            View = new View(this, game);
            Control = new Control(this, game, View.Sounds);
            InitializeButtons();
            Timer = game.Timer;
            Timer.Tick += (sender, args) => Invalidate();

            View.AddStartMenu();
        }

        private void InitializeButtons()
        {
            View.AddButtonInForm(Point.Empty, "Начать игру", "Start");
            View.AddButtonInForm(Point.Empty, "Выйти", "Exit");
            View.AddButtonInForm(Point.Empty, "Продолжить", "Pause");
            Control.InitializeButtonsClicks();
        }

        public void ChangeButtonPositionAndStatus(string controlName, Point position = new Point())
        {
            var button = buttons.FirstOrDefault(x => x.Name == controlName);
            if (button is null) return;
            button.Location = PointMethods.GetOffsetPosition(position, button.Size);
            if (Controls.Contains(button)) Controls.Remove(button);
            else Controls.Add(button);
        }

        private void InitializeComponent()
        {
            BackColor = Color.DarkSlateGray;
            Size = Screen.PrimaryScreen.Bounds.Size;
            DoubleBuffered = true;
            MinimumSize = new Size(600, 600);
            Name = "GameForm";
            Text = "CrazyKiller";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None; //полный экран
        }
    }
}
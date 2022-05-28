using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CrazyKiller
{
    public sealed class GameForm : Form
    {
        private readonly Control control;
        public readonly List<Button> Buttons;
        public View View { get; }

        public GameForm()
        {
            InitializeComponent();
            GameModel.WindowSize = Size;
            var game = new GameModel();
            Buttons = new List<Button>();
            View = new View(this, game);
            control = new Control(this, game, View.Sounds);
            InitializeButtons();
            var timer = game.Timer;
            timer.Tick += (sender, args) => Invalidate();

            View.AddStartMenu();
        }

        private void InitializeButtons()
        {
            View.AddButtonInForm(Point.Empty, "Начать игру", "Start");
            View.AddButtonInForm(Point.Empty, "Выйти", "Exit");
            View.AddButtonInForm(Point.Empty, "Продолжить", "Pause");
            control.InitializeButtonsClicks();
        }

        public void ChangeButtonPositionAndStatus(string controlName, Point position = new Point())
        {
            var button = Buttons.FirstOrDefault(x => x.Name == controlName);
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
            FormBorderStyle = FormBorderStyle.None;
        }
    }
}
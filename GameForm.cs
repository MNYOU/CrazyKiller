using System.Drawing;
using System.Windows.Forms;
using CrazyKiller;

namespace CrazyKiller
{
    public sealed partial class GameForm : Form
    {
        private GameModel game;
        private Player player;
        public View View;
        public Control Control;
        public Timer Timer;
        public int elapsedTime;
        public GameForm()
        {
            InitializeComponent();
            GameModel.WindowSize = ClientSize;
            // GameModel.WindowSize = Size;
            this.game = new GameModel();
            player = game.Player;
            View = new View(this, game);
            Control = new Control(this, game);
            Timer = game.Timer;
            Timer.Tick += (sender, args) => Invalidate();

            // var beforeTimer = new Timer();
            // beforeTimer.Interval = 1000;
            // beforeTimer.Tick += (sender, args) =>
            // {
            //     if (elapsedTime == 5)
            //     {
            //         beforeTimer.Stop();
            //         View.AddStartMenu();
            //     }
            //
            //     elapsedTime++;
            // };
            // beforeTimer.Start();

            View.AddStartMenu();
        }




        private void InitializeComponent()
        {
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized; // полноэкранный режим
            Text = "CrazyKiller";
            Size = Screen.PrimaryScreen.Bounds.Size;
            Dock = DockStyle.Fill;
            MinimumSize = new Size(600, 600);
            // Icon = Icon.ExtractAssociatedIcon("C:/Programs/C#/СrazyKiller/images/icon.ico");
            Size = new Size(1500, 800);
            BackColor = Color.DarkSlateGray;
            DoubleBuffered = true;
        }
    }
}

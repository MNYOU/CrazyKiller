using System.Drawing;
using System.Windows.Forms;
using CrazyKiller;

namespace CrazyKiller
{
    public sealed class GameForm : Form
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
            game = new GameModel();
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
            // this.SuspendLayout();
            this.BackColor = System.Drawing.Color.DarkSlateGray;
            // this.ClientSize = new System.Drawing.Size(1567, 619);
            Size = Screen.PrimaryScreen.Bounds.Size;
            this.DoubleBuffered = true;
            MinimumSize = new Size(600, 600);
            Name = "GameForm";
            Text = "CrazyKiller";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            // this.ResumeLayout(false);

        }
    }
}

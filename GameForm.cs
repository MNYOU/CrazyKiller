using System.Drawing;
using System.Windows.Forms;
using СrazyKiller;

namespace CrazyKiller
{
    public sealed partial class GameForm : Form
    {
        GameModel game;
        private Player player;
        public readonly Size panelSize = new Size(40, 40);
        TableLayoutPanel table;
        Panel panelPlayer;
        public GameForm(GameModel game)
        {
            this.game = game;
            BackColor = Color.DarkSlateGray;
            DoubleBuffered = true;
            player = game.player;
            Dock = DockStyle.Fill;
            MinimumSize = new Size(600, 600);
            Icon = Icon.ExtractAssociatedIcon("C:/Programs/C#/СrazyKiller/images/icon.ico");
            Size = new Size(1500, 800);
            StartPosition = FormStartPosition.CenterScreen;


            KeyDown += GameForm_KeyDown;



            table = new TableLayoutPanel();
            for (var i = 0; i < game.Size.Height; i++)
            {
                table.RowStyles.Add(new RowStyle(SizeType.Absolute, panelSize.Height));
            }
            for (var i = 0; i < game.Size.Width; i++)
            {
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, panelSize.Width));
            }


            for (var i = 0; i < game.Size.Height; i++)
            {
                for (var j = 0; j < game.Size.Width; j++)
                {
                    var panel = new Panel();
                    var pair = GetColorWithImage(game.map, j, i);
                    panel.BackgroundImage = pair.Item2;
                    panel.Margin = Padding.Empty;
                    table.Controls.Add(panel, j, i);
                }
            }

            AddPlayerInForm();

            table.Size = new Size(panelSize.Width * game.Size.Width, panelSize.Height * game.Size.Height);
            table.BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(table);

            // var somePanel = new Panel();
            // somePanel.Margin = Padding.Empty;
            // somePanel.Size = panelSize;
            // somePanel.BackColor = Color.Black;
            // somePanel.Location = new Point(ClientSize.Width - panelSize.Height, ClientSize.Height - panelSize.Height);
            // Controls.Add(somePanel);

            //InitializeComponent();
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            player.Move(sender, e, game.map);
            //Invalidate();
            //InitializeComponent();
            InvalidateViews();
        }

        private void InvalidateViews()
        {
            panelPlayer.Location = player.Position;
        }

        private void AddPlayerInForm()
        {
            panelPlayer = new Panel();
            panelPlayer.Size = panelSize;
            panelPlayer.BackgroundImage = Image.FromFile("C:/Programs/C#/СrazyKiller/images/player2.jpg");
            panelPlayer.Margin = Padding.Empty;
            panelPlayer.Location = player.Position;
            Controls.Add(panelPlayer);
        }

        private (Color, Image) GetColorWithImage(MapCell[,] map, int j, int i)
        {
            // вынести общий путь в переменную, а в каждый класс добавить название файла
            // для этого можно реализовать интерфейс
            switch (map[i, j])
            {
                case MapCell.Ground:
                    return (Color.Green, Image.FromFile("C:/Programs/C#/СrazyKiller/images/water.jpg"));
                case MapCell.Stone:
                    return (Color.Gray, Image.FromFile("C:/Programs/C#/СrazyKiller/images/stone.jpg"));
                case MapCell.Water:
                    return (Color.Aqua, Image.FromFile("C:/Programs/C#/СrazyKiller/images/ground.jpg"));
                case MapCell.Wall:
                    return (Color.Red, Image.FromFile("C:/Programs/C#/СrazyKiller/images/wall.png"));
                default:
                    return (Color.Black, Image.FromFile("C:/Programs/C#/СrazyKiller/images/black.png"));
            }
        }

        // private void InitializeComponent()
        // {
        //     System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
        //     this.SuspendLayout();
        //     // 
        //     // GameForm
        //     // 
        //     this.ClientSize = new System.Drawing.Size(982, 753);
        //     this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        //     this.Name = "GameForm";
        //     this.ResumeLayout(false);
        //
        // }
    }
}

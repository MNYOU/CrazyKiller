using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace CrazyKiller
{
    public class View
    {
        private GameForm form;
        private GameModel game;
        private Player player;
        private Graphics graphics;
        private bool IsDownloading;
        private bool IsStarted;
        private bool IsPause;
        private Dictionary<string, Image> images;

        public View(GameForm form, GameModel game)
        {
            this.form = form;
            this.game = game;
            player = game.Player;
            form.Paint += Paint;
            InitializeImages();
            IsDownloading = true;
        }


        private void InitializeImages()
        {
            var path =
                string.Join("\\", AppDomain.CurrentDomain.BaseDirectory.Split('\\').Reverse().Skip(3).Reverse()) +
                "\\images\\";
            form.Icon = Icon.ExtractAssociatedIcon(path + "handgun.ico");
            images = new Dictionary<string, Image>();
            InitializeImage(path, "aimSmall.png", player);
            foreach (var zombie in game.Zombies)
                InitializeImage(path, "aimSmall.png", zombie);
            InitializeImage(path, "aimSmall.png", player.Gun);
            InitializeImage(path, "medkitSmall.png", game.MedKit);
            InitializeImage(path, "boxSmall.png", game.Box, 2, 2);
            InitializeImage(path, "unitySmall.png");
        }

        private void InitializeImage(string path, string fileName, IObjectInMap obj = null, int column = 1, int row = 1)
        {
            var image = Image.FromFile(path + fileName);
            images[fileName.Split('.').First()] = image;
            if (obj != null) 
                obj.Size = new Size(image.Width / column, image.Height / row);
        }

        public void Paint(object sender, PaintEventArgs e)
        {
            graphics = e.Graphics;

            if (IsDownloading)
                Download();
            if (!IsStarted || IsPause) return;
            Player();
            PanelBar();
            Zombies();
            Items();
        }

        public void AddStartMenu()
        {
            IsDownloading = false;
            form.BackColor = Color.DarkGray;
            var start = new Button();
            start.Text = "Начать игру";
            start.Size = new Size(300, 50);
            start.Location = new Point((form.ClientSize.Width - start.Width) / 2,
                form.ClientSize.Height / 2 - start.Height);
            form.Controls.Add(start);

            var end = new Button();
            end.Text = "Выйти";
            end.Size = new Size(300, 50);
            end.Location = new Point((form.ClientSize.Width - start.Width) / 2, form.ClientSize.Height / 2);
            end.Click += (sender, args) => Application.Exit();
            form.Controls.Add(end);

            start.Click += (sender, args) =>
            {
                RemoveStartMenu(new []{start, end});
                game.Start();
                IsStarted = true;
            };
        }

        private void RemoveStartMenu(Button[] buttons)
        {
            foreach (var button in buttons)
                form.Controls.Remove(button);
        }

        public void Download()
        {
            form.BackColor = Color.White;
            var image = images["unitySmall"];
            graphics.DrawImage(image,
                new Point((form.ClientSize.Width - image.Width) / 2, (form.ClientSize.Height - image.Height) / 2));
        }

        public void Pause()
        {
        }

        private void Player()
        {
            var position = GetOffsetPosition(player);
            graphics.DrawRectangle(new Pen(Color.Blue, 5), position.X, position.Y, player.Size.Width,
                player.Size.Height);
            graphics.DrawEllipse(new Pen(Color.Black, 2), new Rectangle(player.Position.X, player.Position.Y, 5, 5));

            var aimPos = GetOffsetPosition(player.Gun);
            graphics.DrawImage(images["aimSmall"], aimPos);
            if (player.Gun.IsShoot) graphics.DrawRectangle(new Pen(Color.Chartreuse, 10), 10,10, 50,50);

        }

        private void Zombies()
        {
            foreach (var zombie in game.Zombies)
            {
                var position = GetOffsetPosition(zombie);
                graphics.DrawRectangle(new Pen(Color.Red, 5), position.X, position.Y, zombie.Size.Width, zombie.Size.Height);
                graphics.DrawEllipse(new Pen(Color.Black, 2), new Rectangle(zombie.Position.X, zombie.Position.Y, 5,5));
            }
        }

        private void PanelBar()
        {
        }

        private void Items()
        {
            if (game.MedKit.IsActive)
                graphics.DrawImage(images["medkitSmall"], GetOffsetPosition(game.MedKit));

            if (game.Box.IsActive)
            {
                var box = game.Box;
                var position = GetOffsetPosition(box);
                var rectangle = new Rectangle(0, 0, box.Size.Width, box.Size.Height);
                graphics.DrawImage(images["boxSmall"], position.X, position.Y, rectangle, GraphicsUnit.Pixel);
            }

        }

        private static Point GetOffsetPosition(IObjectInMap obj)
        {
            return GetOffsetPosition(obj.Position, obj.Size);
        }

        private static Point GetOffsetPosition(Point position, Size size)
        {
            var offset = new Size(size.Width / 2, size.Height / 2);
            return position - offset;
        }

    }
}
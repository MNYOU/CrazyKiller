using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ContentAlignment = System.Drawing.ContentAlignment;

namespace CrazyKiller
{
    public class View
    {
        private bool isOver;
        private int counter;
        private readonly Font buttonFont;
        private readonly Font ammunitionFont;
        private readonly Size buttonSize;
        private Dictionary<string, Image> images;
        private string[] phrases;
        private ViewGolems golemsView;
        private ViewPlayer viewPlayer;
        private GameForm Form { get; }
        private GameModel Game { get; }
        private Player Player { get; }
        private Graphics Graphics { get; set; }
        public Sounds Sounds { get; }

        public View(GameForm form, GameModel game)
        {
            Form = form;
            Game = game;
            Player = game.Player;
            form.Paint += Paint;
            InitializeImages();
            GetPhrases();
            buttonSize = new Size(500, 70);
            buttonFont = new Font(FontFamily.GenericMonospace, 30, FontStyle.Regular);
            ammunitionFont = new Font(FontFamily.GenericSerif, 50, FontStyle.Bold);
            Sounds = new Sounds();
        }

        private void GetPhrases()
        {
            phrases = new[]
            {
                "Ты победил всех и можешь по праву считаться истинным мастером-героем",
                "Что ж... Это была хорошая попытка. Однако пока ты недостаточно силён",
            };
        }

        private void InitializeImages()
        {
            var path =
                string.Join("\\", AppDomain.CurrentDomain.BaseDirectory.Split('\\').Reverse().Skip(3).Reverse()) +
                "\\images\\";
            Form.Icon = Icon.ExtractAssociatedIcon(path + "handgun.ico");
            images = new Dictionary<string, Image>();
            InitializeImage(path, "medkitSmall.png", Player);
            InitializeImage(path, "aimSmall.png", Player.Gun);
            InitializeImage(path, "medkitSmall.png", Game.MedKit);
            InitializeImage(path, "boxSmall.png", Game.Box, 2, 2);
            InitializeImage(path, "guns.png");
            InitializeImage(path, "backMenu.png");
            InitializeImage(path, "background.png");
        }

        private void InitializeImage(string path, string fileName, IObjectInMap obj = null, int column = 1, int row = 1)
        {
            var image = Image.FromFile(path + fileName);
            images[fileName.Split('.').First()] = image;
            if (obj != null)
                obj.Size = new Size(image.Width / column, image.Height / row);
        }

        private void Paint(object sender, PaintEventArgs e)
        {
            Graphics = e.Graphics;
            if (Game.IsPause) return;
            if (Game.IsOver) End();
            else if (Game.IsStarted)
            {
                Items();
                golemsView.PaintGolems(Graphics, Game.Golems);
                viewPlayer.Paint(Graphics);
                PanelBar();
                PaintAim();
            }
        }

        public void InitializeStart()
        {
            Form.BackgroundImage = images["background"];
            Form.BackgroundImageLayout = ImageLayout.Stretch;
            golemsView = new ViewGolems(Game.Golems);
            viewPlayer = new ViewPlayer(Player);
        }

        public void AddButtonInForm(Point position, string text, string name = null)
        {
            var button = new Button();
            button.Size = buttonSize;
            if (position != Point.Empty)
                button.Location = position - new Size(button.Size.Width / 2, button.Size.Height / 2);
            if (name != null) button.Name = name;
            button.Text = text;
            button.ForeColor = Color.Black;
            button.Font = buttonFont;
            button.BackColor = Color.Crimson;
            Form.Buttons.Add(button);
        }

        public void AddStartMenu()
        {
            var buttonPos = new Point(Form.Size.Width / 2, Form.Size.Height * 3 / 4);
            Form.ChangeButtonPositionAndStatus("Start", buttonPos);
            buttonPos.Y += buttonSize.Height + 10;
            Form.ChangeButtonPositionAndStatus("Exit", buttonPos);
            Form.BackgroundImageLayout = ImageLayout.Stretch;
            Form.BackgroundImage = images["backMenu"];
        }

        private void End()
        {
            if (counter == 170)
                Application.Exit();
            Sounds.Stop();
            counter++;
            if (isOver) return;
            isOver = true;
            var label = new Label();
            label.Size = new Size(GameModel.WindowSize.Width * 3 / 4, 500);
            label.BackColor = Color.Transparent;
            label.Font = new Font(FontFamily.GenericMonospace, 40, FontStyle.Italic);
            label.ForeColor = Color.White;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Location = new Point((Form.ClientSize.Width - label.Size.Width) / 2,
                (Form.ClientSize.Height - label.Size.Height) / 2);
            label.Text = Game.IsWon ? phrases.FirstOrDefault() : phrases.LastOrDefault();
            Form.Controls.Clear();
            Form.Controls.Add(label);
            label.BringToFront();
        }

        private void PaintAim()
        {
            var aimPos = PointMethods.GetOffsetPosition(Player.Gun);
            Graphics.DrawImage(images["aimSmall"], aimPos);
        }

        private void PanelBar()
        {
            var length = 500;
            var start = new Point((GameModel.WindowSize.Width - length) / 2, GameModel.WindowSize.Height - 70);
            var end = start;
            end.X += length;
            var pen = new Pen(Color.Black, 27);
            Graphics.DrawLine(pen, start, end);
            end.X = start.X + (int) (length * (Player.Hp * 1.0 / Player.MaxHp));
            pen.Color = Color.Red;
            Graphics.DrawLine(pen, start, end);

            end.X = start.X + length;
            PaintGun(end);

            var pos = new Point(Form.ClientSize.Width - 230, Form.ClientSize.Height / 2 - 50);
            Graphics.DrawString("волна " + Game.CurrentLevel?.Number, buttonFont, new SolidBrush(Color.Black), pos);
        }

        private void PaintGun(Point position)
        {
            position = new Point(position.X + 20, position.Y - 40);
            var image = images["guns"];
            var imageSize = new Size(image.Width / 11, image.Height / 11);
            var imagePosition = new Point();
            switch (Player.Gun)
            {
                case MachineGun _:
                    imagePosition = new Point(imageSize.Width, 0);
                    break;
                case Shotgun _:
                    imagePosition = new Point(imageSize.Width * 4, imageSize.Height * 6);
                    break;
                case Pistol _:
                    imagePosition = new Point(0, imageSize.Height);
                    break;
                case Rifle _:
                    imagePosition = new Point(imageSize.Width * 5, 0);
                    break;
                default:
                    throw new InvalidCastException("Оружие не добавлено в отрисовку");
            }

            var rect = new Rectangle(imagePosition, imageSize);
            Graphics.DrawImage(image, position.X, position.Y, rect, GraphicsUnit.Pixel);

            var ammunition = (Player.Gun.Ammunition - Player.Gun.FiredAmmunition).ToString();
            position = new Point(position.X + imageSize.Width + 20, position.Y + 10);
            Graphics.DrawString(ammunition + " ", ammunitionFont, new SolidBrush(Color.Black), position);
        }

        private void Items()
        {
            if (Game.MedKit.IsActive)
                Graphics.DrawImage(images["medkitSmall"], PointMethods.GetOffsetPosition(Game.MedKit));

            if (Game.Box.IsActive)
            {
                var box = Game.Box;
                var position = PointMethods.GetOffsetPosition(box);
                var rectangle = new Rectangle(0, 0, box.Size.Width, box.Size.Height);
                Graphics.DrawImage(images["boxSmall"], position.X, position.Y, rectangle, GraphicsUnit.Pixel);
            }
        }
    }
}
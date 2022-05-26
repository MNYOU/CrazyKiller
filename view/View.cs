using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using ContentAlignment = System.Drawing.ContentAlignment;

namespace CrazyKiller
{
    public class View
    {
        private bool IsOver;
        private bool IsPause;
        private int counter;
        private readonly Font buttonFont;
        private readonly Font ammunitionFont;
        private readonly Color labelFontColor;
        private readonly Size buttonSize;
        private Dictionary<string, Image> images;
        private string[] phrases;
        public Sounds Sounds { get; }
        private ViewGolems golemsView;
        private ViewPlayer viewPlayer;

        private GameForm form { get; }
        private GameModel game { get; }
        private Player Player { get; }
        private Graphics graphics { get; set; }

        public View(GameForm form, GameModel game)
        {
            this.form = form;
            this.game = game;
            Player = game.Player;
            form.Paint += Paint;
            InitializeImages();
            GetPhrases();
            buttonSize = new Size(500, 70);
            buttonFont = new Font(FontFamily.GenericMonospace, 30, FontStyle.Regular);
            ammunitionFont = new Font(FontFamily.GenericSerif, 50, FontStyle.Bold);
            labelFontColor = Color.White;
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
            form.Icon = Icon.ExtractAssociatedIcon(path + "handgun.ico");
            images = new Dictionary<string, Image>();
            InitializeImage(path, "medkitSmall.png", Player);
            // InitializeImage(path, "medkitSmall.png", game.Zombies.FirstOrDefault());
            game.Zombies.FirstOrDefault().Size = new Size(105, 70);
            InitializeImage(path, "aimSmall.png", Player.Gun);
            InitializeImage(path, "medkitSmall.png", game.MedKit);
            InitializeImage(path, "boxSmall.png", game.Box, 2, 2);
            InitializeImage(path, "unitySmall.png");
            InitializeImage(path, "guns.png");
            InitializeImage(path, "firstPart.png");
            InitializeImage(path, "secondPart.png");
            InitializeImage(path, "backMenu.png");
            InitializeImage(path, "background2.png");
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
            graphics = e.Graphics;
            if (game.IsPause) return;
            if (game.IsOver)
            {
                End();
            }
            else if (game.IsStarted)
            {
                Items();
                golemsView.PaintGolems(graphics, game.Zombies);
                viewPlayer.Paint(graphics);
                PanelBar();
                PaintAim();
            }
        }

        public void InitializeStart()
        {
            form.BackgroundImage = images["background2"];
            form.BackgroundImageLayout = ImageLayout.Stretch;
            // form.BackColor = Color.DarkGray;

            golemsView = new ViewGolems(game.Zombies);
            viewPlayer = new ViewPlayer(Player);
        }

        public void AddButtonInForm(Point position, string text, string name = null)
        {
            var button = new Button();
            button.Size = buttonSize;
            if (position != Point.Empty)
                button.Location = position - new Size(button.Size.Width / 2, button.Size.Height / 2);
            button.Name = name ?? "";
            button.Text = text;
            button.ForeColor = Color.Black;
            button.Font = buttonFont;
            button.BackColor = Color.Crimson;
            form.buttons.Add(button);
        }

        public void AddStartMenu()
        {
            var buttonPos = new Point(form.ClientSize.Width / 2, form.ClientSize.Height * 3 / 4);
            form.ChangeButtonPositionAndStatus("Start", buttonPos);
            buttonPos.Y += buttonSize.Height + 10;
            form.ChangeButtonPositionAndStatus("Exit", buttonPos);
            form.BackgroundImageLayout = ImageLayout.Stretch;
            form.BackgroundImage = images["backMenu"];
        }

        private void End()
        {
            if (counter == 170)
                Application.Exit();
            Sounds.Stop();
            counter++;
            if (IsOver) return;
            IsOver = true;
            var label = new Label();
            label.Size = new Size(GameModel.WindowSize.Width * 3 / 4, 500);
            label.BackColor = Color.Transparent;
            label.Font = new Font(FontFamily.GenericMonospace, 40, FontStyle.Italic);
            label.ForeColor = labelFontColor;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Location = new Point((form.ClientSize.Width - label.Size.Width) / 2,
                (form.ClientSize.Height - label.Size.Height) / 2);
            label.Text = game.IsWon ? phrases.FirstOrDefault() : phrases.LastOrDefault();
            form.Controls.Clear();
            form.Controls.Add(label);
            label.BringToFront();
        }

        private void PaintAim()
        {
            var aimPos = PointMethods.GetOffsetPosition(Player.Gun);
            graphics.DrawImage(images["aimSmall"], aimPos);
            if (Player.Gun.ISPenetration)
                graphics.DrawRectangle(new Pen(Color.Chartreuse, 10), 10, 10, 50, 50);
            if (Player.Gun.MouseIsClick)
                graphics.DrawRectangle(new Pen(Color.Blue, 10), 60, 10, 50, 50);
        }

        private void PanelBar()
        {
            var length = 500;
            var start = new Point((GameModel.WindowSize.Width - length) / 2, GameModel.WindowSize.Height - 70);
            var end = start;
            end.X += length;
            var pen = new Pen(Color.Black, 27);
            graphics.DrawLine(pen, start, end);
            end.X = start.X + (int) (length * (Player.Hp * 1.0 / Player.MaxHp));
            pen.Color = Color.Red;
            graphics.DrawLine(pen, start, end);

            end.X = start.X + length;
            PaintGun(end);

            var pos = new Point(form.ClientSize.Width - 230, form.ClientSize.Height / 2 - 50);
            graphics.DrawString("волна " + game.currentLevel?.Number, buttonFont, new SolidBrush(Color.Black), pos);
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
            graphics.DrawImage(image, position.X, position.Y, rect, GraphicsUnit.Pixel);

            var ammunition = (Player.Gun.Ammunition - Player.Gun.FiredAmmunition).ToString();
            position = new Point(position.X + imageSize.Width + 20, position.Y + 10);
            graphics.DrawString(ammunition + " ", ammunitionFont, new SolidBrush(Color.Black), position);
        }

        private void Items()
        {
            if (game.MedKit.IsActive)
                graphics.DrawImage(images["medkitSmall"], PointMethods.GetOffsetPosition(game.MedKit));

            if (game.Box.IsActive)
            {
                var box = game.Box;
                var position = PointMethods.GetOffsetPosition(box);
                var rectangle = new Rectangle(0, 0, box.Size.Width, box.Size.Height);
                graphics.DrawImage(images["boxSmall"], position.X, position.Y, rectangle, GraphicsUnit.Pixel);
            }
        }
    }
}
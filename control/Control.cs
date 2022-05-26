using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrazyKiller
{
    public class Control
    {
        private readonly GameForm form;
        private readonly GameModel game;
        private readonly Keys[] keysMove;
        private readonly Sounds Sounds;

        public Control(GameForm form, GameModel game, Sounds sounds)
        {
            this.form = form;
            this.game = game;
            Sounds = sounds;
            keysMove = new[] {Keys.A, Keys.D, Keys.W, Keys.S};
            InitializeEvents();
        }

        public void InitializeButtonsClicks()
        {
            foreach (var button in form.buttons)
            {
                button.Click += (sender, args) => Sounds.SoundButtonClick();
                switch (button.Name)
                {
                    case "Start":
                        button.Click += (sender, args) =>
                        {
                            form.Controls.Clear();
                            form.View.InitializeStart();
                            game.Start();
                            Sounds.StartMusic();
                        };
                        break;
                    case "Exit":
                        button.Click += (sender, args) => Application.Exit();
                        break;
                    case "Pause":
                        button.Click += (sender, args) =>
                        {
                            Pause(Keys.Escape);
                            Sounds.StartMusic();
                        };
                        break;
                }
            }
        }

        private void InitializeEvents()
        {
            form.KeyDown += OnKeyDown;
            form.KeyUp += OnKeyUp;
            form.MouseMove += MouseMove;
            form.MouseDown += MouseDown;
            form.MouseUp += MouseUp;
        }

        private void MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                game.Player.Gun.MouseIsClick = false;
        }

        private void MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                game.Player.Gun.MouseIsClick = true;
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (game.IsPause) return;
            game.MousePosition = e.Location;
            game.Player.MousePosition = e.Location;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            var key = e.KeyCode;
            if (keysMove.Contains(key))
                game.Player.AddOffset(key);
            else
                Pause(key);
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            var key = e.KeyCode;
            if (keysMove.Contains(key))
                game.Player.RemoveOffset(key);
        }

        private void Pause(Keys key)
        {
            if (key != Keys.Escape) return;
            if (game.IsPause)
                Sounds.StartMusic();
            else
                Sounds.Stop();
            game.IsPause = !game.IsPause;
            form.ChangeButtonPositionAndStatus("Pause",
                new Point(form.ClientSize.Width / 2, form.ClientSize.Height / 2));
            form.ChangeButtonPositionAndStatus("Exit",
                new Point(form.ClientSize.Width / 2, form.ClientSize.Height / 2 + 90));
        }
    }
}
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
        private readonly Sounds sounds;

        public Control(GameForm form, GameModel game, Sounds sounds)
        {
            this.form = form;
            this.game = game;
            this.sounds = sounds;
            keysMove = new[] {Keys.A, Keys.D, Keys.W, Keys.S};
            InitializeEvents();
        }

        public void InitializeButtonsClicks()
        {
            foreach (var button in form.Buttons)
            {
                button.Click += (sender, args) => sounds.SoundButtonClick();
                switch (button.Name)
                {
                    case "Start":
                        button.Click += (sender, args) =>
                        {
                            form.Controls.Clear();
                            form.View.InitializeStart();
                            game.Start();
                            sounds.StartMusic();
                        };
                        break;
                    case "Exit":
                        button.Click += (sender, args) => Application.Exit();
                        break;
                    case "Pause":
                        button.Click += (sender, args) =>
                        {
                            Pause(Keys.Escape);
                            sounds.StartMusic();
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
                sounds.StartMusic();
            else
                sounds.Stop();
            game.IsPause = !game.IsPause;
            form.ChangeButtonPositionAndStatus("Pause",
                new Point(form.Size.Width / 2, form.Size.Height / 2));
            form.ChangeButtonPositionAndStatus("Exit",
                new Point(form.Size.Width / 2, form.Size.Height / 2 + 90));
        }
    }
}
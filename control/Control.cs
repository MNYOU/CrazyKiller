using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrazyKiller
{
    public class Control
    {
        private GameForm form;
        private GameModel game;
        private Keys[] keysMove;

        public Control(GameForm form, GameModel game)
        {
            this.form = form;
            this.game = game;
            keysMove = new[] {Keys.A, Keys.D, Keys.W, Keys.S};
            InitializeEvents();
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
            game.MousePosition = e.Location;
            game.Player.MousePosition = e.Location;
        }

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            var key = e.KeyCode;
            if (keysMove.Contains(key))
                game.Player.AddOffset(key);
            else
            {
                Pause(key);
            }
        }

        public void OnKeyUp(object sender, KeyEventArgs e)
        {
            var key = e.KeyCode;
            if (keysMove.Contains(key))
                game.Player.RemoveOffset(key);

        }

        private void Pause(Keys key)
        {
            if (key != Keys.Escape) return;
        }
    }
}

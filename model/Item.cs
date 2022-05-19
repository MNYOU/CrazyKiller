using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrazyKiller
{
    public class Item : IObjectInMap
    {
        private int inactiveTime;

        public Item()
        {
            GenerateSleepTime();
        }

        private int timeToActivate { get; set; }

        public bool IsActive { get; private set; }
        public Point Position { get; set; }
        public Size Size { get; set; }
        public Random rnd = GameModel.rnd;

        private void GenerateSleepTime()
        {
            // слишком быстро
            switch (rnd.Next(0, 3))
            {
                case 0:
                    timeToActivate = 700;
                    break;
                case 1:
                    timeToActivate = 1000;
                    break;
                case 2:
                    timeToActivate = 2000;
                    break;
            }
        }

        private void Activate()
        {
            IsActive = true;
            Position = new Point(rnd.Next(GameModel.WindowSize.Width - Size.Width / 2),
                rnd.Next(GameModel.WindowSize.Height - Size.Height / 2));
            GenerateSleepTime();
        }

        private void Deactivate()
        {
            IsActive = false;
        }


        public void Interact(Player player)
        {
            if (inactiveTime == timeToActivate)
            {
                Activate();
                inactiveTime = 0;
            }

            if (!IsActive)
            {
                inactiveTime++;
                return;
            }

            if (GameModel.IsInteract(this, player))
            {
                Use(player);
                Deactivate();
            }
        }

        protected virtual void Use(Player player)
        {
            throw new NotImplementedException("метод не реализован");
        }
    }
}
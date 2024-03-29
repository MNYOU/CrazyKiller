﻿using System;
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
        private int timeToActivate;
        public bool IsActive { get; private set; }
        public Point Position { get; private set; }
        public Size Size { get; set; }
        protected Item()
        {
            GenerateSleepTime();
        }

        private void GenerateSleepTime()
        {
            var rnd = GameModel.Random;
            switch (rnd.Next(0, 3))
            {
                case 0:
                    timeToActivate = rnd.Next(500,700);
                    break;
                case 1:
                    timeToActivate = rnd.Next(700, 1000);
                    break;
                case 2:
                    timeToActivate = rnd.Next(1000, 1500);
                    break;
            }
        }

        private void Activate()
        {
            var rnd = GameModel.Random;
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
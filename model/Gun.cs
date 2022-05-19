﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyKiller
{
    public class Gun : IObjectInMap, IGun
    {
        private Player player;
        private static Size size;
        private int elapsedTime;
        private readonly int reloadingBetweenShoots;

        public Gun(Player player)
        {
            this.player = player;
            reloadingBetweenShoots = 10;
            elapsedTime = reloadingBetweenShoots;
        }

        public bool MouseIsClick { get; set; }
        public bool IsShooting { get; private set; }

        public Size Size
        {
            get => size;
            set => size = value;
        }

        public int Damage { get; protected set; }
        public int Ammunition { get; protected set; }
        public int Distance { get; protected set; }
        public int FiredAmmunition { get; set; }
        public int Recharge { get; protected set; }

        public Point Position
        {
            get
            {
                var playerToMouse = new Point(player.MousePosition.X - player.Position.X,
                    player.MousePosition.Y - player.Position.Y);
                var distance = Math.Sqrt(playerToMouse.X * playerToMouse.X + playerToMouse.Y * playerToMouse.Y);
                return new Point(player.Position.X + (int) (playerToMouse.X / distance * Distance),
                    player.Position.Y + (int) (playerToMouse.Y / distance * Distance));
            }
        }

        public void Shoot(List<Zombie> zombies)
        {
            IsShooting = false;
            if (FiredAmmunition == Ammunition || elapsedTime != reloadingBetweenShoots)
                elapsedTime++;
            if (FiredAmmunition == Ammunition)
            {
                if (elapsedTime != Recharge) return;
                elapsedTime = 0;
                FiredAmmunition = 0;
            }

            if (elapsedTime != reloadingBetweenShoots) return;
            if (!MouseIsClick) return;
            elapsedTime = 0;
            var zombie = zombies
                .OrderBy(z => PointMethods.GetDistance(z.Position, player.Position))
                .FirstOrDefault(IsPenetration);
            if (zombie == null) return;
            FiredAmmunition++;
            IsShooting = true;
            zombie.Hp -= Damage;
            if (zombie.Hp <= 0) zombies.Remove(zombie);
        }

        public bool IsPenetration(IObjectInMap target)
        {
            // лютая логика(не факт, что эффективная), понимать не советую
            if (target.Position.X >= player.Position.X)
            {
                if (target.Position.X - target.Size.Width / 2 > Position.X) return false;
            }
            else if (target.Position.X + target.Size.Width / 2 < Position.X)
            {
                return false;
            }

            var playerToAim = new Point(Position.X - player.Position.X,
                Position.Y - player.Position.Y);
            Func<int, int> f = x => (int) (playerToAim.Y * 1.0 / playerToAim.X * x);
            var playerToTarget = new Point(target.Position.X - player.Position.X,
                target.Position.Y - player.Position.Y);
            if (Math.Abs(playerToAim.Y) - Math.Abs(playerToAim.X) > 0)
            {
                (playerToAim.X, playerToAim.Y) = (playerToAim.Y, playerToAim.X);
                (playerToTarget.X, playerToTarget.Y) = (playerToTarget.Y, playerToTarget.X);
            }

            if (f(playerToTarget.X) <= playerToTarget.Y + target.Size.Height / 2 &&
                f(playerToTarget.X) >= playerToTarget.Y - target.Size.Height / 2)
            {
                ((Zombie) target).IsPenetration = true;
                return true;
            }

            ((Zombie) target).IsPenetration = false;
            return false;
        }
    }

    public class Pistol : Gun
    {
        public Pistol(Player player) : base(player)
        {
            Damage = 20;
            Ammunition = 10;
            Recharge = 19;
            Distance = 200;
        }
    }

    public class MachineGun : Gun
    {
        public MachineGun(Player player) : base(player)
        {
            Damage = 30;
            Ammunition = 20;
            Recharge = 19;
            Distance = 300;
        }
    }

    public class Shotgun : Gun
    {
        public Shotgun(Player player) : base(player)
        {
            Damage = 50;
            Ammunition = 6;
            Recharge = 19;
            Distance = 150;
        }
    }

    public class Rifle : Gun
    {
        public Rifle(Player player) : base(player)
        {
            Damage = 100;
            Ammunition = 4;
            Recharge = 19;
            Distance = 400;
        }
    }
}
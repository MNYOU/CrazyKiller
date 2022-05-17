using System;
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

        public Gun(Player player)
        {
            this.player = player;
        }

        public bool IsShoot { get; set; }
        public Size Size { get; set; }
        public int Damage { get; protected set; }
        public int Ammunition { get; protected set; }
        public int Distance { get; protected set; }
        public int FiredAmmunition { get; set; }
        public double Recharge { get; protected set; }

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
            if (!IsShoot) return;
            var zombie = zombies
                .OrderBy(z => GameModel.GetDistance(z.Position, player.Position))
                .FirstOrDefault(IsPenetration);
            if (zombie == null) return;
            zombie.Hp -= Damage;
            if (zombie.Hp <= 0) zombies.Remove(zombie);
        }

        public bool IsPenetration(IObjectInMap target)
        {
            var playerToMouse = new Point(player.MousePosition.X - player.Position.X,
                player.MousePosition.Y - player.Position.Y);
            Func<int, int> f = x => (int) (playerToMouse.Y * 1.0 / playerToMouse.X * x);
            var playerToTarget = new Point(target.Position.X - player.Position.X,
                target.Position.Y - player.Position.Y);
            if (f(playerToTarget.X) <= playerToTarget.X + target.Size.Width / 2 &&
                f(playerToTarget.X) >= playerToTarget.X - target.Size.Width / 2) return true;
            return false;
        }

        public Point GetAim()
        {
            return new Point();
        }
    }

    public class Pistol : Gun
    {
        public Pistol(Player player) : base(player)
        {
            Damage = 4;
            Ammunition = 10;
            Recharge = 19;
            Distance = 200;
        }
    }

    public class MachineGun : Gun
    {
        public MachineGun(Player player) : base(player)
        {
            Damage = 4;
            Ammunition = 10;
            Recharge = 19;
            Distance = 100;
        }
    }

    public class Shotgun : Gun
    {
        public Shotgun(Player player) : base(player)
        {
            Damage = 4;
            Ammunition = 10;
            Recharge = 19;
            Distance = 100;
        }
    }
}
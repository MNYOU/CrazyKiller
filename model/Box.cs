using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyKiller
{
    public class Box : Item // плохое оружие выпадает с большим шансом, хорошое с меньшим
    {
        private static Gun GenerateGun(Player player)
        {
            var rnd = GameModel.rnd.Next(0, 101);
            Gun gun;
            if (0 <= rnd && rnd <= 40)
                gun = new Pistol(player);
            else if (41 <= rnd && rnd <= 70)
                gun = new Shotgun(player);
            else if (71 <= rnd && rnd <= 90)
                gun = new MachineGun(player);
            else
                gun = new Rifle(player);
            return gun;
        }

        protected override void Use(Player player)
        {
            player.Gun = GenerateGun(player);
        }
    }
}

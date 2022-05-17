using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyKiller
{
    public class Box : Item
    {
        public Gun Gun { get; }

        public Box(Player player)
        {
            Gun = new MachineGun(player);
        }
    }
}

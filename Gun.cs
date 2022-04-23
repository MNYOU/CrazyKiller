using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace СrazyKiller
{
    public class Gun
    {

    }

    public class Pistol : Gun, IGun
    {
        public int Damage { get; set; }
        public int Ammunition { get; set; }
        public double Recharge { get; set; }

        public Pistol(int damage, int ammunition, double recharge)
        {
            Damage = damage;
            Ammunition = ammunition;
            Recharge = recharge;
        }

    }

    public class MachineGun : IGun
    {
        public int Damage { get;  set; }
        public int Ammunition { get; set; }
        public double Recharge { get; set; }

        public MachineGun(int damage, int ammunition, double recharge)
        {
            Damage = damage;
            Ammunition = ammunition;
            Recharge = recharge;
        }
    }

    public class Shotgun : IGun
    {
        public int Damage { get; set; }
        public int Ammunition { get; set; }
        public double Recharge { get; set; }

        public Shotgun(int damage, int ammunition, double recharge)
        {
            Damage = damage;
            Ammunition = ammunition;
            Recharge = recharge;
        }
    }
}

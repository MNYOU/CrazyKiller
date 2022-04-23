using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace СrazyKiller
{
    public interface IGun
    {
        int Damage { get; set; }
        int Ammunition { get; set; }
        double Recharge { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyKiller
{
    public interface IGun
    {
        int Damage { get; }
        int Ammunition { get; }
        int Distance { get; }
        int Recharge { get; }
    }
}
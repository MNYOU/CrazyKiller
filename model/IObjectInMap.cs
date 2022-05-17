using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyKiller
{
    public interface IObjectInMap
    {
        Point Position { get; }
        Size Size { get; set; }
    }
}

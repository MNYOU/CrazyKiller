using System.Drawing;

namespace CrazyKiller
{
    public interface IObjectInMap
    {
        Point Position { get; }
        Size Size { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyKiller
{
    public static class PointMethods
    {
        public static Point GetOffsetPosition(IObjectInMap obj)
        {
            return GetOffsetPosition(obj.Position, obj.Size);
        }

        public static Point GetOffsetPosition(Point position, Size size)
        {
            var offset = new Size(size.Width / 2, size.Height / 2);
            return position - offset;
        }

        public static int GetDistance(Point start, Point finish)
        {
            var point = new Point(finish.X - start.X, finish.Y - start.Y);
            return (int)Math.Sqrt(point.X * point.X + point.Y * point.Y);
        }
    }
}

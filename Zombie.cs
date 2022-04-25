using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace СrazyKiller
{
    // internal class Zombie<T> : IPerson where T : IPerson // что бы не забыть, как это пишется
    internal class Zombie: IPerson
    {
        public int Damage { get; }
        public int Speed { get; }
        public readonly Point Position;
        public Zombie(int damage, int speed, Point position)
        {
            Speed = speed;
            Position = position;
            Damage = damage;
        }

        public string GetImageFileName()
        {
            throw new NotImplementedException();
        }
    }
}
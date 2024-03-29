﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyKiller
{
    public class Level
    {
        public readonly int Number;
        public readonly int CountZombies;
        private static readonly int degreeIncrease = 5;

        public Level(int number)
        {
            Number = number;
            CountZombies = number * degreeIncrease;
        }
    }
}

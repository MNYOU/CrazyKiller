﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyKiller
{
    public class MedKit : Item
    {
        public int Health { get; }

        public MedKit()
        {
            Health = 50;
        }
    }
}

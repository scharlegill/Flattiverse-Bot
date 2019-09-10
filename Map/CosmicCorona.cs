﻿using Flattiverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map
{
    public class CosmicCorona
    {
        public readonly double Radius;

        public CosmicCorona(Corona corona)
        {
            Radius = corona.Radius;
        }
    }
}

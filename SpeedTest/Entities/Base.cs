﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedTest.Entities
{
    public abstract class Base
    {
        public Guid Id { get; set; }

        public Guid NoIndexId { get; set; }
    }
}

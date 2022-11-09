﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Base
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public bool InActive { get; set; } = false;


    }
}

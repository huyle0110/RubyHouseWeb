﻿using RubyHouseServices.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubyHouseServices.IServices
{
    public interface IAccountServices
    {
        bool Login(User model);
    }
}
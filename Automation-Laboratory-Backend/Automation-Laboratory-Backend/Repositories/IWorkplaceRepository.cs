﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Automation_Laboratory_Backend.Models;

namespace Automation_Laboratory_Backend.Repositories
{
    public interface IWorkplaceRepository
    {
        Workplace Create(Workplace workplace);
        List<Workplace> Read();
        Workplace Read(int workplaceId);
        Workplace Update(Workplace workplace);
    }
}

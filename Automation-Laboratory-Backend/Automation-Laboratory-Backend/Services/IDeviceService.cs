using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Automation_Laboratory_Backend.Models;

namespace Automation_Laboratory_Backend.Services
{
    public interface IDeviceService
    {
        Device Create(Device device);
        List<Device> Read();
        Device Read(int deviceId);
        Device Update(Device device);
    }
}

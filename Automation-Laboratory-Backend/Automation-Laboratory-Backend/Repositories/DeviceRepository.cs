using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Automation_Laboratory_Backend.Models;

namespace Automation_Laboratory_Backend.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly AutomationlaboratorydbContext _automationlaboratorydbContext;

        public DeviceRepository(AutomationlaboratorydbContext automationlaboratorydbContext)
        {
            _automationlaboratorydbContext = automationlaboratorydbContext;
        }
        public Device Create(Device device)
        {
            _automationlaboratorydbContext.Devices.Add(device);
            _automationlaboratorydbContext.SaveChanges();
            return device;
        }

        public List<Device> Read()
        {
            var everyDevice = _automationlaboratorydbContext.Devices.ToList();
            return everyDevice;
        }

        public Device Read(int deviceId)
        {
            var certainDevice = _automationlaboratorydbContext.Devices.FirstOrDefault(cd => cd.Id == deviceId);
            return certainDevice;
        }

        public Device Update(Device device)
        {
            _automationlaboratorydbContext.Devices.Update(device);
            _automationlaboratorydbContext.SaveChanges();
            return device;
        }
    }
}

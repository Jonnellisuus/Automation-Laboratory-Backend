using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Automation_Laboratory_Backend.Models;
using Automation_Laboratory_Backend.Repositories;

namespace Automation_Laboratory_Backend.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _deviceRepository;

        public DeviceService(IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }
        public Device Create(Device device)
        {
            return _deviceRepository.Create(device);
        }

        public List<Device> Read()
        {
            return _deviceRepository.Read();
        }

        public Device Read(int deviceId)
        {
            return _deviceRepository.Read(deviceId);
        }

        public Device Update(Device device)
        {
            return _deviceRepository.Update(device);
        }
    }
}

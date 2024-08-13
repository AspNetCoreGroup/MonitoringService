﻿using MonitoringCommonLibrary.Interfaces.Services;
using MonitoringService.DataSources;
using MonitoringService.Model.Entities;
using Microsoft.EntityFrameworkCore;
using ModelLibrary.Model;

namespace MonitoringService.Services
{
    public class DevicesService : IDevicesService
    {
        #region Поля

        private ILogger Logger { get; set; }

        private MonitoringContext Context { get; set; }

        #endregion

        #region Функционал

        public DevicesService(ILoggerFactory loggerFactory, MonitoringContext context)
        {
            Logger = loggerFactory.CreateLogger<DevicesService>();
            Context = context;
        }

        public async Task<DeviceDto> GetDeviceAsync(int deviceID)
        {
            var device = await Context.Devices.FindAsync(deviceID) ?? throw new KeyNotFoundException($"Device with deviceID {deviceID}");

            return Convert(device);
        }

        public async Task<IEnumerable<DeviceDto>> GetDevicesAsync()
        {
            var devices = await Context.Devices.ToListAsync();

            return devices.Select(Convert);
        }

        public async Task CreateDeviceAsync(DeviceDto deviceDto)
        {
            var device = Convert(deviceDto);

            Context.Add(device);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateDeviceAsync(int deviceID, DeviceDto deviceDto)
        {
            var device = Convert(deviceDto);

            device.DeviceID = deviceID;

            Context.Attach(device);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteDeviceAsync(int deviceID)
        {
            var device = new Device()
            {
                DeviceID = deviceID,
                DeviceCode = ""
            };

            Context.Remove(device);

            await Context.SaveChangesAsync();
        }

        private static DeviceDto Convert(Device device)
        {
            return new DeviceDto()
            {
                DeviceID = device.DeviceID,
                DeviceCode = device.DeviceCode
            };
        }

        private static Device Convert(DeviceDto device)
        {
            return new Device()
            {
                DeviceID = device.DeviceID,
                DeviceCode = device.DeviceCode,
            };
        }

        #endregion
    }
}
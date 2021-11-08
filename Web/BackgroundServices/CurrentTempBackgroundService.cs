using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmokerPiGui.Web.Services;
using System.Device.Gpio;
using Iot.Device.Ads1115;
using System.Device.I2c;
using UnitsNet;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Security.Cryptography;

namespace SmokerPiGui.Web.BackgroundServices 
{
    public class CurrentTempBackgroundService : BackgroundService
    {
        private readonly ILogger<CurrentTempBackgroundService> _logger;
        private readonly IProbeService _probeService;
        private readonly I2cConnectionSettings _settings1;
        private readonly I2cConnectionSettings _settings2;
        private readonly I2cDevice _device1;
        private readonly I2cDevice _device2;
        private readonly Ads1115 adc1;
        private readonly Ads1115 adc2;

        public CurrentTempBackgroundService(IProbeService probeService, ILogger<CurrentTempBackgroundService> logger)
        {
            _probeService = probeService;
            _logger = logger;
#if !DEBUG
            _settings1 = new I2cConnectionSettings(1, 0x48);
            // _settings2 = new I2cConnectionSettings(1, 0x49);
            _device1 = I2cDevice.Create(_settings1);
            // _device2 = I2cDevice.Create(_settings2);
            var measuringRange = MeasuringRange.FS2048;
            adc1 = new (_device1, measuringRange: measuringRange, dataRate: DataRate.SPS860, deviceMode: DeviceMode.Continuous);
            // adc2 = new (_device2, measuringRange: measuringRange, dataRate: DataRate.SPS860, deviceMode: DeviceMode.Continuous);
#endif
        }

        private void CheckTemps()
        {
            #if !DEBUG
            ElectricPotential sourceVoltage = adc1.ReadVoltage(InputMultiplexer.AIN2);
            // if (_probeService.Chamber.Connected) 
                ElectricPotential chamberProbe = adc1.ReadVoltage(InputMultiplexer.AIN0);
                _probeService.Chamber.Temperature = CalcTemp(chamberProbe, sourceVoltage);
                _probeService.Chamber = _probeService.Chamber;
                _logger.LogInformation($"Chamber Temperature is {_probeService.Chamber.Temperature}.");
                Console.WriteLine($"Current chamber temp is {_probeService.Chamber.Temperature}");
            // }
            if (_probeService.Probe1.Connected) 
            {
                ElectricPotential probe1 = adc1.ReadVoltage(InputMultiplexer.AIN1);
                _probeService.Probe1.Temperature = CalcTemp(probe1, sourceVoltage);
                _logger.LogInformation($"Probe 1 Temperature is {_probeService.Probe1.Temperature}.");
                Console.WriteLine($"Probe 1 Temperature is {_probeService.Probe1.Temperature}");
            }
            if (_probeService.Probe2.Connected)
            {
                ElectricPotential probe2 = adc1.ReadVoltage(InputMultiplexer.AIN3);
                _probeService.Probe2.Temperature = CalcTemp(probe2, sourceVoltage);
                _logger.LogInformation($"Probe 2 Temperature is {_probeService.Probe2.Temperature}.");
                Console.WriteLine($"Probe 2 Temperature is {_probeService.Probe2.Temperature}");
            }
            // if (_probeService.Probe3.Connected)
            // {
            //     ElectricPotential probe3 = adc2.ReadVoltage(InputMultiplexer.AIN0);
            //     _probeService.Probe2.Temperature = CalcTemp(probe3, sourceVoltage);
            //     _logger.LogInformation($"Probe 2 Temperature is {_probeService.Probe2.Temperature}.");
            //     Console.WriteLine($"Probe 2 Temperature is {_probeService.Probe2.Temperature}");
            // }
            // if (_probeService.Probe4.Connected)
            // {
            //     ElectricPotential probe4 = adc2.ReadVoltage(InputMultiplexer.AIN1);
            //     _probeService.Probe2.Temperature = CalcTemp(probe4, sourceVoltage);
            //     _logger.LogInformation($"Probe 2 Temperature is {_probeService.Probe2.Temperature}.");
            //     Console.WriteLine($"Probe 2 Temperature is {_probeService.Probe2.Temperature}");
            // }
            #endif
            #if DEBUG
            var random = new Random();
            _probeService.Chamber.Temperature = random.Next(74, 100);
            _probeService.Probe1.Temperature = random.Next(74, 100);
            _probeService.Probe2.Temperature = random.Next(74, 100);
            _probeService.Probe3.Temperature = random.Next(74, 100);
            _probeService.Probe4.Temperature = random.Next(74, 100);
            #endif
        }

        private static double CalcTemp(ElectricPotential voltage, ElectricPotential sourceVoltage) 
        {
            const double aValue = 0.0007343140544;
            const double bValue = 0.0002157437229;
            const double cValue = 0.0000000951568577;
            var volts = voltage.Volts;
            var sourceVolts = sourceVoltage.Volts;
            Console.WriteLine($"Source voltage is {sourceVolts}.");
            Console.WriteLine($"Current voltage is {volts}.");
            var resistance = 22000 * ((sourceVolts / volts) - 1);
            Console.WriteLine($"Thermistor resistance is {resistance}.");
            var temperatureK = 1 / (aValue + bValue * Math.Log(resistance) + cValue * Math.Pow(Math.Log(resistance), 3));
            var temperatureC = temperatureK - 272.15;
            var temperatureF = 9 / 5 * temperatureC + 32;

            return temperatureF;
        }

        protected async override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(2500, cancellationToken);
            while(!cancellationToken.IsCancellationRequested) {
                CheckTemps();
                await Task.Delay(2000, cancellationToken);
            }
        }
    }
}
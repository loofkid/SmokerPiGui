using System;
using System.Device.Gpio;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmokerPiGui.Web.Services;

namespace SmokerPiGui.Web.BackgroundServices 
{
    public class ProbeStatusBackgroundService : BackgroundService
    {
        private readonly IProbeService _probeService;
        private readonly ILogger<ProbeStatusBackgroundService> _logger;
        private readonly GpioController _gpioController;

        private const int chamberPin = 11, probe1Pin = 13, probe2Pin = 15, probe3Pin = 16;

        public ProbeStatusBackgroundService(IProbeService probeService, ILogger<ProbeStatusBackgroundService> logger)
        {
            _probeService = probeService;
            _logger = logger;
            _gpioController = new (PinNumberingScheme.Board);
            _gpioController.OpenPin(chamberPin, PinMode.InputPullDown);
            _gpioController.OpenPin(probe1Pin, PinMode.InputPullDown);
            _gpioController.OpenPin(probe2Pin, PinMode.InputPullDown);
        }

        private void CheckProbes()
        {

            if (_probeService.Chamber.Connected != (bool)_gpioController.Read(chamberPin))
            {
                _probeService.Chamber.Connected = (bool)_gpioController.Read(chamberPin);
                _logger.LogInformation($"Chamber probe {(_probeService.Chamber.Connected ? "connected" : "disconnected")}.");
                Console.WriteLine($"Chamber probe is {(_probeService.Chamber.Connected ? "connected" : "disconnected")}.");
            }
            if (_probeService.Probe1.Connected != (bool)_gpioController.Read(probe1Pin))
            {
                _probeService.Probe1.Connected = (bool)_gpioController.Read(probe1Pin);
                _logger.LogInformation($"Probe 1 {(_probeService.Probe1.Connected ? "connected" : "disconnected")}.");
                Console.WriteLine($"Probe 1 {(_probeService.Probe1.Connected ? "connected" : "disconnected")}.");
            }
            if (_probeService.Probe2.Connected != (bool)_gpioController.Read(probe2Pin))
            {
                _probeService.Probe2.Connected = (bool)_gpioController.Read(probe2Pin);
                _logger.LogInformation($"Probe 2 {(_probeService.Probe2.Connected ? "connected" : "disconnected")}.");
                Console.WriteLine($"Probe 2 {(_probeService.Probe2.Connected ? "connected" : "disconnected")}.");
            }
        }

        protected async override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while(!cancellationToken.IsCancellationRequested) {
                CheckProbes();
                await Task.Delay(500, cancellationToken);
            }
        }


        public async override Task StopAsync(CancellationToken cancellationToken)
        {
            _gpioController.ClosePin(chamberPin);
            _gpioController.ClosePin(probe1Pin);
            _gpioController.ClosePin(probe2Pin);
            await base.StopAsync(cancellationToken);
        }
    }
}
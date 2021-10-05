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
    public class TargetTempBackgroundService : BackgroundService
    {
        private readonly IProbeService _probeService;
        private readonly ILogger<TargetTempBackgroundService> _logger;
        private readonly IStatusService _statusService;
        private readonly GpioController _gpioController;
        private const int heatPin = 18;
        private bool heatingStatus = false;

        public TargetTempBackgroundService(IProbeService probeService, ILogger<TargetTempBackgroundService> logger, IStatusService statusService)
        {
            _probeService = probeService;
            _logger = logger;
            _statusService = statusService;
            _gpioController = new (PinNumberingScheme.Board);
            _gpioController.OpenPin(heatPin, PinMode.Output);
        }

        private void ControlHeating()
        {
            if (_probeService.Chamber.Temperature < _probeService.Chamber.TargetTemperature - 4) 
            {
                heatingStatus = true;
                _statusService.Heating = true;
                _gpioController.Write(heatPin, PinValue.High);
                _logger.LogInformation($"Heating.");
                Console.WriteLine($"Heating.");
            }
            else if (!heatingStatus)
            {}
            else if (_probeService.Chamber.Temperature > _probeService.Chamber.TargetTemperature + 1) {
                heatingStatus = false;
                _statusService.Heating = false;
                _gpioController.Write(heatPin, PinValue.Low);
                _logger.LogInformation($"Not heating.");
                Console.WriteLine($"Not heating.");
            }
        }

        protected async override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(4000, cancellationToken);
            while(!cancellationToken.IsCancellationRequested) {
                ControlHeating();
                await Task.Delay(2000, cancellationToken);
            }
        }

        public async override Task StopAsync(CancellationToken cancellationToken)  
        {
            _gpioController.ClosePin(heatPin);
            await base.StopAsync(cancellationToken);
        }
    }
}
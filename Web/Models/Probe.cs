using System;
using SmokerPiGui.Web.Services;

namespace SmokerPiGui.Web.Models
{
    public class Probe
    {
        public Probe()
        {
            DisplayName = String.Empty;
            Temperature = 0;
            TargetTemperature = 0;
            Connected = false;
        }
        private string displayName;
        private double temperature;
        private double targetTemperature;
        private bool connected;

        public event EventHandler<ProbeValueChangedArgs> DisplayNameChanged;
        public event EventHandler<ProbeValueChangedArgs> TemperatureChanged;
        public event EventHandler<ProbeValueChangedArgs> TargetTemperatureChanged;
        public event EventHandler<ProbeValueChangedArgs> ConnectedStateChanged;

        public string DisplayName 
        { 
            get => displayName; 
            set
            {
                if (displayName != value)
                { 
                    displayName = value;
                    DisplayNameChanged?.Invoke(this, new ProbeValueChangedArgs());
                }
            }
        }
        public double Temperature 
        { 
            get => temperature; 
            set
            {
                if (temperature != value)
                { 
                    temperature = value;
                    TemperatureChanged?.Invoke(this, new ProbeValueChangedArgs());
                }
            }
        }
        public double TargetTemperature 
        { 
            get => targetTemperature; 
            set
            {
                if (targetTemperature != value)
                {
                    targetTemperature = value; 
                    TargetTemperatureChanged?.Invoke(this, new ProbeValueChangedArgs());
                }
            }
        }
        public bool Connected 
        { 
            get => connected; 
            set
            {
                if (connected != value)
                {
                    connected = value; 
                    ConnectedStateChanged?.Invoke(this, new ProbeValueChangedArgs());
                }
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Probe probeObj
                && probeObj.DisplayName == DisplayName
                && probeObj.Temperature == Temperature
                && probeObj.TargetTemperature == TargetTemperature
                && probeObj.Connected == Connected;
        }

        public override int GetHashCode()
        {
            return DisplayName.GetHashCode() +
                Temperature.GetHashCode() +
                TargetTemperature.GetHashCode() +
                Connected.GetHashCode();
        }
    }
}

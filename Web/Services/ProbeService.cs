using System;
using SmokerPiGui.Web.Models;

namespace SmokerPiGui.Web.Services
{
    public class ProbeService : IProbeService
    {
        private Probe chamber;
        private Probe probe1;
        private Probe probe2;
        private Probe probe3;
        private Probe probe4;

        public ProbeService()
        {
            Chamber = new()
            {
                DisplayName = "Chamber Probe",
                Connected = false,
                Temperature = 0.00,
                TargetTemperature = 0.00
            };
            Chamber.DisplayNameChanged += ChamberValuesUpdated;
            Chamber.TemperatureChanged += ChamberValuesUpdated;
            Chamber.TargetTemperatureChanged += ChamberValuesUpdated;
            Chamber.ConnectedStateChanged += ChamberValuesUpdated;
            Probe1 = new()
            {
                DisplayName = "Probe 1",
                Connected = false,
                Temperature = 0.00,
                TargetTemperature = 0.00
            };
            Probe1.DisplayNameChanged += Probe1ValuesUpdated;
            Probe1.TemperatureChanged += Probe1ValuesUpdated;
            Probe1.TargetTemperatureChanged += Probe1ValuesUpdated;
            Probe1.ConnectedStateChanged += Probe1ValuesUpdated;
            Probe2 = new()
            {
                DisplayName = "Probe 2",
                Connected = false,
                Temperature = 0.00,
                TargetTemperature = 0.00
            };
            Probe2.DisplayNameChanged += Probe2ValuesUpdated;
            Probe2.TemperatureChanged += Probe2ValuesUpdated;
            Probe2.TargetTemperatureChanged += Probe2ValuesUpdated;
            Probe2.ConnectedStateChanged += Probe2ValuesUpdated;
            Probe3 = new()
            {
                DisplayName = "Probe 3",
                Connected = false,
                Temperature = 0.00,
                TargetTemperature = 0.00
            };
            Probe3.DisplayNameChanged += Probe3ValuesUpdated;
            Probe3.TemperatureChanged += Probe3ValuesUpdated;
            Probe3.TargetTemperatureChanged += Probe3ValuesUpdated;
            Probe3.ConnectedStateChanged += Probe3ValuesUpdated;
            Probe4 = new()
            {
                DisplayName = "Probe 4",
                Connected = false,
                Temperature = 0.00,
                TargetTemperature = 0.00
            };
            Probe4.ConnectedStateChanged += Probe4ValuesUpdated;
            Probe4.TemperatureChanged += Probe4ValuesUpdated;
            Probe4.TargetTemperatureChanged += Probe4ValuesUpdated;
            Probe4.ConnectedStateChanged += Probe4ValuesUpdated;
        }

        private void Probe4ValuesUpdated(object? sender, ProbeValueChangedArgs e)
        {
            Probe4Updated?.Invoke(this, new ProbeUpdateArgs());
        }

        private void Probe3ValuesUpdated(object? sender, ProbeValueChangedArgs e)
        {
            Probe3Updated?.Invoke(this, new ProbeUpdateArgs());
        }

        private void Probe2ValuesUpdated(object? sender, ProbeValueChangedArgs e)
        {
            Probe2Updated?.Invoke(this, new ProbeUpdateArgs());
        }

        private void Probe1ValuesUpdated(object? sender, ProbeValueChangedArgs e)
        {
            Probe1Updated?.Invoke(this, new ProbeUpdateArgs());
        }

        private void ChamberValuesUpdated(object? sender, ProbeValueChangedArgs e)
        {
            ChamberUpdated?.Invoke(this, new ProbeUpdateArgs());
        }

        public event EventHandler<ProbeUpdateArgs> ChamberUpdated;
        public event EventHandler<ProbeUpdateArgs> Probe1Updated;
        public event EventHandler<ProbeUpdateArgs> Probe2Updated;
        public event EventHandler<ProbeUpdateArgs> Probe3Updated;
        public event EventHandler<ProbeUpdateArgs> Probe4Updated;


        public Probe Chamber 
        { 
            get => chamber; 
            set 
            {
                chamber = value; 
                ChamberUpdated?.Invoke(this, new ProbeUpdateArgs());
            }
        }
        public Probe Probe1 
        { 
            get => probe1; 
            set
            {
                probe1 = value;
                Probe1Updated?.Invoke(this, new ProbeUpdateArgs());
            }
        }
        public Probe Probe2 
        { 
            get => probe2; 
            set 
            {
                probe2 = value;
                Probe2Updated?.Invoke(this, new ProbeUpdateArgs());
            }
        }
        public Probe Probe3 
        { 
            get => probe3; 
            set
            {
                probe3 = value; 
                Probe3Updated?.Invoke(this, new ProbeUpdateArgs());
            }
        }
        public Probe Probe4 
        {
            get => probe4; 
            set
            {
                probe4 = value; 
                Probe4Updated?.Invoke(this, new ProbeUpdateArgs());
            }
        }

    }
}
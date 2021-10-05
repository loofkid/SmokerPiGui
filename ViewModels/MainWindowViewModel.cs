using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using SmokerPiGui.Models;
using SmokerPiGui.Views;
using SmokerPiGui.Web.Models;
using SmokerPiGui.Web.Services;

namespace SmokerPiGui.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private IProbeService _probeService;
        private IStatusService _statusService;
        private string cookingText;
        private double chamberTemp;
        private string chamberReadout;
        private double targetTemp;
        private string targetTemperature;
        private string probe1Label;
        private double probe1Readout;
        private double probe1Target;
        private string probe2Label;
        private double probe2Readout;
        private double probe2Target;
        private string probe3Label;
        private double probe3Readout;
        private double probe3Target;
        private string probe4Label;
        private double probe4Readout;
        private double probe4Target;
        private bool dialogBackgroundVisible;
        private int dialogBackgroundZIndex;
        private int numberPanelZIndex;
        private int numpadTransform;
        private bool numpadClosed;
        private double numberPanelValue;
        private string preset1Label;
        private string preset2Label;
        private string numberPanelTitle;
        private string ipAddress;
        private string hostName;

        public MainWindowViewModel()
        {
            _probeService = SmokerPiGuiServices.ServiceProvider!.GetRequiredService<IProbeService>();
            _statusService = SmokerPiGuiServices.ServiceProvider!.GetRequiredService<IStatusService>();

            _probeService.Chamber.TemperatureChanged += UpdateChamberTemperature;
            _probeService.Chamber.TargetTemperatureChanged += UpdateTargetTemperature;

            _statusService.HeatingStatusChanged += UpdateHeatingStatus;

            CookingText = "Cooking";
            Cooking = true;
            Heating = _statusService.Heating;

            ChamberTemp = _probeService.Chamber.Temperature;
            ChamberReadout = $"Chamber Temp: {ChamberTemp}";

            TargetTemp = _probeService.Chamber.TargetTemperature;
            TargetTemperature = $"Target Temperature: {TargetTemp}";

            Probe1Label = "Probe 1";
            Probe1Readout = 75;
            Probe1Target = 160;

            Probe2Label = "Probe 2";
            Probe2Readout = 75;
            Probe2Target = 160;

            Probe3Label = "Probe 3";
            Probe3Readout = 75;
            Probe3Target = 160;

            Probe4Label = "Probe 4";
            Probe4Readout = 75;
            Probe4Target = 160;

            DialogBackgroundVisible = false;
            DialogBackgroundZIndex = -1000;

            NumpadClosed = true;
            
            SettingsClosed = true;

            OnInfo = true;
            OnSettings = false;

            IPAddress = GetIPv4Address(NetworkInterfaceType.Ethernet) ?? GetIPv4Address(NetworkInterfaceType.Wireless80211);
            HostName = Dns.GetHostName();
        }

        private void UpdateTargetTemperature(object? sender, ProbeValueChangedArgs e)
        {
            TargetTemp = _probeService.Chamber.TargetTemperature;
        }

        private void UpdateChamberTemperature(object? sender, ProbeValueChangedArgs e)
        {
            ChamberTemp = _probeService.Chamber.Temperature;
        }

        private void UpdateHeatingStatus(object? sender, StatusServiceArgs e)
        {
            Heating = _statusService.Heating;
        }

        public Probes? CurrentProbe;
        private bool settingsClosed;
        private bool onInfo;
        private bool onSettings;
        private bool cooking;
        private bool heating;

        public string CookingText { get => cookingText; set => this.RaiseAndSetIfChanged(ref cookingText, value); }
        public bool Cooking { get => cooking; set => this.RaiseAndSetIfChanged(ref cooking, value); }
        public bool Heating { get => heating; set => this.RaiseAndSetIfChanged(ref heating, value); }

        public double ChamberTemp { get => chamberTemp; set => this.RaiseAndSetIfChanged(ref chamberTemp, value); }
        public string ChamberReadout { get => chamberReadout; set => this.RaiseAndSetIfChanged(ref chamberReadout, value); }

        public double TargetTemp 
        { 
            get => targetTemp; 
            set
            {
                if (value != TargetTemp)
                {
                    _probeService.Chamber.TargetTemperature = value;
                }
                this.RaiseAndSetIfChanged(ref targetTemp, value); 
            }
        }
        public string TargetTemperature { get => targetTemperature; set => this.RaiseAndSetIfChanged(ref targetTemperature, value); }

        public string Probe1Label { get => probe1Label; set => this.RaiseAndSetIfChanged(ref probe1Label, value); }
        public double Probe1Readout { get => probe1Readout; set => this.RaiseAndSetIfChanged(ref probe1Readout, value); }
        public double Probe1Target { get => probe1Target; set => this.RaiseAndSetIfChanged(ref probe1Target, value); }

        public string Probe2Label { get => probe2Label; set => this.RaiseAndSetIfChanged(ref probe2Label, value); }
        public double Probe2Readout { get => probe2Readout; set => this.RaiseAndSetIfChanged(ref probe2Readout, value); }
        public double Probe2Target { get => probe2Target; set => this.RaiseAndSetIfChanged(ref probe2Target, value); }

        public string Probe3Label { get => probe3Label; set => this.RaiseAndSetIfChanged(ref probe3Label, value); }
        public double Probe3Readout { get => probe3Readout; set => this.RaiseAndSetIfChanged(ref probe3Readout, value); }
        public double Probe3Target { get => probe3Target; set => this.RaiseAndSetIfChanged(ref probe3Target, value); }

        public string Probe4Label { get => probe4Label; set => this.RaiseAndSetIfChanged(ref probe4Label, value); }
        public double Probe4Readout { get => probe4Readout; set => this.RaiseAndSetIfChanged(ref probe4Readout, value); }
        public double Probe4Target { get => probe4Target; set => this.RaiseAndSetIfChanged(ref probe4Target, value); }

        public bool DialogBackgroundVisible { get => dialogBackgroundVisible; set => this.RaiseAndSetIfChanged(ref dialogBackgroundVisible, value); }
        public int DialogBackgroundZIndex { get => dialogBackgroundZIndex; set => this.RaiseAndSetIfChanged(ref dialogBackgroundZIndex, value); }

        public bool NumpadClosed { get => numpadClosed; set => this.RaiseAndSetIfChanged(ref numpadClosed, value); }

        public int NumberPanelZIndex { get => numberPanelZIndex; set => this.RaiseAndSetIfChanged(ref numberPanelZIndex, value); }
        public int NumpadTransform { get => numpadTransform; set => this.RaiseAndSetIfChanged(ref numpadTransform, value); }

        public double NumberPanelValue { get => numberPanelValue; set => this.RaiseAndSetIfChanged(ref numberPanelValue, value); }

        public string Preset1Label { get=> preset1Label; set => this.RaiseAndSetIfChanged(ref preset1Label, value); }
        public string Preset2Label { get=> preset2Label; set => this.RaiseAndSetIfChanged(ref preset2Label, value); }
        public string NumberPanelTitle { get=> numberPanelTitle; set => this.RaiseAndSetIfChanged(ref numberPanelTitle, value); }

        public bool SettingsClosed { get=> settingsClosed; set=> this.RaiseAndSetIfChanged(ref settingsClosed, value); }

        public bool OnInfo { get=> onInfo; set=> this.RaiseAndSetIfChanged(ref onInfo, value); }
        public bool OnSettings { get=> onSettings; set=> this.RaiseAndSetIfChanged(ref onSettings, value); }
        

        public string IPAddress { get=> ipAddress; set=> this.RaiseAndSetIfChanged(ref ipAddress, value); }
        public string HostName { get=> hostName; set=> this.RaiseAndSetIfChanged(ref hostName, value); }

        public void TargetTempPressed()
        {
            Console.WriteLine("Target Temp Pressed.");
            Preset1Label = "225";
            Preset2Label = "250";
            NumberPanelValue = TargetTemp;
            NumberPanelTitle = "Set Target Chamber Temp";
            CurrentProbe = Probes.Chamber;
            ShowNumberPanel();
        }
        public void Probe1Pressed()
        {
            Console.WriteLine("Probe 1 Pressed.");
            Preset1Label = "203";
            Preset2Label = "180";
            NumberPanelValue = Probe1Target;
            NumberPanelTitle = "Set Probe 1 Target Temp";
            CurrentProbe = Probes.Probe1;
            ShowNumberPanel();
        }
        public void Probe2Pressed()
        {
            Console.WriteLine("Probe 2 Pressed");
            Preset1Label = "203";
            Preset2Label = "180";
            NumberPanelValue = Probe2Target;
            NumberPanelTitle = "Set Probe 2 Target Temp";
            CurrentProbe = Probes.Probe2;
            ShowNumberPanel();
        }
        public void Probe3Pressed()
        {
            Console.WriteLine("Probe 3 Pressed.");
            Preset1Label = "203";
            Preset2Label = "180";
            NumberPanelValue = Probe3Target;
            NumberPanelTitle = "Set Probe 3 Target Temp";
            CurrentProbe = Probes.Probe3;
            ShowNumberPanel();
        }
        public void Probe4Pressed()
        {
            Console.WriteLine("Probe 4 Pressed.");
            Preset1Label = "203";
            Preset2Label = "180";
            NumberPanelValue = Probe4Target;
            NumberPanelTitle = "Set Probe 4 Target Temp";
            CurrentProbe = Probes.Probe4;
            ShowNumberPanel();
        }

        private void ShowNumberPanel() 
        {
            DialogBackgroundVisible = true;
            DialogBackgroundZIndex = 150;
            NumpadClosed = false;
        }
        private void CloseNumberPanel()
        {
            DialogBackgroundVisible = false;
            NumpadClosed = true;
            NumberPanelValue = 0;
        }

        public void Preset1Button()
        {
            NumberPanelValue = double.Parse(Preset1Label);
            Console.WriteLine("Preset 1 Pressed.");
        }
        public void Preset2Button()
        {
            NumberPanelValue = double.Parse(Preset2Label);
            Console.WriteLine("Preset 2 Pressed.");
        }
        public void OneButton()
        {
            UpdateValueString(1);
            Console.WriteLine("Button 1 Pressed.");
        }
        public void TwoButton()
        {
            UpdateValueString(2);
            Console.WriteLine("Button 2 Pressed.");
        }
        public void ThreeButton()
        {
            UpdateValueString(3);
            Console.WriteLine("Button 3 Pressed.");
        }
        public void FourButton()
        {
            UpdateValueString(4);
            Console.WriteLine("Button 4 Pressed.");
        }
        public void FiveButton()
        {
            UpdateValueString(5);
            Console.WriteLine("Button 5 Pressed.");
        }
        public void SixButton()
        {
            UpdateValueString(6);
            Console.WriteLine("Button 6 Pressed.");
        }
        public void SevenButton()
        {
            UpdateValueString(7);
            Console.WriteLine("Button 7 Pressed.");
        }
        public void EightButton()
        {
            UpdateValueString(8);
            Console.WriteLine("Button 8 Pressed.");
        }
        public void NineButton()
        {
            UpdateValueString(9);
            Console.WriteLine("Button 9 Pressed.");
        }
        public void ZeroButton()
        {
            UpdateValueString(0);
            Console.WriteLine("Button 0 Pressed.");
        }
        public void CancelButton()
        {
            Console.WriteLine("Cancel Button Pressed.");
            CloseNumberPanel();
        }
        public void SaveButton()
        {
            switch (CurrentProbe) 
            {
                case Probes.Chamber:
                {
                    TargetTemp = NumberPanelValue;
                    break;
                }
                case Probes.Probe1:
                {
                    Probe1Target = NumberPanelValue;
                    break;
                }
                case Probes.Probe2:
                {
                    Probe2Target = NumberPanelValue;
                    break;
                }
                case Probes.Probe3:
                {
                    Probe3Target = NumberPanelValue;
                    break;
                }
                case Probes.Probe4:
                {
                    Probe4Target = NumberPanelValue;
                    break;
                }
                default:
                {
                    break;
                }

            }
            CloseNumberPanel();
            CurrentProbe = null;
            Console.WriteLine("Save Button Pressed.");
        }
        public void BackspaceButton()
        {
            var numberPanelValueString = NumberPanelValue.ToString();
            var numberPanelValueStringBackspaced = numberPanelValueString.Length > 1 ? numberPanelValueString.Remove(numberPanelValueString.Length - 1) : "0";
            NumberPanelValue = double.Parse(numberPanelValueStringBackspaced);
            Console.WriteLine("Backspace Button Pressed.");
        }

        private void UpdateValueString(int numberToAdd)
        {
            var numberPanelValueString = NumberPanelValue.ToString();
            var numberPanelValueStringAddNumber = numberPanelValueString != "0" ? numberPanelValueString + $"{numberToAdd}" : $"{numberToAdd}";
            NumberPanelValue = double.Parse(numberPanelValueStringAddNumber);
        }

        public void SettingsButton()
        {
            SettingsClosed = false;
        }
        public void SettingsCloseButton()
        {
            SettingsClosed = true;
        }

        public void GoToInfo()
        {
            OnInfo = true;
            OnSettings = false;
        }
        public void GoToSettings()
        {
            OnSettings = true;
            OnInfo = false;
        }

        private static string GetIPv4Address(NetworkInterfaceType type) 
        {
            string output = "";
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
            return output;
        }
    }
}

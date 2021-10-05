using System;
using SmokerPiGui.Web.Models;

namespace SmokerPiGui.Web.Services
{
    public interface IProbeService
    {
        Probe Chamber { get; set; }
        Probe Probe1 { get; set; }
        Probe Probe2 { get; set; }
        Probe Probe3 { get; set; }
        Probe Probe4 { get; set; }

        event EventHandler<ProbeUpdateArgs> ChamberUpdated;
        event EventHandler<ProbeUpdateArgs> Probe1Updated;
        event EventHandler<ProbeUpdateArgs> Probe2Updated;
        event EventHandler<ProbeUpdateArgs> Probe3Updated;
        event EventHandler<ProbeUpdateArgs> Probe4Updated;
    }
}

using System;

namespace SmokerPiGui.Web.Services
{
    public interface IStatusService
    {
        bool Heating { get; set; }

        event EventHandler<StatusServiceArgs> HeatingStatusChanged;
    }
}
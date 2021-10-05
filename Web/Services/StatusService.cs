using System;

namespace SmokerPiGui.Web.Services
{

    public class StatusService : IStatusService
    {
        public StatusService()
        {
            Heating = false;
        }
        private bool heating;

        public event EventHandler<StatusServiceArgs> HeatingStatusChanged;

        public bool Heating
        {
            get => heating;
            set
            {
                if (value != heating)
                {
                    heating = value;
                    HeatingStatusChanged?.Invoke(this, new StatusServiceArgs());
                }
            }
        }
    }
}
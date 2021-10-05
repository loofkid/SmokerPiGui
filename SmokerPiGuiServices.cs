using Microsoft.Extensions.DependencyInjection;

namespace SmokerPiGui
{
    public static class SmokerPiGuiServices
    {
        public static ServiceCollection Services { get; set; } = new ServiceCollection();
        public static ServiceProvider? ServiceProvider { get; set; }
    }
}
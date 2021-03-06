using Microsoft.Extensions.DependencyInjection;
using Miunie.Core.Logging;
using Miunie.Logger;
using Miunie.InversionOfControl;

namespace Miunie.ConsoleApp
{
    public static class InversionOfControl
    {
        private static ServiceProvider _provider;

        public static ServiceProvider Provider => GetOrInitProvider();

        private static ServiceProvider GetOrInitProvider()
        {
            if (_provider is null)
            {
                InitializeProvider();
            }

            return _provider;
        }

        private static void InitializeProvider()
            => _provider = new ServiceCollection()
                .AddSingleton<ILogger, ConsoleLogger>()
                .AddMiunieTypes()
                .BuildServiceProvider();
    }
}

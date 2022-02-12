using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineCraftConsole.Server.Services
{
    public class MineCraftService : BackgroundService
    {
        private readonly ICoordinator _coordinator;

        public MineCraftService(ICoordinator coordinator)
        {
            _coordinator = coordinator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            await _coordinator.StartServerAsync(stoppingToken);

        }
    }
}

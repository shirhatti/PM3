using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Terminal.Gui;

namespace ProcessManager
{
    internal class ProcessManagerService : IHostedService
    {
        private readonly IHostApplicationLifetime _lifetime;

        public ProcessManagerService(IHostApplicationLifetime lifetime)
        {
            _lifetime = lifetime;
            Application.Init();
            var top = Application.Top;
            top.Add(WindowHelper.Create());
            top.Add(MenuHelper.Create());
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();
            Application.Run();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Application.Top.Running = false;
            return Task.CompletedTask;
        }
    }
}
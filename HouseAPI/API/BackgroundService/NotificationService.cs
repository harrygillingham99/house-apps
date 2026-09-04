using House.HLL.Notifier.Interfaces;

namespace House.API.BackgroundService
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using HLL;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Serilog;

    public class NotificationHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public NotificationHostedService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }


        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Log.Information($"Hosted Service is starting.");

            while (!cancellationToken.IsCancellationRequested)
            {

                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var scopedServices = scope.ServiceProvider;
                    var processor = scopedServices.GetRequiredService<INotifier>();

                    //Notify all connected clients with new wallboard info
                    await processor.Notify();

                    await Task.Delay(10000, cancellationToken);

                }
                catch (Exception ex)
                {
                    Log.Error(ex,
                        $"Error occurred in Hosted Service");
                }
            }

            Log.Information($"Hosted Service has stopped.");
        }
    }

}

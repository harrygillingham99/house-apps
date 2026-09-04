using System.Linq;
using System.Threading.Tasks;
using House.HLL.Cat.Interfaces;
using House.HLL.Images.Interfaces;
using House.HLL.Notifier.Interfaces;
using House.HLL.ServerStats;
using House.Objects.Objects;
using House.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace House.HLL.Notifier
{
    public class Notifier : INotifier
    {
        private readonly ICatServiceClient _cats;
        private readonly IImageProvider _images;
        private readonly IMemoryStatusProvider _memory;
        private readonly IProcessInfo _processes;
        private readonly IHubContext<AppHub, IAppHub> _hub;

        public Notifier(ICatServiceClient cats, IImageProvider images, IMemoryStatusProvider memory, IProcessInfo processes, IHubContext<AppHub, IAppHub> hub)
        {
            _cats = cats;
            _images = images;
            _memory = memory;
            _processes = processes;
            _hub = hub;
        }

        public async Task Notify()
        {
            var info = new WallboardInfo
            {
                CatUrl = await _cats.GetRandomCatUrl(),
                RandomImageUri = await _images.GetRandomImageSource(),
                ServerStatus = _memory.GetMemoryInfo().ToList(),
                Processes = _processes.GetProcessInfo().OrderByDescending(proc => proc.MemoryMbUsed).ToList()
            };

            await _hub.Clients.All.Broadcast(info);
        }
    }
}

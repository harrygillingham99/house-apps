using System;
using System.Threading.Tasks;
using House.Objects.Objects;
using Microsoft.AspNetCore.SignalR;

namespace House.SignalR
{
    public class AppHub : Hub<IAppHub>
    {
        public Task Broadcast(WallboardInfo info)
        {
            return Clients.All.Broadcast(info);
        }
    }

    public interface IAppHub
    {
        Task Broadcast(WallboardInfo info);
    }
}

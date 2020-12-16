using System.Threading.Tasks;
using SignalrDemo.Models;

namespace SignalrDemo.Signalr
{
    public interface IHubClient<T> where T:class
    {
        Task SendMessage(MessageInstance msg);
        Task SendNofti(T msg);
        Task AddToGroup( string groupName);
       Task RemoveFromGroup( string groupName);
    }
}
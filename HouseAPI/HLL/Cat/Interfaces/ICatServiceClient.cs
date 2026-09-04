using System.Threading.Tasks;

namespace House.HLL.Cat.Interfaces
{
    public interface ICatServiceClient
    {
        Task<string> GetRandomCatUrl();
    }
}
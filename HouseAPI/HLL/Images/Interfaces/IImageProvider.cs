using System.Threading.Tasks;

namespace House.HLL.Images.Interfaces
{
    public interface IImageProvider
    {
        Task<string> GetRandomImageSource();
    }
}
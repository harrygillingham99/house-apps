using System.Collections.Generic;
using System.Threading.Tasks;
using House.HLL.News.Models;

namespace House.HLL.News.Interfaces
{
    public interface IAutoNewsConsumer
    {
        Task<List<NewsMessage>> CurrentNews();
    }
}
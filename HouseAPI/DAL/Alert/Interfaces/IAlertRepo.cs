namespace House.DAL.Alert.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using House.DAL.Alert.DataTransferObjects;

    public interface IAlertRepo
    {
        Task<IEnumerable<AlertDto>> Get();
        Task<IEnumerable<AlertDto>> GetLatest();
        Task<IEnumerable<AlertDto>> Get(int id);
        Task<IEnumerable<AlertDto>> Get(IEnumerable<int> ids);
        Task Post(NewAlert newAlert);
        Task Put(int id, NewAlert newAlert);
        Task Delete(int id);
    }
}

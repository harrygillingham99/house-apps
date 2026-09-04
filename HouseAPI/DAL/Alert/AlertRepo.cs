namespace House.DAL.Alert
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dapper;
    using DataTransferObjects;
    using Interfaces;
    using SQL;
    using Microsoft.Extensions.Options;

    public class AlertRepo : BaseRepository, IAlertRepo
    {
        public AlertRepo(IOptions<DbConnections> connectionStrings)
            : base(connectionStrings.Value.HouseSql)
        {
        }

        public Task<IEnumerable<AlertDto>> Get()
        {
            return ExecuteFunc(qry => qry.QueryAsync<AlertDto>(AlertSql.Get));
        }

        public Task<IEnumerable<AlertDto>> GetLatest()
        {
            return ExecuteFunc(qry => qry.QueryAsync<AlertDto>(AlertSql.GetLatest));
        }

        public Task<IEnumerable<AlertDto>> Get(int id)
        {
            return ExecuteFunc(qry => qry.QueryAsync<AlertDto>(AlertSql.GetById, new
            {
                Id = id,
            }));
        }

        public Task<IEnumerable<AlertDto>> Get(IEnumerable<int> ids)
        {
            return ExecuteFunc(qry => qry.QueryAsync<AlertDto>(AlertSql.GetByIds, new
            {
                Id = ids,
            }));
        }

        public async Task Post(NewAlert newAlert)
        {
            await ExecuteFunc(qry => qry.ExecuteAsync(AlertSql.Insert, new
            {
                newAlert.Message, newAlert.CreatedBy,
            }));
        }

        public async Task Put(int id, NewAlert newAlert)
        {
            await ExecuteFunc(qry => qry.ExecuteAsync(AlertSql.Update, new
            {
                Id = id,
                newAlert.Message,
                newAlert.CreatedBy,
            }));
        }

        public async Task Delete(int id)
        {
            await ExecuteFunc(qry => qry.ExecuteAsync(AlertSql.Delete, new
            {
                Id = id,
            }));
        }
    }
}

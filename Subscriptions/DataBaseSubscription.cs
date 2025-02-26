


using ChartsExampleServer.Data;
using ChartsExampleServer.Hubs;
using ChartsExampleServer.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TableDependency.SqlClient;

namespace ChartsExampleServer.Subscriptions
{
    public interface IDataBaseSubscription
    {
        void Configure(string tableName);
    }

    public class DataBaseSubscription<T> : IDataBaseSubscription where T : class, new() //T nesne üretilebilir bir class
    {
        SqlTableDependency<T>? _sqlTableDependency;
        IConfiguration _configuration;
        IHubContext<SalesHub> _salesHubContext;

        public DataBaseSubscription(IConfiguration configuration, IHubContext<SalesHub> salesHubContext)
        {
            _configuration = configuration;
            _salesHubContext = salesHubContext;
        }

        public void Configure(string tableName)
        {
            _sqlTableDependency = new SqlTableDependency<T>(_configuration.GetConnectionString("connection"), tableName);
            //evente lambda ile delege verioz o ve e => object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<T> e

            _sqlTableDependency.OnChanged += async (o, e) =>
            {

                ChartsExampleDbContext context = new ChartsExampleDbContext();

                var query = from personel in context.Personels
                            join sale in context.Sales
                            on personel.Id equals sale.PersonelId
                            select new { personel, sale };

                var data = await query.ToListAsync();

                var values = data.GroupBy(d => d.personel.Id)
                            .Select(group => new
                            {
                                name = group.Select(g => g.personel.Name).FirstOrDefault(),
                                data = group.Select(g => g.sale.Price).ToList()

                            }).ToList();



                await _salesHubContext.Clients.All.SendAsync("receiveMessage", values);
            };

            //error
            _sqlTableDependency.OnError += (o, e) =>
            {


            };

            _sqlTableDependency.Start();
        }

        //de~constractor
        ~DataBaseSubscription()
        {
            _sqlTableDependency?.Stop();
        }

    }
}

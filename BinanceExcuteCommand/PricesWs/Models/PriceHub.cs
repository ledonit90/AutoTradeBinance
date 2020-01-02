using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Pricesws.Models
{
    public class PriceHub : Hub<IPriceHub>
    {
        //private IUnitOfWorks _unitofwork;
        public PriceHub() : base()
        {
            //ConfigurationBuilder configBuilder = new ConfigurationBuilder();
            //var path = Directory.GetCurrentDirectory();
            //configBuilder.SetBasePath(path)
            //.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            //var config = configBuilder.Build();
            //var connectionString = config.GetConnectionString("SGDConnection");
            //var buildOptionContext = new DbContextOptionsBuilder<SGDDataContext>();
            //buildOptionContext.UseSqlServer(connectionString);
            //SGDDataContext _context = new SGDDataContext(buildOptionContext.Options);
            //_unitofwork = new UnitOfWorks(_context);
        }

        //public List<string> getListGroups()
        //{
        //    var allCoins = _unitofwork.Coins.GetAllQuery(x => x.Active == true && x.isBuy == true).Select(x=>x.Code).ToList();
        //    return allCoins;
        //}

        public override Task OnConnectedAsync()
        {
            //List<string> listAllCoin = getListGroups();
            //foreach(var item in listAllCoin)
            //{
            //    Groups.AddToGroupAsync(Context.ConnectionId, item);
            //}
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            // Close all GroupName
            //List<string> listAllCoin = getListGroups();
            //foreach (var item in listAllCoin)
            //{
            //    Groups.RemoveFromGroupAsync(Context.ConnectionId, item);
            //}
            return base.OnDisconnectedAsync(exception);
        }
    }
}

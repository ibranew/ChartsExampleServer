using ChartsExampleServer.Models;

namespace ChartsExampleServer.Subscriptions
{
    public static class ServiceRegistration
    {
        public static void AddChartsExampleSubscriptionsServices(this IServiceCollection services)
        { 
           services.AddSingleton<DataBaseSubscription<Personel>>();   
           services.AddSingleton<DataBaseSubscription<Sale>>();   
        }
    }
}


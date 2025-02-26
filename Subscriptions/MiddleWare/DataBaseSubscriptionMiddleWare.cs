


namespace ChartsExampleServer.Subscriptions.MiddleWare
{
    static public class DataBaseSubscriptionMiddleWare
    {
        public static void UseChartsExampleDataBaseSubscriptions<T>(this IApplicationBuilder builder,string tableName) where T : class ,IDataBaseSubscription
        {
            object? obje = builder.ApplicationServices.GetService(typeof(T));
            
            if(obje is not null and T)
            {
                T subscription = (T)obje;
                subscription.Configure(tableName);
            }
        }
    }
}

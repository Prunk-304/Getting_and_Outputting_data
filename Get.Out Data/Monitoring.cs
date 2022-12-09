using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;

namespace ConsoleApp1;

public class Monitoring : IMonitoring
{
    private readonly HttpClient _client = new HttpClient(_handler)
    {
    };

    private static readonly SocketsHttpHandler _handler = new SocketsHttpHandler
    {
        PooledConnectionLifetime = TimeSpan.FromSeconds(5),
    };
    public int Timer(dynamic configuration, dynamic logger)
    {
        int time = Convert.ToInt32(configuration.GetSection("Timeout").Value);
        logger.Info($"Задержка обновления:{time}");
        logger.Info("Приложение начало работу");
        return time;
    }

    public string[] URIList(dynamic configuration)
    {
        string URI = configuration.GetSection("URL").Value;
        string[] uriList = URI.Split(',');
        return uriList;

    }


    public async void Check( dynamic logger, string urilist)
    { string uri = urilist;
        try
        {
            logger.Info($"Now checking: {uri}"); 
            using var result = await _client.GetAsync(uri); 
            logger.Info((int)result.StatusCode); 
            logger.Info(result.ReasonPhrase); 
            object? result2 = await _client.GetFromJsonAsync(uri, typeof(Object)); 
            if (result2 is Object need) 
            { 
                if (need.error != null) 
                { 
                    logger.Error($"{uri}: Error is not null.\nindicatorId: {need.indicatorId}\nfromCache: {need.fromCache}\nerror: {need.error}");
                }
                else
                {
                    logger.Info($"indicatorId: {need.indicatorId}\nfromCache: {need.fromCache}\nerror: {need.error}");
                }
            }
        }catch (Exception e)
        {
            logger.Error($"{uri}. {e.Message}"); 
        }
            
    }
}
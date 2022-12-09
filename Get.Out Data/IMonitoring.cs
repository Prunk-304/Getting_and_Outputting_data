using System.Net.Http.Json;

namespace ConsoleApp1;

public interface IMonitoring
{

    int Timer(dynamic configuration, dynamic logger);
    string[] URIList(dynamic configuration);
    void Check(dynamic logger, string urilist);

}
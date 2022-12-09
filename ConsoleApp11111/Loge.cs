using NLog;
using NLog.Extensions.Logging;
using NLog.Telegram;
namespace ConsoleApp1;

public class Loge : ILoge
{
    public dynamic LogConst(dynamic configure)
    { LogManager.Configuration = new NLogLoggingConfiguration(configure.GetSection("NLog"));
        var logger = NLog.Web.NLogBuilder.ConfigureNLog(LogManager.Configuration).GetLogger("COM");
      
        return logger;
    }
}
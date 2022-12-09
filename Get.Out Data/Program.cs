using System.Net.Http.Json;
using ConsoleApp1;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace ConsoleApp1
{  
    class Program
        {
            public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            {
                IConstr First = new Constr();
                ILoge log = new Loge();
                IMonitoring Mon = new Monitoring();
                var configuration = First.build("configure.json");
                var logger = log.LogConst(configuration);
                int timeout = Mon.Timer(configuration, logger);
                string[] uriList = Mon.URIList(configuration);
                var wait = Task.Delay(TimeSpan.FromSeconds(timeout));
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
                if(update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
                {
                    var message = update.Message;
                    if (message.Text.ToLower() == "/start")
                    {
                        foreach (string uri in uriList) 
                        { 
                            Mon.Check(logger, uri); 
                            await wait;
                        }
                        
                        return;
                    }
                    await botClient.SendTextMessageAsync(message.Chat, "Для начала работы выполните команду /start");
                }
            }

            public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
            {
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
            }


            static void Main()
            {
                IConstr TelegIni = new Constr();
                var config = TelegIni.build("configure.json");
                ITelegramBotClient bot = new TelegramBotClient(config.GetSection("TOKEN").Value);
                Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);
                var cts = new CancellationTokenSource();
                var cancellationToken = cts.Token;
                var receiverOptions = new ReceiverOptions
                {
                    AllowedUpdates = { },
                };
                bot.StartReceiving(
                    HandleUpdateAsync,
                    HandleErrorAsync,
                    receiverOptions,
                    cancellationToken
                );
                Console.ReadLine();
            }
        }
    }
       
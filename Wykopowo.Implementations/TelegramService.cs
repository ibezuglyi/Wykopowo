using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Wykopowo.Contracts;

namespace Wykopowo.Implementations
{
    public class TelegramService : ITelegramService
    {
        private TelegramBotClient botClient;
        public event EventHandler<MessageEventArgs> OnMessage;
        public TelegramService(string token)
        {
            botClient = new TelegramBotClient(token);
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
        }

        private void Bot_OnMessage(object? sender, MessageEventArgs e)
        {
            var onMessage = this.OnMessage;
            onMessage?.Invoke(sender, e);
        }


        public async Task SendMessages(List<SubscriptionMessage> messages)
        {
            foreach (var message in messages)
            {
                await botClient.SendTextMessageAsync(message.ChatId, message.Text, ParseMode.Html);
            }
        }
    }
}
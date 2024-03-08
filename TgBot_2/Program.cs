using System;
using System.Security.Cryptography;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;


const string Token = "6797204620:AAFykmKYzY0kz0fybDSwsxii912hRi-hiuk";
const long ChatId = -1001651569674;
bool SentToMeMode = false;

var botClient = new TelegramBotClient(Token);
using var cts  = new CancellationTokenSource();

var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = { }
};
botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, cancellationToken: cts.Token);
var me = await botClient.GetMeAsync();
Console.WriteLine($"Наченаем работу с @" +  me.Username);
await Task.Delay( int.MaxValue );
cts.Cancel();
async Task HandleUpdateAsync( TelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    #region [главное меню]
    InlineKeyboardMarkup mainMenu = new InlineKeyboardMarkup(new[] {
    new[] {InlineKeyboardButton.WithUrl(text: "Перейти на катал GG", url: "https://t.me/gOdModgg")},
    new[] {InlineKeyboardButton.WithCallbackData(text: "Написать пожелание", callbackData: "sendOrder")} });
    #endregion
    InlineKeyboardMarkup backMenu = new(new[] { new[] { InlineKeyboardButton.WithCallbackData(text: "К главному меню", callbackData: "toBack") } });
    
    if (update.Type == UpdateType.Message && update.Message!.Type == MessageType.Text)
    {
        var chatId = update.Message.Chat.Id;
        var messgeText = update.Message.Text;
        string firstName = update.Message.From.FirstName;
        object messageText = null;
        Console.WriteLine($"Получено сообщение: '{messageText}' в чате {chatId}");
        #region [Первое сообщение ]
        if(messageText == "/start")
        {
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"Привет {firstName}! \n \nЯ ботик для дружбы и для работы с каналом ദ്ദി(˶ᵔ ᵕ ᵔ˶)" + Environment.NewLine + "Чё те надо бро? 🧐",
                replyMarkup: mainMenu,
                cancellationToken: cancellationToken);
        }
        #endregion
    }
}
Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };
    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;  
}

async Task SendPhoto(long chatID, CancellationToken token)
{
    Message message = await botClient.SendPhotoAsync(
        chatId: chatID,
        photo: InputFile.FromUri("https://i.pinimg.com/originals/ca/a9/92/caa992c5d2b6310c02de9d4472809781.jpg"),
        parseMode: ParseMode.Html,
        cancellationToken: token);
}
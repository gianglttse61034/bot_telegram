using System;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;


string command = "/start";
string token = "{token here}";
ChatId paramGroupChatCRM = 1213 ; // Chat Id group here 
var botClient = new TelegramBotClient(token);
using var cts = new CancellationTokenSource();
var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
};
botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
        );
while (command != string.Empty)
{
    if (command == "/command stop-bot")
    {
        cts.Cancel();
        break;
    }
    Thread.Sleep(60000 * 60 * 24);
}

/// <summary>
/// Handle message listen from BOT
/// </summary>
async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    string str = "";
    // Only process Message updates: https://core.telegram.org/bots/api#message
    if (update.Message is not { } message)
        return;
    if (message.Chat.Id == paramGroupChatCRM)
    {
        return;
    }
    if (message.Sticker != null && !string.IsNullOrWhiteSpace(message.Sticker?.FileId))
    {
        string fieldId = message.Sticker.FileId;
        Message sentMessageGroup = await botClient.SendStickerAsync(
        chatId: paramGroupChatCRM,
        new Telegram.Bot.Types.InputFiles.InputOnlineFile(fieldId),
        cancellationToken: cancellationToken);
    }
    if (message.Document != null && !string.IsNullOrEmpty(message.Document.FileId))
    {
        Message sentMessageGroup = await botClient.SendDocumentAsync(
        chatId: paramGroupChatCRM,
       new Telegram.Bot.Types.InputFiles.InputOnlineFile(message.Document.FileId),
        cancellationToken: cancellationToken);

    }
    if (message.Video != null && !string.IsNullOrEmpty(message.Video.FileId))
    {
        Message sentMessageGroup = await botClient.SendVideoAsync(
        chatId: paramGroupChatCRM,
       new Telegram.Bot.Types.InputFiles.InputOnlineFile(message.Video.FileId),
        cancellationToken: cancellationToken);

    }
    if (message.Audio != null && !string.IsNullOrEmpty(message.Audio.FileId))
    {
        Message sentMessageGroup = await botClient.SendAudioAsync(
        chatId: paramGroupChatCRM,
       new Telegram.Bot.Types.InputFiles.InputOnlineFile(message.Audio.FileId),
        cancellationToken: cancellationToken);

    }
    if (message.Photo != null && message.Photo.Count() > 0)
    {

        Message sentMessageGroup = await botClient.SendPhotoAsync(
        chatId: paramGroupChatCRM,
       new Telegram.Bot.Types.InputFiles.InputOnlineFile(message.Photo[0].FileId),
        cancellationToken: cancellationToken);

    }
    if (!string.IsNullOrEmpty(message.Text))
    {
        switch (message.Text)
        {
            case "/command stop-bot":
                command = "/command stop-bot";
                break;
            case "/start":
                await botClient.SendTextMessageAsync(message.Chat.Id, "😘Bé bot đáng yêu đã khởi động😘. Tất cả tin nhắn chat sẽ được gửi sang group MOMO - Confession ẩn danh 🙈", ParseMode.Markdown, null, null, null, null, null, true, CreateButton());
                break;
            case "/help":
                await botClient.SendTextMessageAsync(message.Chat.Id, "😘Không có gì để hướng dẫn😘. Tất cả tin nhắn chat sẽ được gửi sang group MOMO - Confession ẩn danh 🙈", ParseMode.Markdown, null, null, null, null, null, true, CreateButton());
                break;
            case "/settings":
                await botClient.SendTextMessageAsync(message.Chat.Id, "😘Không có gì để cấu hình cả😘. Tất cả tin nhắn chat sẽ được gửi sang group MOMO - Confession ẩn danh 🙈", ParseMode.Markdown, null, null, null, null, null, true, CreateButton());
                break;
            default:
                Message sentMessageGroup = await botClient.SendTextMessageAsync(
                chatId: paramGroupChatCRM,
                text: "➡️ " + message.Text,
                cancellationToken: cancellationToken);
                break;
        }
       
    }
   
}

/// <summary>
/// Handle error message
/// </summary>
Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}

InlineKeyboardMarkup CreateButton()
{
    List<InlineKeyboardButton> lstButton = new List<InlineKeyboardButton>();
    lstButton.Add(new InlineKeyboardButton("Link group MOMO - Confession") { Url = "https://t.me/+c78mJD1cT1pmM2Q1" });
    return new InlineKeyboardMarkup(lstButton);
}





using Microsoft.Extensions.Options;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;

namespace Outlays.Bot.Services;

public class TelegramBotService
{
    private readonly ITelegramBotClient _botClient;

    public TelegramBotService(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task SendTextMessageAsync(long chatId, string message)
    {
        await _botClient.SendTextMessageAsync(chatId, message);
        
    }
    public async Task SendMessageAsync(long chatId, string message, IReplyMarkup reply = null)
    {
        await _botClient.SendTextMessageAsync(chatId, message, replyMarkup: reply);
    }

    public void SendMessage(long chatId, string message, Stream image, IReplyMarkup reply = null)
    {
        _botClient.SendPhotoAsync(chatId, new InputOnlineFile(image), message, replyMarkup: reply);
    }

    public void EditMessageButtons(long chatId, int messageId, InlineKeyboardMarkup reply)
    {
        _botClient.EditMessageReplyMarkupAsync(chatId, messageId, replyMarkup: reply);
    }

    public ReplyKeyboardMarkup GetKeyboard(List<string> buttonsText)
    {
        var buttons = new KeyboardButton[buttonsText.Count][];

        for (var i = 0; i < buttonsText.Count; i++)
        {
            buttons[i] = new KeyboardButton[] { new(buttonsText[i]) };
        }

        return new ReplyKeyboardMarkup(buttons) { ResizeKeyboard = true,OneTimeKeyboard = true };
    }

    public InlineKeyboardMarkup GetInlineKeyboard(List<string> buttonsText, int? correctAnswerIndex = null, int? questionIndex = null)
    {
        InlineKeyboardButton[][] buttons = new InlineKeyboardButton[buttonsText.Count][];

        for (var i = 0; i < buttonsText.Count; i++)
        {
            buttons[i] = new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData(
                text: buttonsText[i],
                callbackData: correctAnswerIndex == null ? buttonsText[i] : $"{correctAnswerIndex},{i},{questionIndex}"),  };
        }

        return new InlineKeyboardMarkup(buttons);
    }

    public InlineKeyboardMarkup GetInlineTicketsKeyboard(List<string> buttonsText, int? ticketIndex = null)
    {
        var buttons = new InlineKeyboardButton[buttonsText.Count][];

        for (var i = 0; i < buttonsText.Count; i++)
        {
            buttons[i] = new[] { InlineKeyboardButton.WithCallbackData(
                text: buttonsText[i],
                callbackData: ticketIndex == null ? i.ToString() : ticketIndex.ToString())  };
        }

        return new InlineKeyboardMarkup(buttons);
    }

    public InlineKeyboardMarkup GetInlineKeyboard(List<string> buttonsText, int correctAnswerIndex, int questionIndex, int ticketIndex)
    {
        InlineKeyboardButton[][] buttons = new InlineKeyboardButton[buttonsText.Count][];

        for (var i = 0; i < buttonsText.Count; i++)
        {
            buttons[i] = new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData(
                text: buttonsText[i],
                callbackData: $"{correctAnswerIndex},{i},{questionIndex},{ticketIndex}")};
        }

        return new InlineKeyboardMarkup(buttons);
    }
}
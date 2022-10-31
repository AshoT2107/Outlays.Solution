using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Outlays.Bot.Helper;
using Outlays.Bot.Repository;
using Outlays.Bot.Services;
using Outlays.Data.Entities;
using Outlays.Data.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = Outlays.Data.Entities.User;

namespace Outlays.Bot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly TelegramBotService _botService;
        private readonly UserRepository _userRepository;
        private readonly RoomRepository _roomRepository;
        private readonly OutlayRepository _outlayRepository;

        public BotController(TelegramBotService botService,
                             UserRepository userRepository,
                             RoomRepository roomRepository,
                             OutlayRepository outlayRepository)
        {
            _botService = botService;
            _userRepository = userRepository;
            _roomRepository = roomRepository;
            _outlayRepository = outlayRepository;
        }

        [HttpGet]
        public IActionResult GetString()
        {
            return Ok("Working....");
        }
        [HttpPost]
        public async Task GetUpdates(Update update)
        {
            var (chatId, message, name) = GetUserInfo(update);
            if (update.Type != UpdateType.Message)
            {
                await _botService.SendMessageAsync(chatId, "this is invalid");
                return;
            }
            var user = await TypingUser(chatId, name);
            

            switch (user.Step)
            {
                case UserStep.NewUser: await SendMenuToUser(user);break;
                case UserStep.Menu: await MenuTextFilter(user,message);break;
                case UserStep.GetRoomName: await CreateNewRoom(user,message);break;
                case UserStep.CreateNewRoom:await OutlayMenuFilter(user, message);break;
                case UserStep.OutlayDetails: await AddOutlayDetailsToDb(user, message);break;
                case UserStep.AddOutlayDetails: await AddOutlayCostsToDb(user,message);break;
                case UserStep.OutlayCosts: await AddOutlayCostsToDb(user, message);break;
            }
        }

        private async Task AddOutlayCostsToDb(User user, string message)
        {
            
            var _user = await _userRepository.GetUserByChatId(user.ChatId);
            if (Int32.TryParse(message, out _))
            {
                var outlay = await _outlayRepository.GetOutlayByUserId(_user.Id);
                outlay.Cost = Int32.Parse(message);
                await _outlayRepository.UpdateOutlay(outlay);

                user.Step = UserStep.CreateNewRoom;
                await _userRepository.UpdateUser(user);
                var menu = _botService.GetKeyboard(new List<string>() { "Add Outlay", "Calculation" });
                await _botService.SendMessageAsync(user.ChatId, "Do you want to Add Outlay", menu);
            }
            else
            {
                await EnterOutlayCosts(user);
            }
            
        }

        private async Task EnterOutlayCosts(User user)
        {
            user.Step = UserStep.OutlayCosts;
            await _botService.SendTextMessageAsync(user.ChatId, "Enter costs of given outlays(sum)");
            await _userRepository.UpdateUser(user);
        }

        private async  Task AddOutlayDetailsToDb(User user, string message)
        {
            user.Step = UserStep.AddOutlayDetails;
            var room = await _roomRepository.GetRoomById(user.RoomId!.Value);
            if(room != null)
            {
                var outlay = new Outlay
                {
                    Description = message,
                    UserId = user.Id,
                    RoomId = user.RoomId
                };
                await _outlayRepository.AddOutlayAsync(outlay);
            }
            await _userRepository.UpdateUser(user);
            await _botService.SendTextMessageAsync(user.ChatId, "Enter costs of given outlays(sum)");
        }

        private async Task OutlayMenuFilter(User user, string message)
        {
            switch (message)
            {
                
                case "Add Outlay":await EnterOutlayDetails(user);break;
                case "Calculation": await Calculation(user); break;
            }
        }

        private async Task Calculation(User user)
        {
            string statistics = $"Statistics of {user.Name}\n\n";
            var outlays = await _outlayRepository.GetOutlaysOfUser(user.Id);
            foreach (var outlay in outlays)
            {
                statistics += $"Description: {outlay.Description}\n" +
                              $"Cost: {outlay.Cost}\n";
            }
            statistics += $"Total Sums: {user.Outlays.Sum(o => o.Cost)}";
            await _botService.SendTextMessageAsync(user.ChatId, statistics);
            user.Step = UserStep.CreateNewRoom;
            await _userRepository.UpdateUser(user);
            var menu = _botService.GetKeyboard(new List<string>() { "Add Outlay", "Calculation" });
            await _botService.SendMessageAsync(user.ChatId, "Do you want to Add Outlay", menu);
        }

        private async Task EnterOutlayDetails(User user)
        {
            user.Step = UserStep.OutlayDetails;
            await _userRepository.UpdateUser(user);
            await _botService.SendTextMessageAsync(user.ChatId, "Enter your own outlay details");
        }

        private async Task CreateNewRoom(User user,string roomName)
        {
            user.Step = UserStep.CreateNewRoom;
            var room = new Room()
            {
                Name = roomName,
                Key = RandomGenerator.RandomKey,
                Status = RoomStatus.Created,
            };
            await _roomRepository.AddRoomAsync(room);
            user.RoomId = room.Id;
            user.IsAdmin = true;
            await _userRepository.UpdateUser(user);
            var menu = _botService.GetKeyboard(new List<string>() { "Add Outlay", "Calculation" });
            await _botService.SendMessageAsync(user.ChatId,"Choose Menu",menu);
        }

        private async Task MenuTextFilter(User user, string message)
        {
            switch (message)
            {
                case "Create Room": await EnterRoomName(user);break;
                case "Join Room":;break;
            }
        }

        private async Task EnterRoomName(User user)
        {
            user.Step = UserStep.GetRoomName;
            await _userRepository.UpdateUser(user);
            await _botService.SendTextMessageAsync(user.ChatId, "Xona nomini kiriting");
        }

        private async Task SendMenuToUser(User user)
        {
            user.Step = UserStep.Menu;

            var menu = _botService.GetKeyboard(new List<string>() { "Create Room", "Join Room" });
            await _botService.SendMessageAsync(user.ChatId, "Choose Menu", menu);
            
            await _userRepository.UpdateUser(user);
        }

        private Tuple<long, string, string> GetUserInfo(Update update)
        {
            var chatId = update.Message!.From!.Id;
            var text = update.Message.Text!;
            var name = string.IsNullOrEmpty(update.Message.From.Username)
                                ? update.Message.From.FirstName
                                : update.Message.From.Username;
            return new(chatId, text, name);
        }
        private async Task<User> TypingUser(long chatId, string name)
        {
            var user = await _userRepository.GetUserByChatId(chatId);
            if (user == null)
            {
                user = new User()
                {
                    ChatId = chatId,
                    Name = name,
                };
                await _userRepository.AddUserAsync(user);
            }
            return user;
        }

    }

}


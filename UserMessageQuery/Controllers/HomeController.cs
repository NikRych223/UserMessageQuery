using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UserMessageQuery.Interfaces;
using UserMessageQuery.Models;

namespace UserMessageQuery.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMessageQuery _messageQuery;

        public HomeController(ILogger<HomeController> logger, IMessageQuery messageQuery)
        {
            _logger = logger;
            _messageQuery = messageQuery;
        }

        public IActionResult Index()
        {
            var users = _messageQuery.Users;
            return View(users);
        }

        [HttpGet]
        public IActionResult GetAllMessages()
        {
            var messages = _messageQuery.Users.SelectMany(user => user.Messages)
                .OrderBy(msg => msg.CreateTime).ToList();

            if (messages == null) return NotFound();

            return Ok(messages);
        }

        [HttpGet]
        public IActionResult GetMessagesByUserName(string userName)
        {
            try
            {
                var userMessages = _messageQuery.Users.FirstOrDefault(user => user.UserName == userName.Trim())
                .Messages.OrderBy(msg => msg.CreateTime).ToList();

                if (userMessages == null) return NotFound("user not have messages");

                return Ok(userMessages);
            }
            catch (NullReferenceException ex)
            {
                return NotFound("userName not found or empty");
            }
            catch(Exception ex)
            {
                return NotFound("Server error");
            }
        }

        [HttpPost]
        public IActionResult AddMessageToQuery(string userName, string message)
        {
            var currentUser = _messageQuery.Users.FirstOrDefault(user => user.UserName == userName.Trim());

            if (currentUser == null)
            {
                var newUser = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = userName.Trim(),
                    Messages = new List<Message>()
                };

                _messageQuery.Users.Add(newUser);
                currentUser = newUser;
            }

            var userMessage = new Message
            {
                Id = Guid.NewGuid().ToString(),
                Description = message,
                CreateTime = $"{DateTime.Now.Hour}/{DateTime.Now.Minute}/{DateTime.Now.Second}"
            };

            currentUser.Messages.Insert(0, userMessage);

            _messageQuery.RemoveOldMessageByUserName(10, userName.Trim());
            _messageQuery.RemoveOldMessage(20);

            var users = _messageQuery.Users;

            return Ok(users);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
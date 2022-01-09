using SignalRChat.Data;
using SignalRChat.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRChat.Hubs
{
    public class CustomChat : Hub
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IHttpContextAccessor _httpContext;

        public CustomChat(AppDbContext context, 
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IHttpContextAccessor httpContext)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContext = httpContext;
        }
        public async Task SendPrivateMessage(string receiverId, string message)
        {
            IdentityUser loggedInUser = await _userManager.GetUserAsync(_httpContext.HttpContext.User);

            await Clients.User(receiverId).SendAsync("ReceiveMessage", loggedInUser.UserName, message);

            Message datamessage = new Message()
            {
                Text = message,
                ReceiverId = receiverId,
                SenderId = loggedInUser.Id,
                CreatedDate = DateTime.Now/*.ToString("hh:mm t")*/
            };

            _context.Messages.Add(datamessage);
            _context.SaveChanges();
        }
        public async Task ShowTyping(string receiverId)
        {

            await Clients.User(receiverId).SendAsync("ShowTyping");
        }
        public async Task HideTyping(string receiverId)
        {

            await Clients.User(receiverId).SendAsync("HideTyping");
        }
    }
}

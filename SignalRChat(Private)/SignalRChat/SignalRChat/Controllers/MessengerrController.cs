using SignalRChat.Data;
using SignalRChat.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRChat.Controllers
{
    public class MessengerrController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public MessengerrController(AppDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            string senderId = _userManager.GetUserId(User);
            return View(_context.CustomUsers.Where(si=>si.Id!=senderId).ToList());
        }
        public IActionResult Chat(string receiverId)
        {
            string senderId = _userManager.GetUserId(User);
            VmMessage model = new VmMessage();
            model.User = _context.CustomUsers.Find(receiverId);
            model.Messages = _context.Messages.Where(m => m.SenderId == senderId && m.ReceiverId == receiverId || m.SenderId == receiverId && m.ReceiverId == senderId).ToList();
            model.SenderId = senderId;
            return View(model);
        }
    }
}

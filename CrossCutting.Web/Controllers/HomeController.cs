using System;
using System.Diagnostics;
using System.Linq;
using CrossCutting.Common.Database;
using CrossCutting.Common.Models;
using CrossCutting.Web.Factories;
using Microsoft.AspNetCore.Mvc;
using CrossCutting.Web.Models;

namespace CrossCutting.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<User> _userRepository;
        private readonly IUserFactory _userFactory;

        public HomeController(IRepository<User> userRepository, IUserFactory userFactory)
        {
            this._userRepository = userRepository;
            this._userFactory = userFactory;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Users()
        {
            var users = this._userRepository.GetAll();
            return View(users.Select(this._userFactory.CreateUserDto));
        }

        public IActionResult User(Guid id)
        {
            var user = this._userRepository.Get(id);
            return View(this._userFactory.CreateUserDto(user));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}
using DotNetEnglishP5_3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetEnglishP5_3.Controllers
{
    public class UserRolesController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserRolesController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserRolesViewModel>();
            foreach (IdentityUser user in users)
            {
                var thisViewModel = new UserRolesViewModel();
                thisViewModel.Email = user.Email;
                //thisViewModel.FirstName = user.FirstName;
                //thisViewModel.LastName = user.LastName;
                thisViewModel.Roles = await _userManager.GetRolesAsync(user);
                userRolesViewModel.Add(thisViewModel);
            }
            return View(userRolesViewModel);
        }
    }
}

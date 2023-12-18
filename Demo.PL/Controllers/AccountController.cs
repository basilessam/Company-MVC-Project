using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
			_userManager = userManager;
			_signInManager = signInManager;
		}
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    FName = model.FName,
                    LName = model.LName,
                    UserName = model.Email.Split('@')[0],
                    Email = model.Email,
                    IsAgree=model.IsAgree,
                };

                var result= await _userManager.CreateAsync(user,model.Password);

                if(result.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
      
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user= await _userManager.FindByEmailAsync(model.Email);
                if(user is not null)
                {
                    var flag= await _userManager.CheckPasswordAsync(user, model.Password);
                    if (flag)
                    {
                        await _signInManager.PasswordSignInAsync(user, model.Password, model.RemeberMe, false);
                        return RedirectToAction("Index","Home");
                    }

                    ModelState.AddModelError(string.Empty, "Invaild Password");
                }
                ModelState.AddModelError(string.Empty, "Email is not Exited"); 
            }
            return View(model);
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail( ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user= await _userManager.FindByEmailAsync(model.Email);
                if( user is not null )
                {
                    var token=_userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordRestLink = Url.Action("RestPassword", "Account",new {email=user.Email,token },"https", "localhost:44367");
                    //Account
                    var email = new Email()
                    {
                        Subject = "Reset Password",
                        To = user.Email,
                        Body = passwordRestLink,
                    };
                    EmailSetting.SendEmail(email);
                    return RedirectToAction(nameof(CheckYourInbox));
                }
                ModelState.AddModelError(string.Empty, "Email is not Existed");
            }

            return View(model);
        }
        public IActionResult CheckYourInbox()
        {
            return View();
        }
	}
}

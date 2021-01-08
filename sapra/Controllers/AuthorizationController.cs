using Microsoft.AspNetCore.Mvc;
using sapra.Models;
using sapra.ViewModels;

namespace sapra.Controllers
{

	public class AuthorizationController : SystemController
	{
		[HttpGet]
		public IActionResult Login()
		{
			var settings = LoadSettings();
			ViewBag.MainTitle = settings.Title;
			ViewBag.Subtitle = settings.Subtitle;

			if (new SessionController().GetSessionRole(HttpContext) > 0)
			{
				TempData.Clear();
				return RedirectToAction("Index", "Administrator");
			}

			return View(new LoginViewModel());
		}

		public IActionResult ForceRequestLogin(){ 
			return RedirectToAction("Login", "Authorization");
		}

		[HttpGet]
		public IActionResult Logout()
		{
			new SessionController().Logout(HttpContext);
			return RedirectToAction("Login");
		}

		[HttpPost]
		public IActionResult Login(LoginViewModel model)
		{
			var status = new SessionController().TryLogin(HttpContext, model.Email, model.Password);
			if (status > 0) 
			{
				TempData.Clear();
				return RedirectToAction("Index", "Administrator");
			}
			else if(status < 0) 
			{
				model.Response = new ServerResponseViewModel("Se ha intentando iniciar muchas veces sesión con ese usuario. Contacte un administrador.", ResponseType.Error);
			}
			else
			{
				model.Response = new ServerResponseViewModel("Correo o contraseña incorrecta.", ResponseType.Error);
			}

			var settings = LoadSettings();
			ViewBag.MainTitle = settings.Title;
			ViewBag.Subtitle = settings.Subtitle;

			return View(model);
		}
	}
}

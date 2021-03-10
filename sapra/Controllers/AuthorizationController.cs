using Microsoft.AspNetCore.Mvc;
using System.Linq;
using sapra.App;
using sapra.Models;
using sapra.ViewModels;
using System;
using sapra.App.Helpers;
using System.Threading.Tasks;

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

			if (SessionController.GetSessionVariable(HttpContext) > 0)
			{
				TempData.Clear();
				return RedirectToAction("Map", "Zone");
			}

			return View(new LoginViewModel(){ Response = GetResponseFromRedirect() });
		}

		[HttpGet]
		public IActionResult RequestRestorePassword()
		{
			if (SessionController.GetSessionVariable(HttpContext) > 0)
			{
				TempData.Clear();
				return RedirectToAction("Map", "Zone");
			}

			return View(new RestorePasswordViewModel());
		}

		[HttpPost]
		public async Task<IActionResult> RequestRestorePassword(RestorePasswordViewModel model)
		{
			using var db = new DatabaseContext();

			var user = db.UserRepository.Where(e => e.UserInfo.Email == model.Email).SingleOrDefault();
			if (user != null)
			{
				user.RecoveryHash = CryptographyHelper.CreateSalt(32);
				db.SaveChanges();

				var settings = LoadSettings();

				var body = "Haga click aqui para recuperar su contraseña: " +
				"https://localhost:44349/Authorization/RestorePassword?token=" + user.RecoveryHash;

				await SendEmail(model.Email, "[" + settings.Title + " " + settings.Subtitle +
				"] Recuperación de Contraseña", body);
			}

			TempData["Response"] = "Verifique su correo electrónico.";
			return ForceRequestLogin();
		}

		[HttpGet]
		public IActionResult RestorePassword(string token, bool ftm = false)
		{
			using var db = new DatabaseContext();

			if (SessionController.GetSessionVariable(HttpContext) > 0)
			{
				TempData.Clear();
				return RedirectToAction("Map", "Zone");
			}

			if (string.IsNullOrEmpty(token))
			{
				return ForceRequestLogin();
			}

			var model = new RestorePasswordViewModel();
			var user = db.UserRepository.Where(e => e.RecoveryHash == token).SingleOrDefault();

			if (user == null)
			{
				return ForceRequestLogin();
			}

			model.Token = token;
			model.FirstTimeMode = ftm;

			return View(model);
		}

		public IActionResult ForceRequestLogin(){ 
			return RedirectToAction("Login", "Authorization");
		}

		[HttpGet]
		public IActionResult Logout()
		{
			SessionController.Logout(HttpContext);
			return RedirectToAction("Login");
		}

		[HttpPost]
		public IActionResult Login(LoginViewModel model)
		{
			using var db = new DatabaseContext();

			var settings = LoadSettings();
			ViewBag.MainTitle = settings.Title;
			ViewBag.Subtitle = settings.Subtitle;

			switch (SessionController.TryLogin(HttpContext, model.Email, model.Password))
			{
				case SessionController.IS_LOGGED:
				case SessionController.SUCCESS:
					TempData.Clear();
					return RedirectToAction("Map", "Zone");

				case SessionController.FIRST_LOGIN:
					var user = db.UserRepository.Where(e => e.UserInfo.Email == model.Email).SingleOrDefault();
					return RedirectToAction("RestorePassword", new { token = user.RecoveryHash, ftm = true });

				case SessionController.NOT_FOUND:
					model.Response = new ServerResponseViewModel("Correo o contraseña incorrecta.", ResponseType.Error);
					break;

				case SessionController.TOO_MANY_TRIES:
					model.Response = new ServerResponseViewModel("Se ha intentando iniciar muchas veces sesión con ese usuario. Contacte un administrador.", ResponseType.Error);
					break;

			}
			return View(model);
		}

		[HttpPost]
		public IActionResult RestorePassword(RestorePasswordViewModel model)
		{
			using var db = new DatabaseContext();

			if (model.Password == model.ConfirmPassword)
			{
				var user = db.UserRepository.Where(e => e.RecoveryHash == model.Token).SingleOrDefault();
				user.Salt = CryptographyHelper.CreateSalt(32);
				user.Hash = CryptographyHelper.Hash(model.Password + user.Salt);
				user.LastLogin = DateTime.Now;
				user.RecoveryHash = CryptographyHelper.CreateSalt(32);
				db.SaveChanges();
				TempData["Response"] = "Contraseña actualizada";
				return RedirectToAction("Login", "Authorization");
			}
			model.Response = new ServerResponseViewModel("Ha ocurrido un error.", ResponseType.Error);
			return View(model);
		}

		public static bool IsAllowed(int roleId, Permission permission) 
		{
			using var db = new DatabaseContext();

			var obj = db.RolePermissionRepository.Where(e => e.RoleId == roleId).
			Where(e => e.PermissionName == permission.ToString()).SingleOrDefault();

			return (obj != null);
		}
	}
}

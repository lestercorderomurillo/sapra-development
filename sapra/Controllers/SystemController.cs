using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sapra.Models;
using sapra.App.Helpers;
using sapra.App;
using sapra.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace sapra.Controllers
{

	public class SystemController : Controller
	{

		[HttpGet]
		public IActionResult Index()
		{
			if (new SessionController().GetSessionRole(HttpContext) > 0)
			{
				return View();
			}
			return new AuthorizationController().ForceRequestLogin();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpPost]
		public User RequestUser(int userId, bool includeExtraInformation = false)
		{
			var db = new DatabaseContext();
			var user = db.UserRepository.Where(e => e.UserId == userId).SingleOrDefault();

			if (includeExtraInformation) 
			{
				var userInfo = db.UserInfoRepository.Where(e => e.UserId == userId).SingleOrDefault();
				var userPhones = db.UserPhoneRepository.Where(e => e.UserId == userId).ToList();
				userInfo.PhoneNumbers = userPhones;
				user.SetUserInfo(userId, userInfo);
			}
			
			return user;
		}

		[HttpPost]
		public List<User> RequestAllUsers(string searchQuery = "")
		{
			var db = new DatabaseContext();
			var users = db.UserRepository.ToList();
			var usersComplex = new List<User>();

			foreach(var user in users) 
			{
				usersComplex.Add(RequestUser(user.UserId, true));
			}

			return usersComplex;
		}

		[HttpPost]
		public Role RequestRole(int roleId, bool includePermissions = false)
		{
			var db = new DatabaseContext();
			var role = db.RoleRepository.Where(e => e.RoleId == roleId).SingleOrDefault();

			if (role != null)
			{
				if (includePermissions)
				{
					var permissions = new List<string>();

					if (role.ParentRoleId != 0)
					{
						var parentRole = RequestRole(role.ParentRoleId, true);
						permissions.AddRange(parentRole.Permissions);
					}

					var permissionsFromDb = db.RolePermissionRepository.Where(e => e.RoleId == roleId).Select(e => e.PermissionName).ToList();
					permissions.AddRange(permissionsFromDb);

					role.Permissions = permissions;
				}
				else
				{
					role.Permissions = new List<string>();
				}
			}
			else
			{ 
				return null;
			}

			return role;
		}

		public ServerResponseViewModel GetResponseFromRedirect(ResponseType responseType = ResponseType.Success)
		{
			if (TempData["response"] != null)
			{
				var response = (string)(TempData["response"]);
				return new ServerResponseViewModel(response, responseType);
			}
			return new ServerResponseViewModel(string.Empty, ResponseType.None);
		}

		[HttpGet]
		public IActionResult AccessDeniedView()
		{
			return View("Response", new ServerResponseViewModel("Acceso Denegado", ResponseType.Error).ExtendFull());
		}

		#pragma warning disable CS0618
		public const string fileName = "globalSettings.json";
		public static IHostingEnvironment env = Startup.RootAccess.Environment;
		public static string webRoot = env.WebRootPath;

		public void SaveSettings(GeneralSettingsModel model) 
		{
			var file = System.IO.Path.Combine(webRoot, fileName);
			string json = JsonConvert.SerializeObject(model);
			System.IO.File.WriteAllText(file, json);
		}

		public GeneralSettingsModel LoadSettings()
		{
			var file = System.IO.Path.Combine(webRoot, fileName);
			var json = System.IO.File.ReadAllText(file);
			return JsonConvert.DeserializeObject<GeneralSettingsModel>(json);
			
		}


	}
}

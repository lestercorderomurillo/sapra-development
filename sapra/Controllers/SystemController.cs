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
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mail;
using System.Net;

namespace sapra.Controllers
{

	public class SystemController : Controller
	{

		[HttpGet]
		public IActionResult Index()
		{
			if (SessionController.GetSessionVariable(HttpContext) > 0)
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
		public static User RequestUser(int userId, bool includeExtraInformation = false)
		{
			using var db = new DatabaseContext();

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

		public MapLayer RequestLayer(int layerId, bool includeExtraInformation = false)
		{
			using var db = new DatabaseContext();

			var layer = db.MapLayerRepository.Where(e => e.MapLayerId == layerId).SingleOrDefault();

			if (includeExtraInformation)
			{
				var fields = db.MapLayerFieldRepository.Where(e => e.MapLayerId == layerId).ToList();
				layer.MapLayerFields = fields;
			}
			return layer;

		}

		[HttpPost]
		public List<MapLayer> RequestAllMapLayers(int page = 0, bool requestOnlyVisible = true)
		{
			using var db = new DatabaseContext();

			var pp = 8;
			var offset = page * pp;
			var list = new List<MapLayer>();

			if (requestOnlyVisible)
			{
				list = db.MapLayerRepository.Where(e => e.Visible == 0).Skip(offset).Take(pp).ToList();
			}
			else
			{
				list = db.MapLayerRepository.Skip(offset).Take(pp).ToList();
			}

			var found = list.Find(e => e.Name == LoadSettings().BaseLayer);
			if (found != null)
			{
				found.MapLayerFields = RequestLayer(found.MapLayerId, true).MapLayerFields;
			}

			return list;
		}

		[HttpPost]
		public List<User> RequestAllUsers(string searchQuery = "", int page = 0)
		{

			using var db = new DatabaseContext();

			var pp = 8;
			var offset = page * pp;
			var users = new List<User>();

			if (searchQuery != null && searchQuery.Length > 0)
			{
				users = db.UserRepository.Where(e => e.UserInfo.FirstName.Contains(searchQuery)).Skip(offset).Take(pp).ToList();
			}
			else
			{
				users = db.UserRepository.Skip(offset).Take(pp).ToList();
			}

			var usersComplex = new List<User>();

			foreach (var user in users)
			{
				usersComplex.Add(RequestUser(user.UserId, true));
			}

			return usersComplex;
		}

		[HttpPost]
		public Role RequestRole(int roleId, bool includePermissions = false)
		{
			using var db = new DatabaseContext();

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

		public static void SaveSettings(GeneralSettingsModel model) 
		{
			var file = System.IO.Path.Combine(webRoot, fileName);
			string json = JsonConvert.SerializeObject(model);
			System.IO.File.WriteAllText(file, json);
		}

		public static GeneralSettingsModel LoadSettings()
		{
			var file = System.IO.Path.Combine(webRoot, fileName);
			var json = System.IO.File.ReadAllText(file);
			return JsonConvert.DeserializeObject<GeneralSettingsModel>(json);
		}

		public async Task SendEmail(string dst, string title, string body)
		{
			var message = new MailMessage("sapradevelopment@gmail.com", dst)
			{
				Subject = title,
				Body = body
			};

			var smtpClient = new SmtpClient("smtp.gmail.com", 25) {
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				UseDefaultCredentials = true,
				Credentials = new NetworkCredential("sapradevelopment@gmail.com", "play3657x")
			};
			
			await smtpClient.SendMailAsync(message);
			
		}

	}
}

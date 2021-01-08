using System;
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using sapra.App;
using sapra.Models;
using sapra.ViewModels;
using sapra.App.Helpers;
using Newtonsoft.Json;

namespace sapra.Controllers
{

	public class AdministratorController : SystemController
	{

		[HttpGet]
		public IActionResult Administrator(string tab = "General")
		{
			if (new SessionController().GetSessionRole(HttpContext) > 0) 
			{
				ViewBag.Tab = tab;
				return View();
			}
			return new AuthorizationController().ForceRequestLogin();
		}

		[HttpPost]
		public IActionResult TabGeneral()
		{
			if (new SessionController().GetSessionRole(HttpContext) > 0) 
			{
				var model = LoadSettings();
				model.Response = GetResponseFromRedirect();
				return PartialView(model);
			}
			return AccessDeniedView();
		}

		[HttpPost]
		public IActionResult TabRoles()
		{
			if (new SessionController().GetSessionRole(HttpContext) > 0) 
			{
				var model = new RoleListViewModel(new DatabaseContext().RoleRepository.ToList());
				model.Response = GetResponseFromRedirect();

				return PartialView(model);
			}
			return AccessDeniedView();
		}

		[HttpPost]
		public IActionResult TabUsers(string searchQuery = "")
		{
			if (new SessionController().GetSessionRole(HttpContext) > 0) {
				var model = new UserListViewModel(RequestAllUsers(searchQuery));
				model.Response = GetResponseFromRedirect();
				
				return PartialView(model);
			}
			return AccessDeniedView();
		}

		[HttpPost]
		public IActionResult TabZone()
		{
			if (new SessionController().GetSessionRole(HttpContext) > 0) {
				return PartialView();
			}
			return AccessDeniedView();
		}

		[HttpPost]
		public IActionResult EditUser(int userId)
		{
			User user;

			if (userId == 0)
			{
				user = new User();
			}
			else
			{
				user = RequestUser(userId, true);
			}

			var PhoneNumbers = new List<string>();
			var PhoneNumbersType = new List<string>();

			for (int i = 0; i < user.UserInfo.PhoneNumbers.Count; i++) 
			{
				PhoneNumbers.Add(user.UserInfo.PhoneNumbers[i].Number);
				PhoneNumbersType.Add(user.UserInfo.PhoneNumbers[i].Type);
			}

			ViewBag.PhoneNumbers     = JsonConvert.SerializeObject(PhoneNumbers);
			ViewBag.PhoneNumbersType = JsonConvert.SerializeObject(PhoneNumbersType);

			ViewBag.Roles = new DatabaseContext().RoleRepository.ToList();

			var model = new UserEditViewModel(user);
			return PartialView("EditUser", model);
		}

		[HttpPost]
		public IActionResult ViewRole(int roleId) 
		{ 
			if(roleId < 1) 
			{
				return AccessDeniedView();
			}
			else 
			{
				Role role = RequestRole(roleId, true);
				return PartialView(role);
			}
		}

		[HttpPost]
		public IActionResult EditRole(int roleId)
		{
			Role role;
			var db = new DatabaseContext();

			if (roleId == 0)
			{
				role = new Role();
			}
			else
			{
				role = db.RoleRepository.Where(e => e.RoleId == roleId).SingleOrDefault();
			}

			var permissions = new List<string>();
			var selected = new List<bool>();

			foreach (Permission permission in EnumParser.GetAllValues<Permission>())
			{
				permissions.Add(permission.ToString());
			}

			var permissionsFromDb = db.RolePermissionRepository.Where(e => e.RoleId == roleId).Select(e => e.PermissionName).ToList();

			ViewBag.Permissions = permissions;
			foreach (var p in permissions) 
			{
				selected.Add(permissionsFromDb.Contains(p));
			}
			ViewBag.Selected = selected;
			ViewBag.Roles = new DatabaseContext().RoleRepository.ToList();

			return PartialView("EditRole", role);
		}

		[HttpPost]
		public IActionResult DeleteRole(int roleId)
		{
			if (roleId < 1)
			{
				return AccessDeniedView();
			}
			else
			{
				var db = new DatabaseContext();
				Role role = db.RoleRepository.Where(e => e.RoleId == roleId).SingleOrDefault();
				db.RoleRepository.Remove(role);
				db.SaveChanges();
				var model = new RoleListViewModel(new DatabaseContext().RoleRepository.ToList());
				model.Response = new ServerResponseViewModel("Se ha eliminado el rol correctamente.", ResponseType.Success);
				return PartialView("TabRoles", model);
			}

		}

		[HttpPost]
		public IActionResult SubmitRoleUpdate(Role role)
		{
			if (role != null)
			{
				if (role.RoleId == 0)
				{
					/* INSERT */
					var db = new DatabaseContext();
					db.RoleRepository.Add(role);
					db.SaveChanges();

					foreach (var permission in role.Permissions)
					{
						db.RolePermissionRepository.Add(new Role_Permission(role.RoleId, permission.ToUpper()));
					}
					db.SaveChanges();
					TempData["response"] = "Se ha agregado el rol correctamente.";
				}
				else
				{
					/* UPDATE */
					var db = new DatabaseContext();
					var model = db.RoleRepository.Where(e => e.RoleId == role.RoleId).SingleOrDefault();
					db.Entry(model).CurrentValues.SetValues(role);
					db.RolePermissionRepository.RemoveRange(db.RolePermissionRepository.Where(e => e.RoleId == role.RoleId));
					db.SaveChanges();

					foreach (var permission in role.Permissions)
					{
						db.RolePermissionRepository.Add(new Role_Permission(role.RoleId, permission.ToUpper()));
					}
					db.SaveChanges();

					TempData["response"] = "Se ha modificado el rol correctamente.";
				}

				return RedirectToAction("Administrator", new { tab = "Roles" });
			}
			return AccessDeniedView();
		}

		[HttpPost]
		public IActionResult SubmitUserUpdate(UserEditViewModel userEditViewModel)
		{
			if (userEditViewModel != null)
			{
				var user = userEditViewModel.User;
				if (user.UserId == 0)
				{
					/* INSERT */
					var db = new DatabaseContext();
					user.Salt = CryptographyHelper.CreateSalt(32);
					user.Hash = CryptographyHelper.Hash(userEditViewModel.Password + user.Salt);
					user.LoginAttemptRecoveryTimestamp = DateTime.Now;
					user.LoginAttempts = 0;

					db.UserRepository.Add(user);
					db.SaveChanges();

					TempData["response"] = "Se ha agregado el usuario correctamente.";
				}
				else
				{
					/*/* UPDATE */
					/*var db = new DatabaseContext();
					var model = db.RoleRepository.Where(e => e.RoleId == role.RoleId).SingleOrDefault();
					db.Entry<Role>(model).CurrentValues.SetValues(role);
					db.SaveChanges();
					TempData["response"] = "Se ha modificado el rol correctamente.";*/
				}

				return RedirectToAction("Administrator", new { tab = "Users" });
			}
			return AccessDeniedView();
		}

		[HttpPost]
		public IActionResult SubmitSettingsUpdate(GeneralSettingsModel model)
		{
			if (ModelState.IsValid)
			{
				SaveSettings(model);
			}
			TempData["response"] = "Se ha actualizado correctamente la información.";
			return RedirectToAction("Administrator", new { tab = "General" });
		}

	}
}

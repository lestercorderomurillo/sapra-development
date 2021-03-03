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
		// Panel Actions
		[HttpGet]
		public IActionResult Panel(string tab = "General")
		{
			if (new SessionController().GetSessionRole(HttpContext) > 0) 
			{
				if(ControllerHelper.ViewExists(this, "Tab" + tab)) 
				{
					ViewBag.Tab = tab;
				}
				else 
				{
					return AccessDeniedView();
				}

				return View();
			}
			return new AuthorizationController().ForceRequestLogin();
		}

		// Tabs Actions
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
				var model = new RoleListViewModel(new DatabaseContext().RoleRepository.ToList())
				{
					Response = GetResponseFromRedirect()
				};

				return PartialView(model);
			}
			return AccessDeniedView();
		}

		[HttpPost]
		public IActionResult TabUsers(string searchQuery = "")
		{
			if (new SessionController().GetSessionRole(HttpContext) > 0) {
				var model = new UserListViewModel(RequestAllUsers(searchQuery))
				{
					Response = GetResponseFromRedirect(),
					SearchQuery = searchQuery
				};
				return PartialView(model);
			}
			return AccessDeniedView();
		}

		[HttpPost]
		public IActionResult TabZone()
		{
			if (new SessionController().GetSessionRole(HttpContext) > 0) {
				var model = new MapLayerListViewModel(RequestAllMapLayers(0, false))
				{
					Response = GetResponseFromRedirect()
				};
				return PartialView(model);
			}
			return AccessDeniedView();
		}

		// View Actions
		[HttpPost]
		public IActionResult ViewUser(int userId)
		{
			if (userId < 1)
			{
				return AccessDeniedView();
			}
			else
			{
				return PartialView(RequestUser(userId, true));
			}
		}

		[HttpPost]
		public IActionResult ViewRole(int roleId)
		{
			if (roleId < 1)
			{
				return AccessDeniedView();
			}
			else
			{
				return PartialView(RequestRole(roleId, true));
			}
		}

		[HttpPost]
		public IActionResult ViewLayer(int mapLayerId)
		{
			if (mapLayerId < 1)
			{
				return AccessDeniedView();
			}
			else
			{
				return PartialView(RequestLayer(mapLayerId, true));
			}
		}

		// Edit (on Load) Actions
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
		public IActionResult EditLayer(int mapLayerId)
		{
			MapLayer layer;
			var db = new DatabaseContext();

			if (mapLayerId == 0)
			{
				layer = new MapLayer();
			}
			else
			{
				layer = RequestLayer(mapLayerId, true);
			}

			var MapLayerFieldsAlias = new List<string>();
			var MapLayerFieldsNames = new List<string>();
			var MapLayerFieldsTypes = new List<int>();

			if (layer.MapLayerFields != null) 
			{
				for (int i = 0; i < layer.MapLayerFields.Count; i++)
				{
					MapLayerFieldsAlias.Add(layer.MapLayerFields[i].Alias);
					MapLayerFieldsNames.Add(layer.MapLayerFields[i].Name);
					MapLayerFieldsTypes.Add(layer.MapLayerFields[i].Type);
				}
			}

			ViewBag.MapLayerFieldsAlias = JsonConvert.SerializeObject(MapLayerFieldsAlias);
			ViewBag.MapLayerFieldsNames = JsonConvert.SerializeObject(MapLayerFieldsNames);
			ViewBag.MapLayerFieldsTypes = JsonConvert.SerializeObject(MapLayerFieldsTypes);

			return PartialView("EditLayer", layer);
		}

		// Delete Actions
		[HttpPost]
		public IActionResult DeleteUser(int userId)
		{
			if (userId < 1)
			{
				return AccessDeniedView();
			}
			else
			{
				var db = new DatabaseContext();
				User user = db.UserRepository.Where(e => e.UserId == userId).SingleOrDefault();
				db.UserRepository.Remove(user);
				db.SaveChanges();
				var model = new UserListViewModel(RequestAllUsers())
				{
					Response = new ServerResponseViewModel("Se ha eliminado el usuario correctamente. ", ResponseType.Success)
				};
				return PartialView("TabUsers", model);
			}
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
				var UsersUsingRole = new DatabaseContext().UserRepository.Where(e => e.RoleId == roleId).ToList().Count();
				if (UsersUsingRole == 0) 
				{
					var db = new DatabaseContext();
					Role role = db.RoleRepository.Where(e => e.RoleId == roleId).SingleOrDefault();
					db.RoleRepository.Remove(role);
					db.SaveChanges();
					var model = new RoleListViewModel(new DatabaseContext().RoleRepository.ToList())
					{
						Response = new ServerResponseViewModel("Se ha eliminado el rol correctamente. ", ResponseType.Success)
					};
					return PartialView("TabRoles", model);
				}
				else 
				{
					var model = new RoleListViewModel(new DatabaseContext().RoleRepository.ToList())
					{
						Response = new ServerResponseViewModel("Al menos " + UsersUsingRole + " usuarios tienen asignados este rol, debe eliminar dichos usuarios si quiere eliminar ese rol.", ResponseType.Error)
					};
					return PartialView("TabRoles", model);
				}
			}
		}

		[HttpPost]
		public IActionResult DeleteLayer(int mapLayerId)
		{
			if (mapLayerId < 1)
			{
				return AccessDeniedView();
			}
			else
			{
				var db = new DatabaseContext();
				var layer = db.MapLayerRepository.Where(e => e.MapLayerId == mapLayerId).SingleOrDefault();
				db.MapLayerRepository.Remove(layer);
				db.SaveChanges();
				var model = new MapLayerListViewModel(RequestAllMapLayers())
				{
					Response = new ServerResponseViewModel("Se ha eliminado la capa correctamente. ", ResponseType.Success)
				};
				return PartialView("TabZone", model);
			}
		}

		// Edit (on Submit) Actions
		[HttpPost]
		public IActionResult SubmitUserUpdate(UserEditViewModel userEditViewModel)
		{
			if (userEditViewModel != null)
			{
				var user = userEditViewModel.User;
				var db = new DatabaseContext();

				if (user.UserId == 0)
				{
					/* INSERT */
					user.Salt = CryptographyHelper.CreateSalt(32);
					user.Hash = CryptographyHelper.Hash(userEditViewModel.Password + user.Salt);
					user.LoginAttempts = 0;

					db.UserRepository.Add(user);
					db.SaveChanges();

					TempData["response"] = "Se ha agregado el usuario correctamente.";
				}
				else
				{
					/* UPDATE */
					var syncUser = db.UserRepository
						.Where(e => e.UserId == user.UserId).SingleOrDefault();

					var syncUserInfo = db.UserInfoRepository
						.Where(e => e.UserId == user.UserId).SingleOrDefault();

					syncUserInfo.FirstName = user.UserInfo.FirstName;
					syncUserInfo.LastName = user.UserInfo.LastName;
					syncUserInfo.Email = user.UserInfo.Email;
					syncUserInfo.TypeIdentity = user.UserInfo.TypeIdentity;
					syncUserInfo.Identity = user.UserInfo.Identity;
					syncUser.RoleId = user.RoleId;
					syncUserInfo.Gender = user.UserInfo.Gender;

					db.SaveChanges();

					TempData["response"] = "Se ha modificado el usuario correctamente.";
				}

				return RedirectToAction("Panel", new { tab = "Users" });
			}
			return AccessDeniedView();
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

				return RedirectToAction("Panel", new { tab = "Roles" });
			}
			return AccessDeniedView();
		}

		[HttpPost]
		public IActionResult SubmitLayerUpdate(MapLayer layer)
		{
			if (layer != null)
			{
				var db = new DatabaseContext();

				if (layer.MapLayerId == 0)
				{
					/* INSERT */
					db.MapLayerRepository.Add(layer);
					db.SaveChanges();
					TempData["response"] = "Se ha agregado la capa correctamente.";
				}
				else
				{
					/* UPDATE */
					var syncLayer = db.MapLayerRepository
						.Where(e => e.MapLayerId == layer.MapLayerId).SingleOrDefault();

					db.Entry(syncLayer).CurrentValues.SetValues(layer);
					db.SaveChanges();

					db.MapLayerFieldRepository.RemoveRange(db.MapLayerFieldRepository.Where(e => e.MapLayerId == layer.MapLayerId));
					db.SaveChanges();

					foreach (var field in layer.MapLayerFields)
					{
						db.MapLayerFieldRepository.Add(new MapLayerField(){ 
							Name = field.Name, 
							Alias = field.Alias, 
							Type = field.Type, 
							MapLayerId = layer.MapLayerId
						});
					}
					db.SaveChanges();

					TempData["response"] = "Se ha modificado la capa correctamente.";
				}

				return RedirectToAction("Panel", new { tab = "Zone" });
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
			return RedirectToAction("Panel", new { tab = "General" });
		}
	}
}

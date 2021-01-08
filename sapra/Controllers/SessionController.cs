using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sapra.App;
using sapra.App.Helpers;
using sapra.Models;
using System.Linq;

namespace sapra.Controllers
{

	public class SessionController : SystemController
	{
		public int TryLogin(HttpContext context, string email, string password)
		{
			var db = new DatabaseContext();
			var userInfo = db.UserInfoRepository.Where(e => e.Email == email).SingleOrDefault();

			if (userInfo != null) 
			{
				var user = db.UserRepository.Where(e => e.UserInfo == userInfo).SingleOrDefault();

				if(user.LoginAttempts >= 8) 
				{
					return -1;
				}

				if (user.Hash == CryptographyHelper.Hash(password + user.Salt))
				{
					user.LoginAttempts = 0;
					db.SaveChanges();
					context.Session.SetInt32("RoleId", user.RoleId);
					context.Session.SetInt32("UserId", user.UserId);
					return 1;
				}
				else 
				{
					user.LoginAttempts += 1;
					db.SaveChanges();
				}
			}

			return 0;
		}

		public int GetSessionRole(HttpContext context)
		{
			if (context == null || context.Session == null) { return 0; }
			return (context.Session.GetInt32("RoleId") != null) ? (int)context.Session.GetInt32("RoleId") : 0;
		}

		public void Logout(HttpContext context)
		{
			context.Session.Clear();
		}

	}
}

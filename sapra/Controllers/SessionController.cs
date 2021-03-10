using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sapra.App;
using sapra.App.Helpers;
using sapra.Models;
using System;
using System.Linq;

namespace sapra.Controllers
{

	public class SessionController : SystemController
	{

		public const int SUCCESS = 0;

		public const int NOT_FOUND = 1;

		public const int TOO_MANY_TRIES = 2;

		public const int FIRST_LOGIN = 3;

		public const int IS_LOGGED = 4;

		public static int TryLogin(HttpContext context, string email, string password)
		{
			using var db = new DatabaseContext();

			if (GetSessionVariable(context) > 0)
			{
				return IS_LOGGED;
			}

			var userInfo = db.UserInfoRepository.Where(e => e.Email == email).SingleOrDefault();

			if (userInfo != null)
			{
				var user = db.UserRepository.Where(e => e.UserInfo == userInfo).SingleOrDefault();

				if (user.LoginAttempts >= 8)
				{
					return TOO_MANY_TRIES;
				}

				if (user.Hash == CryptographyHelper.Hash(password + user.Salt))
				{
					user.LoginAttempts = 0;
					db.SaveChanges();

					if (user.LastLogin < new DateTime(1900, 1, 1))
					{
						user.RecoveryHash = CryptographyHelper.CreateSalt(32);
						db.SaveChanges();
						return FIRST_LOGIN;
					}

					context.Session.SetInt32("RoleId", user.RoleId);
					context.Session.SetInt32("UserId", user.UserId);
					return SUCCESS;
				}
				else
				{
					user.LoginAttempts += 1;
					db.SaveChanges();
				}
			}

			return NOT_FOUND;
		}

		public static int GetSessionVariable(HttpContext context, string variable = "RoleId")
		{
			if (context == null || context.Session == null) { return 0; }
			return (context.Session.GetInt32(variable) != null) ? (int)context.Session.GetInt32(variable) : 0;
		}

		public static void Logout(HttpContext context)
		{
			context.Session.Clear();
		}

	}
}

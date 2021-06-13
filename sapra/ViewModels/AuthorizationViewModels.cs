using sapra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sapra.ViewModels
{

	// Login and restore Passwords ViewModels
	public class LoginViewModel : BaseViewModel
	{
		public string Email { get; set; }

		public string Password { get; set; }

		public LoginViewModel() {
			Email = string.Empty;
			Password = string.Empty;
		}

	}

	public class RestorePasswordViewModel : LoginViewModel
	{
		public bool FirstTimeMode { get; set; }

		public string Token { get; set; }

		public string ConfirmPassword { get; set; }
	}

}

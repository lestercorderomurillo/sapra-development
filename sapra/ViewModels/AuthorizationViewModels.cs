using sapra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sapra.ViewModels
{

	public abstract class BaseViewModel 
	{ 
		public ServerResponseViewModel Response { get; set; }

		public BaseViewModel() {
			Response = new ServerResponseViewModel();
		}
	}

	public class LoginViewModel : BaseViewModel
	{
		public string Email { get; set; }
		public string Password { get; set; }

		public LoginViewModel() {
			Email = string.Empty;
			Password = string.Empty;
		}

	}

}

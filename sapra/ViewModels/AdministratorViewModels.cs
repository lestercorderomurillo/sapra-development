using sapra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sapra.ViewModels
{
	public class MapLayerListViewModel : BaseViewModel
	{
		public string BaseUrl { get; set; }

		public List<MapLayer> Layers { get; set; }

		public MapLayerListViewModel(List<MapLayer> layers)
		{
			Layers = layers;
		}
	}

	public class RoleListViewModel : BaseViewModel
	{
		public List<Role> Roles { get; set; }

		public RoleListViewModel(List<Role> roles) {
			Roles = roles;
		}
	}

	public class UserListViewModel : BaseViewModel
	{
		public string SearchQuery { get; set; }

		public List<User> Users { get; set; }

		public UserListViewModel(List<User> users)
		{
			Users = users;
		}
	}

	public class UserEditViewModel : BaseViewModel
	{
		public User User { get; set; }

		public string Password { get; set; }

		public string ConfirmPassword { get; set; }

		public UserEditViewModel()
		{
			User = new User();
			Password = string.Empty;
		}

		public UserEditViewModel(User user)
		{
			User = user;
			Password = string.Empty;
			ConfirmPassword = string.Empty;
		}
	}
}

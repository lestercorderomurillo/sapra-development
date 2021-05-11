using sapra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sapra.ViewModels
{
	public class MapShowcaseViewModel : BaseViewModel 
	{
		public int QueryFID;

		public MapLayerField Fields;
		public User User;
		public string Transaction;
		public string ActionStr; /*Vivienda*/
		public String DateStr; /*Al ser las @DateTime.Now.ToShortTimeString() del dia @DateTime.Now.ToLongDateString()*/
		public bool Accepted;

	}

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

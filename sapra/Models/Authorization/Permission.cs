using System;
using System.Collections.Generic;

namespace sapra.Models
{
	public enum Permission
	{ 
		SYS_LOGIN,
		SYS_ZONE,
		SYS_ADMIN,
		ROLES_VIEW,
		ROLES_ADD,
		ROLES_EDIT,
		ROLES_REMOVE,
		USERS_VIEW,
		USERS_ADD,
		USERS_EDIT,
		USERS_REMOVE
	}

	public static class EnumParser{ 

		public static T Parse<T>(string value)
		{
			return (T)Enum.Parse(typeof(T), value, true);
		}
		
		public static Array GetAllValues<T>(){ 
			return Enum.GetValues(typeof(T));
		}

		

	};

	

}

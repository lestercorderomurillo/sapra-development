using System;
using System.Collections.Generic;

namespace sapra.Models
{
	public enum Permission
	{ 
		ACCESO_MAPA,
		ACCESO_ADMINISTRADOR,
		ROLES_VER,
		ROLES_CREAR,
		ROLES_EDITAR,
		ROLES_ELIMINAR,
		USUARIOS_VER,
		USUARIOS_CREAR,
		USUARIOS_EDITAR,
		USUARIOS_ELIMINAR
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

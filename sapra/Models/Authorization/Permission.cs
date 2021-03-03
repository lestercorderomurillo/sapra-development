using System;
using System.Collections.Generic;

namespace sapra.Models
{
	public enum Permission
	{ 
		ACCESO_MAPA_ARCGIS,
		ACCESO_ADMINISTRACION,
		ACCESO_PERFIL,
		ADMINISTRACION_GENERAL,
		ADMINISTRACION_ROLES,
		ADMINISTRACION_USUARIOS,
		ADMINISTRACION_CAPAS,
		ROLES_CREAR,
		ROLES_EDITAR,
		ROLES_ELIMINAR,
		USUARIOS_CREAR,
		USUARIOS_EDITAR,
		USUARIOS_ELIMINAR,
		CAPAS_CREAR,
		CAPAS_EDITAR,
		CAPAS_ELIMINAR
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

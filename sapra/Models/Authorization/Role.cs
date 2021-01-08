using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sapra.Models
{

	public class Role
	{
		[Required]
		public int RoleId { get; set; }
		
		public int ParentRoleId { get; set; }

		[Required]
		[StringLength(24, MinimumLength = 4)]
		public string Name { get; set; }

		[Required]
		[StringLength(64, MinimumLength = 4)]
		public string Description { get; set; }

		/**Local System**/
		[NotMapped]
		public List<string> Permissions { get; set; }

		/**EF Reference**/
		public IList<User> Users { get; set; }

		public Role() 
		{
			RoleId = 0;
		}

	}

	public class Role_Permission
	{
		public int RoleId { get; set; }

		public string PermissionName { get; set; }

		public Role_Permission(int RoleId, string PermissionName) 
		{
			this.RoleId = RoleId;
			this.PermissionName = PermissionName;
		}

		/**EF Reference**/
		public Role Role { get; set; }
	}

}
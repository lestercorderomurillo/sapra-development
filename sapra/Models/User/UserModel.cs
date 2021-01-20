using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sapra.Models
{
	public class User
	{
		[Required]
		public int UserId { get; set; }

		[Required]
		public int RoleId { get; set; }

		[Required]
		[StringLength(64)]
		public string Hash { get; set; }

		[Required]
		[StringLength(64)]
		public string Salt { get; set; }

		public DateTime LastLogin { get; set; }

		[StringLength(64)]
		public string RecoveryHash { get; set; }

		public DateTime RecoveryTimestamp { get; set; }

		public int LoginAttempts { get; set; }

		/**EF Reference**/
		public UserInfo UserInfo { get; private set; }
		
		public Role Role { get; set; }

		public void SetUserInfo(int UserId, UserInfo userInfo) 
		{
			userInfo.UserId = UserId;
			UserInfo = userInfo;
		}

		public User()
		{
			UserId = 0;
			UserInfo = new UserInfo();
		}

	}
}

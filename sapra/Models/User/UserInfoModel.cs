using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sapra.Models
{
	public enum GenderEnum
	{
		NULL, Male, Female
	}

	public enum IdentityEnum
	{
		NULL, National, Juridical, Foreign
	}

	public class UserInfo
	{
		[Required]
		public int UserId { get; set; }

		[Required]
		public string TypeIdentity { get; set; }

		[Required]
		[StringLength(18, MinimumLength = 8)]
		public string Identity { get; set; }

		[Required]
		[StringLength(24, MinimumLength = 2)]
		public string FirstName { get; set; }

		[Required]
		[StringLength(48, MinimumLength = 4)]
		public string LastName { get; set; }

		[Required]
		[StringLength(64, MinimumLength = 6)]
		public string Email { get; set; }

		[Required]
		public DateTime DateOfBirth { get; set; }

		[StringLength(48, MinimumLength = 6)]
		public string Address { get; set; }

		[Required]
		public GenderEnum? Gender { get; set; }

		[Required]
		public List<UserPhone> PhoneNumbers { get; set; }

		/**EF Reference**/
		public User User { get; set; }

		public UserInfo() 
		{
			PhoneNumbers = new List<UserPhone>();
			DateOfBirth = new DateTime(1985, 10, 25);
		}
	}
}

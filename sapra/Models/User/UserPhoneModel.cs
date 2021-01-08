using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sapra.Models
{
	public class UserPhone
	{
		[StringLength(35, MinimumLength = 3)]
		public string Number { get; set; }

		[Required]
		public int UserId { get; set; }

		[Required]
		public string Type { get; set; }

		/**EF Reference**/
		public UserInfo UserInfo { get; set; }

		public UserPhone() 
		{
			Number = String.Empty;
			Type = String.Empty;
		}
	}
}

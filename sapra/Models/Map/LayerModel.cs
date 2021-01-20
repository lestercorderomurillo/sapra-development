using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sapra.Models
{
	public class MapLayer
	{
		[Required]
		public int MapLayerId { get; set; }

		[Required]
		[StringLength(32, MinimumLength = 8)]
		public string Name { get; set; }

		[Required]
		[StringLength(64, MinimumLength = 8)]
		public string URL { get; set; }

	}
}

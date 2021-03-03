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

		/**EF Reference**/
		public List<MapLayerField> MapLayerFields { get; set; }

	}
	public class MapLayerField
	{
		public static int StringField = 0;
		public static int NumberField = 1;

		[Required]
		public int MapLayerId { get; set; }

		[Required]
		[StringLength(32, MinimumLength = 2)]
		public string Name { get; set; }

		[Required]
		public string Alias { get; set; }

		[Required]
		public int Type { get; set; }

		/**EF Reference**/
		public MapLayer MapLayer { get; set; }
	}
}

using sapra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sapra.ViewModels
{
	public class ZoneUsageActivityArea 
	{
		public string Name { get; set; }

		public string Code { get; set; }

		public string AreaDescription { get; set; }
	}

	public class ZoneUsageActivity
	{
		public string Name { get; set; }

		public string Code { get; set; }

		public Dictionary<string, ZoneUsageActivityArea> ZoneSizes { get; set; }

	}

	public class ZoneUsageCategory
	{
		public string Name { get; set; }

		public string Code { get; set; }

		public Dictionary<string, ZoneUsageActivity> ZoneUsageActivities { get; set; }

	}

	// Map Explorer ViewModel
	public class MapExplorerViewModel : BaseViewModel
	{
		public string BaseUrl { get; set; }

		public List<MapLayer> Layers { get; set; }

		public Dictionary<string, ZoneUsageCategory> ZoneUsageCategories { get; set; }

		public MapExplorerViewModel(List<MapLayer> layers)
		{
			Layers = layers;
		}
	}

	// Map PDF Report ViewModel
	public class MapReportViewModel : BaseViewModel 
	{
		public int ZoneLayerFID;
		public List<MapLayerField> Fields;
		public User User;
		public string Transaction;
		public string ActionStr; /*Vivienda*/
		public String DateStr; /*Al ser las @DateTime.Now.ToShortTimeString() del dia @DateTime.Now.ToLongDateString()*/
		public bool Accepted;
	}

}

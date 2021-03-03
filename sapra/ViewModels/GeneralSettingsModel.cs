namespace sapra.ViewModels
{
	public class GeneralSettingsModel : BaseViewModel
	{
		public string Title { get; set; }

		public string Subtitle { get; set; }

		public string ArcGISURL { get; set; }

		public string BaseLayer { get; set; }

		public GeneralSettingsModel()
		{
			Title = "Sistema de Administración";
			Subtitle = "Plan Regulador de Alajuela";
			BaseLayer = "Catastro";
			ArcGISURL = "http://www.google.com";
		}
	}
}

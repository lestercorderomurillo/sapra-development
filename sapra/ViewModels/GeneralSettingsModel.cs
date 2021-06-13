namespace sapra.ViewModels
{

	// JSON Model
	public class GeneralSettingsModel : BaseViewModel
	{
		public string Title { get; set; }

		public string Subtitle { get; set; }

		public string ArcGISURL { get; set; }

		public string BaseLayer { get; set; }

		public string SecondaryLayer { get; set; }

		public GeneralSettingsModel()
		{
			Title = "Sistema de Administraci�n";
			Subtitle = "Plan Regulador de Alajuela";
			BaseLayer = "Catastro";
			SecondaryLayer = "Zonificaci�n";
			ArcGISURL = "http://www.google.com";
		}
	}
}

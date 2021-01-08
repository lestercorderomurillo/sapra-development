namespace sapra.ViewModels
{
	public enum ResponseType
	{
		None, Success, Error, Info, Warning
	}

	public class ServerResponseViewModel
	{
		public string Text { get; set; }

		public ResponseType Type { private get; set; }

		public bool FullyScreen { get; set; }

		public ServerResponseViewModel(string _text, ResponseType _type) {
			FullyScreen = false;
			Text = _text;
			Type = _type;
		}

		public ServerResponseViewModel ExtendFull() {
			FullyScreen = true;
			return this;	
		}

		public ServerResponseViewModel() {
			Text = "";
			Type = ResponseType.None;
		}

		public string GetResponseTypeStyle() {
			switch (Type) {
				case ResponseType.Success:
					return ("alert alert-success");
				case ResponseType.Error:
					return ("alert alert-danger");
				case ResponseType.Info:
					return ("alert alert-info");
				case ResponseType.Warning:
					return ("alert alert-warning");
			}
			return ("disable");
		}
	}
}

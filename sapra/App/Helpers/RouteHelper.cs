using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sapra.Models;

namespace sapra.App.Helpers
{
	public enum HttpStatusCode
	{
		OK          = 200,
		Forbidden   = 403,
		NotFound    = 404,
		ServerError = 500,
		Unavailable = 503
	}

	
}

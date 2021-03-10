using System;
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using sapra.App;
using sapra.Models;
using sapra.ViewModels;
using sapra.App.Helpers;
using Newtonsoft.Json;

namespace sapra.Controllers
{

	public class ZoneController : SystemController
	{
		[HttpGet]
		public IActionResult Map()
		{
			if (SessionController.GetSessionVariable(HttpContext) > 0) 
			{
				return View(new MapLayerListViewModel(RequestAllMapLayers()){ BaseUrl = LoadSettings().ArcGISURL });
			}
			return new AuthorizationController().ForceRequestLogin();
		}
	}
}

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

	public class MapController : SystemController
	{
		[HttpGet]
		public IActionResult MapView()
		{
			if (SessionController.GetSessionVariable(HttpContext) > 0) 
			{
				return View(new MapLayerListViewModel(RequestAllMapLayers()){ BaseUrl = LoadSettings().ArcGISURL });
			}
			return new AuthorizationController().ForceRequestLogin();
		}

		/*[MiddlewareFilter(typeof(JsReportPipeline))]
		public IActionResult Invoice()
		{
			HttpContext.JsReportFeature().Recipe(Recipe.ChromePdf);
			return View();
		}*/

		[HttpPost]
		public IActionResult QueryReport(int FID)
		{
			using var db = new DatabaseContext();
			if (SessionController.GetSessionVariable(HttpContext) > 0)
			{
				var fields = db.MapLayerFieldRepository.ToList();
				var user = RequestUser(SessionController.GetSessionVariable(HttpContext, "UserId"), true);
				return PartialView(new MapShowcaseViewModel
				{
					QueryFID = FID,
					Fields = fields,
					User = user
				});
			}
			return new AuthorizationController().ForceRequestLogin();
		}
	}
}

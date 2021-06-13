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

using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace sapra.Controllers
{

	public class MapController : SystemController
	{

		public const string ZoneExcelMappingPath = "wwwroot/zone/datasheet.xlsx";

		[HttpGet]
		public IActionResult MapExplorer()
		{
			if (SessionController.GetSessionVariable(HttpContext) > 0) 
			{
				return View(new MapExplorerViewModel(RequestAllMapLayers()) { BaseUrl = LoadSettings().ArcGISURL, ZoneUsageCategories = GetZoneUsageCategories() });
			}
			return new AuthorizationController().ForceRequestLogin();
		}

		public Dictionary<string, ZoneUsageCategory> GetZoneUsageCategories() 
		{

			Dictionary<string, ZoneUsageCategory> zones;
			using (var package = new ExcelPackage(new System.IO.FileInfo(ZoneExcelMappingPath)))
			{
				ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
				zones = new Dictionary<string, ZoneUsageCategory>();

				for(int y = 2; y < 470; y++) 
				{
					
					var category = (string)worksheet.Cells[y, 1].Value;

					// If category doesn't exists, first register it
					if (!zones.ContainsKey(category)) 
					{
						zones.Add(category, new ZoneUsageCategory()
						{ 
							Name = category, 
							Code = (string)worksheet.Cells[y, 5].Value,
							ZoneUsageActivities = new Dictionary<string, ZoneUsageActivity>()
						}
						);
					}

					// now add the activity
					var activity = (string)worksheet.Cells[y, 2].Value;
					if (!zones[category].ZoneUsageActivities.ContainsKey(activity))
					{
						zones[category].ZoneUsageActivities.Add(activity, new ZoneUsageActivity()
						{
							Name = activity,
							Code = (string)worksheet.Cells[y, 6].Value,
							ZoneSizes = new Dictionary<string, ZoneUsageActivityArea>()
						}
						);
					}

					// now add the size variant
					var sizeVariant = (string)worksheet.Cells[y, 3].Value;

					ZoneUsageActivityArea data;
					if (!zones[category].ZoneUsageActivities[activity].ZoneSizes.TryGetValue(sizeVariant, out data))
					{
						var name = (string)worksheet.Cells[y, 3].Value;
						switch (sizeVariant) 
						{
							case "P": name = "Pequeño"; break;
							case "M": name = "Mediano"; break;
							case "G": name = "Grande"; break;
							case "XG": name = "Extra Grande"; break;
						}
						zones[category].ZoneUsageActivities[activity].ZoneSizes.Add(sizeVariant, new ZoneUsageActivityArea()
						{
							Name = name,
							Code = (string)worksheet.Cells[y, 3].Value,
							AreaDescription = (string)worksheet.Cells[y, 4].Value
						}
						);
					}
				}
			}
			return zones;
		}

		public string GetUseByKeyCode(string keycode)
		{
			using (var package = new ExcelPackage(new System.IO.FileInfo(ZoneExcelMappingPath)))
			{
				ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
				for (int y = 2; y < 480; y++)
				{
					if ((string)worksheet.Cells[7, y].Value == keycode) 
						return ((string)worksheet.Cells[2, y].Value);
				}
			}
			return "Uso no se ha encontrado en el sistema.";
		}

		public bool IsZoneAllowed(string zone, string keycode) 
		{
			using (var package = new ExcelPackage(new System.IO.FileInfo(ZoneExcelMappingPath)))
			{
				ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
				var zoneX = -1;
				var keycodeY = -1;

				for (int x = 8; x < 49; x++)
					if ((string)worksheet.Cells[x, 1].Value == zone) { zoneX = x; break; }

				for (int y = 2; y < 480; y++) 
					if ((string)worksheet.Cells[7, y].Value == keycode) { keycodeY = y; break; }

				if (zoneX >0 && keycodeY > 0)
					return ((string)worksheet.Cells[zoneX, keycodeY].Value == "1");
				
			}
			return false;
		}

		[HttpPost]
		public IActionResult MapReport(string zonecode, string keycode, string zoneFID)
		{
			using var db = new DatabaseContext();
			using var package = new ExcelPackage(new System.IO.FileInfo(ZoneExcelMappingPath));

			ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

			if (SessionController.GetSessionVariable(HttpContext) > 0)
			{
				var fields = db.MapLayerFieldRepository.ToList();
				var user = RequestUser(SessionController.GetSessionVariable(HttpContext, "UserId"), true);
				return View(new MapReportViewModel
				{
					ZoneLayerFID = int.Parse(zoneFID),
					Fields = fields,
					User = user,
					Accepted = IsZoneAllowed(zonecode, keycode),
					DateStr = "diaactualyya;",
					Transaction = "usar contador inteligente",
					ActionStr = GetUseByKeyCode(keycode)
				});
			}
			return new AuthorizationController().ForceRequestLogin();
		}
	}
}

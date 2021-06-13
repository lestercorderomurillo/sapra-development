using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jsreport.AspNetCore;
using jsreport.Binary;
using jsreport.Local;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using sapra.App;

#pragma warning disable CS0618
namespace sapra
{

	public class Startup
	{

		public static Startup RootAccess;

		public Startup(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
		{
			Configuration = configuration;
			Environment = environment;
		}

		public IConfiguration Configuration { get; }

		public Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment { get; }

		public string ConnectionString { private set; get; }

		public void ConfigureServices(IServiceCollection services)
		{
			ConnectionString = Configuration.GetConnectionString("SAPRADEV");
			RootAccess = this;

			services.AddRazorPages().AddRazorRuntimeCompilation();
			services.AddControllersWithViews();
			services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(ConnectionString));
			services.AddDistributedMemoryCache();

			services.AddCors(cors => cors.AddPolicy("Policy", builder =>
			{
				builder.AllowAnyOrigin()
					   .AllowAnyMethod()
					   .AllowAnyHeader();
			}));

			services.AddSession(options => {
				options.IdleTimeout = TimeSpan.FromMinutes(8);
				options.Cookie.IsEssential = true;
			});

			services.Configure<CookiePolicyOptions>(options =>
			{
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);
			services.AddJsReport(new LocalReporting()
				.UseBinary(JsReportBinary.GetBinary())
				.AsUtility()
				.Create());
			}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Shared/Error");
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseRouting();
			app.UseAuthorization();
			app.UseSession();

			app.UseEndpoints(endpoints =>
			{

				endpoints.MapControllerRoute(
					name: "Map",
					pattern: "Map",
					defaults: new { controller = "Map", action = "MapExplorer" });

				endpoints.MapControllerRoute(
					name: "Logout",
					pattern: "Logout",
					defaults: new { controller = "Authorization", action = "Logout" });

				endpoints.MapControllerRoute(
					name: "Login",
					pattern: "Login",
					defaults: new { controller = "Authorization", action = "Login" });

				endpoints.MapControllerRoute(
					name: "Administrator",
					pattern: "Administrator",
					defaults: new { controller = "Administrator", action = "Panel" });

				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Map}/{action=MapExplorer}/{id?}");

			});
		}
	}
}

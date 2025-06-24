using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RealEstate.Core.Domain.IdentityEntities;
using RealEstate.Core.Domain.RepositoryContracts;
using RealEstate.Core.Helpers;
using RealEstate.Core.ServiceContracts;
using RealEstate.Core.Services;
using RealEstate.Infrastructure.DbContext;
using RealEstate.Infrastructure.Repositories;
using Serilog;
using System.Text;
using RealEstate.WebAPI.StartupExtensions;

namespace RealEstate.WebAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Host.UseSerilog((HostBuilderContext contex, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
			{
				loggerConfiguration.ReadFrom.Configuration(contex.Configuration).ReadFrom.Services(services);
			});

			builder.Services.ConfigureServices(builder.Configuration);

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			app.UseHsts();
			app.UseHttpsRedirection();
			app.UseSerilogRequestLogging();
			app.UseHttpLogging();
			app.UseStaticFiles();
			app.UseHttpsRedirection();
			app.UseAuthentication();
			app.UseAuthorization();
			app.MapControllers();

			app.Run();
		}
	}
}

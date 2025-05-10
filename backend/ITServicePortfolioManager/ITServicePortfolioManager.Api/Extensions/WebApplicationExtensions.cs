using ITServicePortfolioManager.Api.Endpoints;
using ITServicePortfolioManager.BLL;
using ITServicePortfolioManager.DAL;

namespace ITServicePortfolioManager.Api.Extensions
{
	public static class WebApplicationExtensions
	{
		public static void AddApplicationServices(this WebApplicationBuilder builder) 
		{
			var services = builder.Services;
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();
			services.AddBLLLayer();
			services.AddInfrastructureLayer();
			services.AddDatabase(builder.Configuration);
			services.AddCors(options =>
			{
				options.AddPolicy("AllowAngular",
					policy =>
					{
						policy.WithOrigins("http://localhost:4200")
							.AllowAnyMethod()
							.AllowAnyHeader()
							.AllowCredentials();
					});
			});
		}

		public static void UseApplicationMiddlewares(this WebApplication app)
		{
			if (app.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseRouting();
			app.MapSolverEndpoint();
			app.UseCors("AllowAngular");
		}
	}
}

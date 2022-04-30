using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using Minibank.Core;
using Minibank.Data;
using Minibank.Web.HostedServices;
using Minibank.Web.Middlewares;

namespace Minibank.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(j =>
            {
                j.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v4", new Microsoft.OpenApi.Models.OpenApiInfo
                    {Title = "MiniBank.Web", Version = "v4"});
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                options.AddSecurityDefinition("oauth2",
                    new OpenApiSecurityScheme()
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows()
                        {
                            ClientCredentials = new OpenApiOAuthFlow()
                            {
                                TokenUrl = new Uri("https://demo.duendesoftware.com/connect/token"),
                                Scopes = new Dictionary<string, string>()
                            }
                        }
                    });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = SecuritySchemeType.OAuth2.GetDisplayName()
                            }
                        },
                        new List<string>()
                    }
                });
            });

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Audience = "api";
                    options.Authority = "https://demo.duendesoftware.com";
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateLifetime = false,
                        ValidateAudience = false
                    };
                });

            services.
                AddData(Configuration).
                AddCore();

            services.AddHostedService<MigrationHostedService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v4/swagger.json", "MiniBank.Web v4");
                });
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseMiddleware<CustomAuthenticationMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

    }
}

using AutoMapper;
using LMS_Project.Data;
using LMS_Project.Data.Abstractions;
using LMS_Project.Data.Profiles;
using LMS_Project.Data.Repositories;
using LMS_Project.Services.Abstractions;
using LMS_Project.Services.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Midenas_Test
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            SetupSwagger(services, currentAssembly);
            SetupDatabase(services);
            SetupAutomapper(services);

            services.AddTransient<IStudentRepository, StudentRepository>();
            services.AddTransient<IProfessorRepository, ProfessorRepository>();
            services.AddTransient<ICourseRepository, CourseRepository>();
            services.AddTransient<IStudentCourseRepository, StudentCourseRepository>();
            services.AddTransient<ICityRepository, CityRepository>();
            services.AddTransient<IStreetRepository, StreetRepository>();

            services.AddTransient<IStudentService, StudentService>();
            services.AddTransient<IProfessorService, ProfessorService>();
            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<ICityService, CityService>();
            services.AddTransient<IStreetService, StreetService>();

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Onboarding.API v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void SetupSwagger(IServiceCollection services, Assembly assembly)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Onboarding API",
                    Description = "Onboarding Smart Solutions Web API",
                    TermsOfService = new Uri("https://midenas.rs/"),
                    Contact = new OpenApiContact
                    {
                        Name = "Onboarding",
                        Email = "davor.pajic@midenas.rs",
                        Url = new Uri("https://midenas.rs/"),
                    },
                });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);

                //c.OperationFilter<SwaggerFileOperationFilter>();

                c.CustomSchemaIds(x => x.FullName);

                c.AddSecurityDefinition("JWT token", new OpenApiSecurityScheme
                {
                    Description = "Authotization header with JWT token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "JWT token"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

                var filePath = Path.Combine(AppContext.BaseDirectory, $"{assembly.GetName().Name}.xml");
                if (File.Exists(filePath))
                {
                    c.IncludeXmlComments(filePath);
                }
            }).AddSwaggerGenNewtonsoftSupport();
        }

        private void SetupAutomapper(IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new StudentProfile());
                mc.AddProfile(new ProfessorProfile());
                mc.AddProfile(new CourseProfile());
                mc.AddProfile(new CityProfile());
                mc.AddProfile(new StreetProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        private void SetupDatabase(IServiceCollection services)
        {
            var assembly = typeof(ApplicationDbContext).Assembly.GetName().Name;
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"),
                    opt =>
                    {
                        opt.EnableRetryOnFailure(3);
                        opt.CommandTimeout(120);
                        opt.MigrationsAssembly(assembly);
                    }),
                ServiceLifetime.Scoped,
                ServiceLifetime.Singleton);
        }
    }
}
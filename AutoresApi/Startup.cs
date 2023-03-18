using AutoresApi.Controllers;
using AutoresApi.Filters;
using AutoresApi.Middlewares;
using AutoresApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;

namespace AutoresApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container.

            services.AddControllers( options =>
            {
                options.Filters.Add(typeof(ExceptionFilter));
            }
            ).AddJsonOptions(x =>
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));

            services.AddTransient<IService, ServiceA>();
            //-> types services examples
            services.AddTransient<ServiceTransient>();
            services.AddScoped<ServiceScoped>();
            services.AddSingleton<ServiceSingleton>();

            //Filter
            services.AddTransient<myFilter>();

            //To execute function retrospectively
            services.AddHostedService<WriteInFile>();

            //For use Cache
            services.AddResponseCaching();

            //For security
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration["keyJWT"])),
                    ClockSkew = TimeSpan.Zero
                });

            //Types servicios:***********************************************
            //Simply increase the lifecycle of the instantiated Class.
            //-AddTransient Always create instance, useful for creating functionality and that's it,example return data and then it's all over (transient service).
            //-AddScoped You will have the same instance in the same HTTP context, between different http requests you will have different instances (clients use different instances) example of this DBContext service all http requests will receive the same instance of the class.
            //-AddSingleton Creates a single instance for all HTTP requests, you will always have the same instance. For example in-memory data always has the same instance
            //*********************************************************************

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                //JWT Jason web token
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                     {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                     }
                });
            });

            services.AddAutoMapper(typeof(Startup));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //Add Users Admins
            services.AddAuthorization(option =>
            {
                option.AddPolicy("IsAdmin", policy => policy.RequireClaim("IsAdmin"));
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    //Cors for allow request from diferents origins
                    builder.WithOrigins("https://www.apirequest.io").AllowAnyMethod().AllowAnyMethod();
                });
            });

            services.AddDataProtection();
            services.AddTransient<HashService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            //All Middlewares ->
            app.UseMiddleware<LogResponseHTTPMiddleware>();


            //Intercept Middleware with Map
            app.Map("/route1", app =>
                    {
                        app.Run(async context =>
                        {
                            await context.Response.WriteAsync("intercept the pipe");
                        });

                    });
           

            // Configure the HTTP request pipeline.
            //Swagger(donde ejecutamos la app) solo funciona en desarrollo
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            //Cors for allow request from diferents origins
            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
             {
                 endpoints.MapControllers();
             });

        }
    }
}

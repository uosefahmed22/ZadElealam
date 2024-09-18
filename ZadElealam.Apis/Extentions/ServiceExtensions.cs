using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using System.Text;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ZadElealam.Repository.Services;
using ZadElealam.Core.Models.Auth;
using ZadElealam.Core.Errors;
using Microsoft.AspNetCore.Mvc;
using ZadElealam.Core.IServices;
using ZadElealam.Apis.Helpers;
using ZadElealam.Core.IServices.Auth;
using ZadElealam.Core.IRepository;
using ZadElealam.Repository.Repository;
using Google.Apis.YouTube.v3.Data;
using ZadElealam.Repository.Data;

namespace ZadElealam.Apis.Extentions
{
    public static class ServiceExtensions
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtConfig>(configuration.GetSection("JwtConfig"));

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            var key = Encoding.ASCII.GetBytes(configuration["JwtConfig:Secret"]);

            var tokenvalidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = false,
                ClockSkew = TimeSpan.Zero
            };

            services.AddSingleton(tokenvalidationParameters);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = tokenvalidationParameters;
            });

            services.AddIdentity<AppUser, IdentityRole>(options =>
                options.SignIn.RequireConfirmedAccount = true)
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<AppDbContext>();

            //services.AddControllers().AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            //    options.JsonSerializerOptions.WriteIndented = true;
            //});

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
            });


            services.AddEndpointsApiExplorer();

            // Configure Swagger using the extension method
            services.AddSwaggerDocumentation();
            // Add Memory Cache
            services.AddMemoryCache();
            //configure Auto Mapper
            services.AddAutoMapper(typeof(MappingProfile));
            //Cloudinary Configuration
            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySetting"));

            services.AddSingleton(cloudinary =>
            {
                var config = configuration.GetSection("CloudinarySetting").Get<CloudinarySettings>();
                var account = new CloudinaryDotNet.Account(config.CloudName, config.ApiKey, config.ApiSecret);
                return new CloudinaryDotNet.Cloudinary(account);
            });

            // Add custom services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<IOtpService, OtpService>();
            services.AddScoped<IPlaylistRepository, PlaylistRepository>();
            services.AddScoped<IImageService, ImageService>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IExamRepository, ExamRepository>();
            services.AddScoped<ICertificateService, CertificateService>();
            services.AddScoped<IFeedbackAndFavorities, FeedbackAndFavorities>();
            // Configure CORS using the extension method
            services.ConfigureCors();

            services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));

            // Add custom error handling for model validation
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .Select(e => new
                        {
                            Field = e.Key,
                            ErrorMessages = e.Value.Errors.Select(x => x.ErrorMessage).ToArray()
                        }).ToArray();

                    var result = new
                    {
                        Message = "Validation failed",
                        Errors = errors
                    };

                    return new BadRequestObjectResult(result);
                };
            });
        }
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("Open", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
        }
        public static void AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
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
                    Array.Empty<string>()
                }
            });
            });
        }
    }

}

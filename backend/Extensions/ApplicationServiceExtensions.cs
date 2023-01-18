using CloudinaryDotNet;
using DatingApp.Data;
using DatingApp.Helpers;
using DatingApp.Interface;
using DatingApp.Services;
using DatingApp.SignalR;
using dotenv.net;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddSingleton<PresenceTracker>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILikesRepository, LikesRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<LogUserActivity>();
            DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
            Cloudinary cloudinary = new(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
            services.AddScoped<IPhotoService, PhotoService>();
            cloudinary.Api.Secure = true;
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            services.AddSignalR();

            return services;
        }
    }
}

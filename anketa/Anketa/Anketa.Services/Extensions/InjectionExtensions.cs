using Anketa.DataAccess;
using Anketa.DataAccess.Implementations;
using Anketa.DataAccess.Interfaces;
using Anketa.Services.Implementations;
using Anketa.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Anketa.Services.Extensions
{
    public static class InjectionExtensions
    {
        public static void InjectDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(option => option.UseSqlServer(connectionString));
        }
        public static void InjectRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IAnswerRepository, AnswerRepository>();
        }
        public static void InjectServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IAnswerService, AnswerService>();
        }
    }
}

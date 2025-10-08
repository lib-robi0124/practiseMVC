using GlasAnketa.DataAccess.DataContext;
using GlasAnketa.DataAccess.Implementations;
using GlasAnketa.DataAccess.Interfaces;
using GlasAnketa.Services.Implementations;
using GlasAnketa.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GlasAnketa.Services.Extensions
{
    public static class InjectionExtensions
    {
        public static void InjectDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(option => option.UseSqlServer(connectionString));
        }

        public static void InjectRepositories(this IServiceCollection services)
        {
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IQuestionFormRepository, QuestionFormRepository>();
            services.AddScoped<IAnswerRepository, AnswerRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
        public static void InjectServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IQuestionFormService, QuestionFormService>();
            services.AddScoped<IAnswerService, AnswerService>();

        }

        public static void InjectAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        }
    }
}

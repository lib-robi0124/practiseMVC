using GlasAnketa.DataAccess.DataContext;
using GlasAnketa.DataAccess.Implementations;
using GlasAnketa.DataAccess.Interfaces;
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
    }
}

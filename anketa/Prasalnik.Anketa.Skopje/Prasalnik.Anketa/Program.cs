using Microsoft.EntityFrameworkCore;
using Prasalnik.DataAccess.DataContext;
using Prasalnik.DataAccess.Implementations;
using Prasalnik.DataAccess.Interaces;
using Prasalnik.Mappers.AutoMapperProfiles;
using Prasalnik.Services.Implementations;
using Prasalnik.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);


string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
// Add services to the container.
builder.Services.AddControllersWithViews();

// ... add others
IServiceCollection serviceCollection = builder.Services.AddAutoMapper(typeof(UserMappingProfile).Assembly);
builder.Services.AddSession();

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IQuestionnaireRepository, QuestionnaireRepository>();
builder.Services.AddScoped<IQuestionItemRepository, QuestionItemRepository>();
builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IQuestionnaireService, QuestionnaireService>();
builder.Services.AddScoped<IQuestionItemService, QuestionItemService>();
builder.Services.AddScoped<IAnswerService, AnswerService>();
builder.Services.AddScoped<IStatusService, StatusService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();

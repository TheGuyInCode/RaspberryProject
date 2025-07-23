using BLL.MapperProfiles;
using BLL.Services.AbstractServices;
using BLL.Services.ConcreteServices;
using DAL.Data;
using DAL.Repositories.AbstractRepositories;
using DAL.Repositories.ConcreteRepositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
// Add services to the container.


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddAutoMapper(typeof(QrRecordProfile).Assembly);
builder.Services.AddAutoMapper(typeof(SensorDataProfile));
builder.Services.AddAutoMapper(typeof(CameraCaptureProfile));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<IQrRecordService, QrRecordService>();
builder.Services.AddScoped<ICameraCaptureService, CameraCaptureService>();
builder.Services.AddScoped<ISensorDataService, SensorDataService>();


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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

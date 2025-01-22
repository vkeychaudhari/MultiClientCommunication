using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Cors.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

/// Enable session support
//builder.Services.AddDistributedMemoryCache();  // Use in-memory cache for sessions
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromSeconds(30); // Optional: set session timeout
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});


//builder.Services.AddSignalR();

var app = builder.Build();


// Enable CORS middleware before routing
app.UseCors("AllowAll");


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
app.UseCors("TrustedOriginPolicy");

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapHub<MyHub>("/myHub");
//});
//app.MapSignalR("/myHub", new HubConfiguration());

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapHub<ChatHub>("/chatHub");
//});

app.UseAuthorization();

// Enable session middleware
//app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

builder.Services.AddScoped<WeConnect.Data.BlockingsDB>();
builder.Services.AddScoped<WeConnect.Data.CommentsDB>();
builder.Services.AddScoped<WeConnect.Data.ConversationDB>();
builder.Services.AddScoped(typeof(WeConnect.Data.CrudDB<>));
builder.Services.AddScoped<WeConnect.Data.FriendsDB>();
builder.Services.AddScoped<WeConnect.Data.LogInDB>();
builder.Services.AddScoped<WeConnect.Data.MessagesDB>();
builder.Services.AddScoped<WeConnect.Data.NotificationsDB>();
builder.Services.AddScoped<WeConnect.Data.PostsDB>();
builder.Services.AddScoped<WeConnect.Data.UsersDB>();
builder.Services.AddScoped<WeConnect.Models.LogInService>();
builder.Services.AddScoped<WeConnect.Models.UserService>();
builder.Services.AddScoped<WeConnect.Models.FriendService>();
builder.Services.AddScoped<WeConnect.Models.ConversationService>();
builder.Services.AddScoped<WeConnect.Data.PhotoDB>();
builder.Services.AddScoped<WeConnect.Models.MessgageService>();
builder.Services.AddScoped<WeConnect.Models.NotificationService>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseExceptionHandler("/Home/Error");

// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

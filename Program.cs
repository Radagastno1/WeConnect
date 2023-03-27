using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.IncludeErrorDetails = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(
                    builder.Configuration.GetSection("JwtSettings:mySecretKey").Value
                )
            ),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration.GetSection("JwtSettings:myIssuer").Value,
            ValidateAudience = true,
            ValidAudience = builder.Configuration.GetSection("JwtSettings:myAudience").Value,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

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
builder.Services.AddScoped<WeConnect.Models.PhotoService>();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseExceptionHandler("/Home/Error");

// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();
app.UseSession();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

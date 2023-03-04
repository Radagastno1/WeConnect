var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
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

// // dessa under ska inte vara delegat, var bara som övning i början
// userUI.OnDialogue += conversationUI.ShowDialogue;
// userUI.OnMakeMessage += messageUI.MakeMessage;
// userUI.OnMakeConversation += conversationUI.MakeNewConversation;

// userManager.OnDelete += usersDB.UpdateToDeleted;
// logInUI.OnLoggedIn += friendManager.Update;
// logInUI.OnLoggedIn += friendManager.LoadFriends;
// userUI.LoadFriends += friendManager.Update;
// userUI.LoadFriends += friendManager.LoadFriends;
// builder.Services.AddScoped<WeConnect.Models.BlockingService>();
// OnBlockUser += friendsDB.Delete;
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

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

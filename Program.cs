var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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

/*
Student Name: Roman Stephenson
Student ID: 620157803

Instructions:

Use visual studio code, extract the folder to a location of your choosing and open the folder in visual studio code.

You may need a few extensions like:
C#
C# Dev Kit
.NET extensions pack
,BET Install tool 

Once you open the folder you extracted the project to in vs code, you should open the program.cs file and the run or play button will be in the top right hand corner.

Click it and the project will load and launch the website in your default browser for you to test the solution.
*/
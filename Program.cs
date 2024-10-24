using FluentValidation;
using FluentValidation.AspNetCore;
using ClassLibrary;
using ClassLibrary.Validators;
using ClassLibrary.Repositories.interfaces;
using ClassLibrary.Repositories;
using ClassLibrary.Services.Interfaces;
using ClassLibrary.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ProjDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddScoped<IMilitaryUnitRepository, MilitaryUnitRepository>();
builder.Services.AddScoped<IContactPersonRepository, ContactPersonRepository>();
builder.Services.AddScoped<IVolunteerOrganizationRepository, VolunteerOrganizationRepository>();
builder.Services.AddScoped<IVolunteerRepository, VolunteerRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<IMilitaryUnitService, MilitaryUnitService>();
builder.Services.AddScoped<IContactPersonService, ContactPersonService>();
builder.Services.AddScoped<IVolunteerOrganizationService, VolunteerOrganizationService>();
builder.Services.AddScoped<IVolunteerService, VolunteerService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews();
builder.Services.AddFluentValidationAutoValidation()
                 .AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<VolunteerValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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

using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using TourBooking.Core.Domain;
using TourBooking.Infrastructure.Context;
using TourBooking.Web.CompositionRoot;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataProtection().PersistKeysToDbContext<TourBookingDbContext>();
builder.Services.AddDbContext<TourBookingDbContext>(OptionsHelper.ContextOptions(builder));

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(OptionsHelper.IdentityOptions()).AddEntityFrameworkStores<TourBookingDbContext>();
builder.Services.AddAuthentication(OptionsHelper.AuthenticationOptions()).AddJwtBearer(OptionsHelper.JwtBearerOptions(builder));

builder.Services.ConfigureTourBookingServices();

builder.Services.AddMvc(options => options.SuppressAsyncSuffixInActionNames = false);
builder.Services.AddSwaggerGen(OptionsHelper.SwaggerGenOptions());

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options => options.DefaultModelsExpandDepth(-1));
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

await app.RunAsync();
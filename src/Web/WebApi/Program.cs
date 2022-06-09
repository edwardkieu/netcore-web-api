// Initialize Logger
using FluentValidation.AspNetCore;

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(GetConfiguration())
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Log.Logger);
builder.WebHost.UseUrls("https://localhost:8000");

// Register container services
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsAllowAll",
        policy =>
        {
            policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        });

    options.AddPolicy(name: GlobalConstants.AllowSpecificCors,
        policy =>
        {
            policy
            .WithHeaders(builder.Configuration.GetSection("AppSettings:CorsAllowSpecific:WithHeaders").Get<string[]>())
            .WithMethods(builder.Configuration.GetSection("AppSettings:CorsAllowSpecific:WithMethods").Get<string[]>())
            .SetPreflightMaxAge(new TimeSpan(builder.Configuration.GetSection("AppSettings:CorsAllowSpecific:MaxAgeInTimeSpan").Get<long>()))
            .WithOrigins(builder.Configuration.GetSection("AppSettings:CorsAllowSpecific:WithOrigins").Get<string[]>())
            .AllowCredentials();
        });
});
builder.Services.AddApiValidationBehavior();
builder.Services.AddApplicationLayer();
builder.Services.AddIdentityInfrastructure(builder.Configuration);
builder.Services.AddPersistenceInfrastructure(builder.Configuration);
builder.Services.AddSharedInfrastructure(builder.Configuration);
builder.Services.AddSwaggerExtension();
// using fluent validation
//builder.Services.AddControllers()
//                .AddFluentValidation();

// ussing default validation
builder.Services.AddControllers(options => options.Filters.Add(typeof(ValidateModelFilter)));
builder.Services.AddControllersWithViews()
    //.AddControllersWithViews(options => options.Filters.Add(typeof(ValidateModelFilter)))
    //.AddJsonOptions(options =>
    //{
    //    options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
    //})
    .AddNewtonsoftJson(opt =>
    {
        opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        opt.SerializerSettings.NullValueHandling = NullValueHandling.Include;
        opt.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
    });
builder.Services.AddApiVersioningExtension();
builder.Services.AddHealthChecks()
    .AddCheck<SqlServerHealthCheck>("Sql", tags: new[] { "example" })
    //.AddCheck<ReadisHealthcheck>("Redis")
    .AddCheck<HttpHealthCheck>("Http", tags: new[] { "example" });
builder.Services.AddTransient<IAuthenticatedUserService, AuthenticatedUserService>();
builder.Services.AddTransient<IViewRenderService, ViewRenderService>();
// Register request pineline
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetCoreApi");
    });
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseErrorHandlingMiddleware();
app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.UseRouting();
app.UseCors(app.Configuration.GetValue<bool>("AppSettings:CorsAllowSpecific:AllowAll")? "CorsAllowAll" : GlobalConstants.AllowSpecificCors);
app.UseAuthentication();
app.UseAuthorization();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHealthChecks("/healthcheck", new HealthCheckOptions
    {
        AllowCachingResponses = false,
        Predicate = (check) => check.Tags.Contains("example") || check.Tags.Contains("abc"),
        ResultStatusCodes =
                    {
                        [HealthStatus.Healthy] = StatusCodes.Status200OK,
                        [HealthStatus.Degraded] = StatusCodes.Status200OK,
                        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                    },
        ResponseWriter = WriteResponse
    });
});

app.UseHealthChecks("/healthcheck");

// Seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    //var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        if (!(await userManager.Users.AnyAsync()) && !(await roleManager.Roles.AnyAsync()))
        {
            await Infrastructure.Identity.Seeds.DefaultRoles.SeedAsync(userManager, roleManager);
            await Infrastructure.Identity.Seeds.DefaultSuperAdmin.SeedAsync(userManager, roleManager);
            await Infrastructure.Identity.Seeds.DefaultBasicUser.SeedAsync(userManager, roleManager);
            Log.Information("Finished Seeding Default Data");
            Log.Information("Application Starting");
        }
    }
    catch (Exception ex)
    {
        Log.Warning(ex, "An error occurred seeding the DB");
    }
    finally
    {
        Log.CloseAndFlush();
    }
}

await app.RunAsync();

static Task WriteResponse(HttpContext context, HealthReport result)
{
    context.Response.ContentType = "application/json";

    var json = new JObject(
        new JProperty("status", result.Status.ToString()),
        new JProperty("results", new JObject(result.Entries.Select(pair =>
            new JProperty(pair.Key, new JObject(
                new JProperty("status", pair.Value.Status.ToString()),
                new JProperty("description", pair.Value.Description),
                new JProperty("data", new JObject(pair.Value.Data.Select(
                    p => new JProperty(p.Key, p.Value))))))))));

    return context.Response.WriteAsync(json.ToString((Newtonsoft.Json.Formatting)Formatting.Indented));
}

static IConfiguration GetConfiguration()
{
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile($"appsettings.json", true, true)
        .AddJsonFile($"appsettings.{environment}.json", optional: false, reloadOnChange: true)
        .Build();

    return config;
}
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);
var azureConfig = builder.Configuration.GetSection("AzureAD");

if (azureConfig == null)
{
    var error = "Azure Configuration Not Found";
}
//ILogger logger;

// Add Logging
builder.Host.ConfigureLogging(logging => {
    logging.ClearProviders();
    logging.AddConsole();
});

var something = new ServiceCollection();
//logger = context.HttpContext.RequestServices.

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(azureConfig!);

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // To preserve the default behavior, capture the original delegate to call later.
        var builtInFactory = options.InvalidModelStateResponseFactory;

        options.InvalidModelStateResponseFactory = context =>
        {
            var _logger = context.HttpContext.RequestServices
                                .GetRequiredService<ILogger<Program>>();

            // Perform logging here.
            _logger.LogError($"Errors found: {context.ModelState.ErrorCount}");

            // Invoke the default behavior, which produces a ValidationProblemDetails
            // response.
            // To produce a custom response, return a different implementation of 
            // IActionResult instead.
            return builtInFactory(context);
        };
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var app = builder.Build();
app.MapHealthChecks("/health");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    IdentityModelEventSource.ShowPII = app.Environment.IsDevelopment();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    // app.UseHsts();
}

app.UseHttpsRedirection();

//app.UseRouting();

app.UseAuthentication();
//app.Use(async (context, next) =>
//{
//    var _logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

//    // Perform logging here.
//    var keys = context.Request.Headers.Keys.ToList();
//    _logger.LogInformation($"REQUEST HEADER KEYS: {string.Join(", ", keys)}");
//    _logger.LogInformation($"Authorization: {string.Join(", ", context.Request.Headers["Authorization"].ToList())}");

//    if (!context.User.Identity?.IsAuthenticated ?? false)
//    {
//        context.Response.StatusCode = 401;
//        await context.Response.WriteAsync("Not Authenticated");
//    }
//    else await next();
//});

app.UseAuthorization();

app.MapControllers();

app.Run();

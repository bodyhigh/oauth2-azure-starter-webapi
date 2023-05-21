using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;

var AllowSpecificArigins = "_allowSpecificArigins";
var builder = WebApplication.CreateBuilder(args);

// Add Logging
var logger = LoggerFactory.Create(config =>
{
    //config.ClearProviders();
    config.AddConsole();
    config.AddConfiguration(builder.Configuration.GetSection("Logging"));
}).CreateLogger("Program");

builder.Services.AddCors(options => 
{
    options.AddPolicy(name: AllowSpecificArigins, policy => 
    {
        policy.WithOrigins("https://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


var azureADConfig = builder.Configuration.GetSection("AzureAD");
if (azureADConfig == null)
{
    logger.LogError("Azure Configuration Not Found");
}

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(azureADConfig!);

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // To preserve the default behavior, capture the original delegate to call later.
        var builtInFactory = options.InvalidModelStateResponseFactory;

        options.InvalidModelStateResponseFactory = context =>
        {
            // Perform logging here.
            logger.LogError($"Errors found: {context.ModelState.ErrorCount}");

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
app.UseRouting();
app.UseCors(AllowSpecificArigins);
app.UseAuthentication();

//app.Use(async (context, next) =>
//{
//    // Debugging request header
//    var keys = context.Request.Headers.Keys.ToList();
//    logger.LogInformation($"REQUEST HEADER KEYS: {string.Join(", ", keys)}");
//    logger.LogInformation($"Authorization: {string.Join(", ", context.Request.Headers["Authorization"].ToList())}");

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

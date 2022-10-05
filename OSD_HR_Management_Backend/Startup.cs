using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using OSD_HR_Management_Backend.ExtensionMethods;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OSD_HR_Management_Backend.Services.Abstractions;
using OSD_HR_Management_Backend.Services.Implementations;
using Amazon.S3;
using OSD_HR_Management_Backend.Middlewares;
using Microsoft.AspNetCore.Authorization;
using OSD_HR_Management_Backend.Constants;
using Microsoft.AspNetCore.Http.Features;

namespace OSD_HR_Management_Backend;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container
    public void ConfigureServices(IServiceCollection services)
    {
        var awsOptions = Configuration.GetAWSOptions();

        services.AddDefaultAWSOptions(awsOptions);
        services.AddAWSService<IAmazonDynamoDB>();

        services.AddScoped<IDynamoDBContext, DynamoDBContext>();

        services.AddCors();

        services.AddControllers();

        services.Configure<FormOptions>(options =>
        {
            // Set the limit file to 5 MB
            options.MultipartBodyLengthLimit = 5000000;
        });

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.AdminOnly,
                policy => policy.Requirements.Add(new AdminPermission()));
        });

        services.AddTransient<IAuthorizationHandler, AdminPermissionHandler>();
        /*services.AddSingleton<IAuthorizationHandler, AdminPermissionHandler>();        */

        services.AddAWSService<IAmazonS3>();

        services.AddScoped<IStorageService, StorageService>();

        // Custom Configurations HERE
        services.ConfigureRepositories();
        services.ConfigureAutoMappers();
        services.ConfigureSwagger();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();            
        }

        app.UseSwagger();

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "OSD API V1");
        });

        app.UseHttpsRedirection();

        app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true)
            .AllowCredentials());

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Welcome to running ASP.NET Core on AWS Lambda");
            });
        });
    }
}
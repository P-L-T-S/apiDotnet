using System.Text;
using Asp.Versioning.ApiExplorer;
using FullAPIDotnet;
using FullAPIDotnet.Application.Mapping;
using FullAPIDotnet.Application.Swagger;
using FullAPIDotnet.Domain.Model;
using FullAPIDotnet.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// adiciona os controllers / as rotas
builder.Services.AddControllers();

// mapeia automaticamente as entidades para DTO
builder.Services.AddAutoMapper(typeof(DomainToDtoMapping));

// configura a utilizacao do CORS
builder.Services.AddCors(options =>
{
    // adiciona as politicas de cors
    options.AddPolicy("MyPolicy", policy =>
    {
        // habilita o dominio consumir a api
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// adiciona e configura o versionamento da api
builder.Services.AddApiVersioning()
    .AddMvc()
    .AddApiExplorer(setup =>
        {
            setup.GroupNameFormat = "'v'VVV";
            setup.SubstituteApiVersionInUrl = true;
        }
    );

builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();

// relaciona a Interface com a Classe para aplicar injeção de dependencia;
builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swaggerOptions =>
{
    // configura o swagger para gerenciar as versoes da api
    swaggerOptions.OperationFilter<SwaggerDefaultValues>();

    // configura o swagger para conseguir receber o token
    swaggerOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    // sei lá, mas é assim - deve precisar para receber o token
    swaggerOptions.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// adiciona a autenticação de rota
var key = Encoding.ASCII.GetBytes(JwtKey.Secret);
builder.Services.AddAuthentication(authOptions =>
{
    // define o jwt como autenticador padrao
    authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt =>
{
    jwt.RequireHttpsMetadata = false;
    jwt.SaveToken = true;
    // define os parametros do token
    jwt.TokenValidationParameters = new TokenValidationParameters
    {
        // valida se a chave do token está correta
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

// define qual politica de CORS vai ser usada
app.UseCors("MyPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // redireciona os erros para a rota escolhida
    app.UseExceptionHandler("/error-development");

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var version = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        {
            foreach (var description in version.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                    $"Web Api - {description.GroupName.ToUpper()}");
            }
        }
    });
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.EntityFrameworkCore.Extensions;
using Sistema_Control_Seguimiento_Backend.Filters;
using Sistema_Control_Seguimiento_Backend.Helpers;
using Sistema_Control_Seguimiento_Backend.Profiles;
using Sistema_Control_Seguimiento_Backend.Token;
using System.Text;

namespace Sistema_Control_Seguimiento_Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("defaultConnection");



            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });


            // Add services to the container.
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ExceptionFilter));

            });


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true, //valida el tiempo de vida
                    ValidateIssuerSigningKey = true, //valida la firma con la llave privada
                    IssuerSigningKey = new SymmetricSecurityKey( //configuramos la llave
                  Encoding.UTF8.GetBytes(builder.Configuration["keyjwt"])),
                    ClockSkew = TimeSpan.Zero //para no tener problemas con diferencias de tiempo al calcular que el token ha vencido.
                });


            //Inyectar servicios de mapper
            var mapperConfig = new MapperConfiguration(mapperConfig =>
            {
                mapperConfig.AddProfile(new RolProfile());
                mapperConfig.AddProfile(new UsuarioProfile());
                mapperConfig.AddProfile(new AlumnoProfile());
                mapperConfig.AddProfile(new CursoProfile());
            });

            //actimos la caché para tener acceso a los servicios del sistema
            //services.AddResponseCaching();//filtro caché
            //Configurar el sistema para inyección de dependencias
            //Cuando se solicita un servicio del tipo 'IRepositorio', se va a servir de la clase 'RepositorioEnMemoria'
            //services.AddScoped<IRepositorio, RepositorioEnMemoria>();//Servicio de nuestra apliación
            //'AddTransient' tiene un ciclo de vida corto, es decir, que cada que se solicita este servicio se crea una nueva instancia.
            //'AddScoped' el tiempo de vida de este servicio durará toda la petición http. Si realizan una misma petición se ler servirá la misma instancia. Tendran los mismos resultados minetras dure la petición
            //'AddSingleton' el tiempo de instancia de este servicio será durante todo el tiempo de ejecucíón de la aplicación, distintos clientes compartiran la misma instancia de la clase repositorioenmemoria. Siempre tendrpan el mismo resultado mienstras dure la ejecución del sistema.

            IMapper mapper = mapperConfig.CreateMapper();
            //El singleton mantiene este único objeto que se ejecuta durante toda la vida de ejecución de la aplicación.
            builder.Services.AddSingleton(mapper);

            //Doy de alta servicio para que se pueda obtener usuario (importante abajo)
            builder.Services.AddScoped<IUserSession, UserSession>();

            //Almacenar archivos de manera local
            builder.Services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivos>();


            //Debemos de registrar este Servicio o dará error el de IUSerSession (No está conectado de forma predeterminada)
            builder.Services.AddHttpContextAccessor();



            //Configuración de cors, para que cualquier cliente pueda aceptar los métodos
            builder.Services.AddCors(o => o.AddPolicy("corsApp", builder =>
            {
                builder.WithOrigins("*")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyHeader()
                .WithExposedHeaders(new string[] { "cantidadTotalRegistros" });
            }));


            var app = builder.Build();

            //Middleware que permite servir archivos estaticos, esto es para almacenar imagenes, por ejemplo 'video: guradando una imagen localmente'
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //cors
            app.UseCors("corsApp");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MySql.Data.EntityFrameworkCore.Extensions;
using Sistema_Control_Seguimiento_Backend.Profiles;

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

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Inyectar servicios de mapper
            var mapperConfig = new MapperConfiguration(mapperConfig =>
            {
                mapperConfig.AddProfile(new RolProfile());
                mapperConfig.AddProfile(new UsuarioProfile());
                mapperConfig.AddProfile(new AlumnoProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            //El singleton mantiene este único objeto que se ejecuta durante toda la vida de ejecución de la aplicación.
            builder.Services.AddSingleton(mapper);


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
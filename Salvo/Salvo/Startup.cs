using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Salvo.Models;
using Salvo.Repositories;
using System;

namespace Salvo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            //Inyección de dependencia para salvo context
            //services.AddDbContext<SalvoContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("SalvoDataBase")));

            /*----------------------Actividad 11---------------------------------*/
            //Para mejor performance entre entityframework 5 y 6
            services.AddDbContext<SalvoContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("SalvoDataBase"),
                o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
            /*-------------------------------------------------------------------------*/

            //Inyectar repositorio de game
            services.AddScoped<IGameRepository, GameRepository>();

            services.AddScoped<IGamePlayerRepository, GamePlayerRepository>();

            services.AddScoped<IPlayerRepository, PlayerRepository>();

            /*----------------------Actividad 11---------------------------------*/
            services.AddScoped<IScoreRepository, ScoreRepository>();
            /*-------------------------------------------------------*/

            //Agrego servicio de autenticacion 
            //Le defino un parametro por defecto CookieAuthenticationDefaults
            //Se hace mediante el mecanismo de cookies
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                //Expresion lamba con sus opciones 
                .AddCookie(options =>
                {
                    //opcion de tiempo de expiracion es igual a 10 min
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                    //despues que haga el login me dirija a una pagina en particular 
                    options.LoginPath = new PathString("/index.html");
                });

            //Agrego servicio de autorizacion
            services.AddAuthorization(options =>
            {
                //Agrego la politica de solo player(PlayerOnly) y que requiere un Claim q se llama Player
                options.AddPolicy("PlayerOnly", policy => policy.RequireClaim("Player"));
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            //Le digo que use autenticacion
            app.UseAuthentication();
            //Le digo que use autorizacion
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=games}/{ action = Get}");
            });
        }
    }
}

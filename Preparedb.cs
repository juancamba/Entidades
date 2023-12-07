using Entidades.Models;

namespace Entidades
{
    public class Preparedb
    {
        public static void Population(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        public static void SeedData(AppDbContext context)
        {
            context.Database.EnsureCreated();
            if (!context.Campos.Any())
            {
                context.Campos.Add(new Campo { Id = 1, Nombre = "Neumología" });
                context.SaveChanges();

            }

            if (!context.TiposMuestras.Any())
            {
                context.TiposMuestras.Add(new TiposMuestra { Id = 1, Nombre = "Espiromtría" });
                context.SaveChanges();
            }

        }
    }
}

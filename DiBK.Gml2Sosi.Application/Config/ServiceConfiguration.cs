using DiBK.Gml2Sosi.Application.Mappers;
using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;
using Microsoft.Extensions.DependencyInjection;

namespace DiBK.Gml2Sosi.Application.Config
{
    public static class ServiceConfiguration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<ISosiObjectTypeMapper, SosiObjectTypeMapper>();
            services.AddTransient<ISosiObjectMapper, SosiObjectMapper>();
            services.AddTransient<ISosiMapper<Ident>, IdentMapper>();
            services.AddTransient<IHodeMapper, HodeMapper>();
        }
    }
}

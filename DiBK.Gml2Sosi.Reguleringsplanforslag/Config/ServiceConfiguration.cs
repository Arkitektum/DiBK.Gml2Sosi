using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Services.Gml2Sosi;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers.Interfaces;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Services.Gml2Sosi;
using Microsoft.Extensions.DependencyInjection;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Config
{
    public static class ServiceConfiguration
    {
        public static void AddApplicationServicesForReguleringsplanforslag(this IServiceCollection services)
        {
            services.AddTransient<IGml2SosiService, RpfGml2SosiService>();
            services.AddTransient<ISosiMapper<NasjonalArealplanId>, NasjonalArealplanIdMapper>();
            services.AddTransient<ISosiElementMapper<RpGrense>, RpGrenseMapper>();
            services.AddTransient<ISosiElementMapper<RpFormålGrense>, RpFormålGrenseMapper>();
            services.AddTransient<ISosiElementMapper<RpOmråde>, RpOmrådeMapper>();
            services.AddTransient<ISosiElementMapper<RpArealformålOmråde>, RpArealformålOmrådeMapper>();
            services.AddTransient<ISosiElementMapper<RpAngittHensynSone>, RpAngittHensynSoneMapper>();
            services.AddTransient<ISosiElementMapper<RpBåndleggingSone>, RpBåndleggingSoneMapper>();
            services.AddTransient<ISosiElementMapper<RpDetaljeringSone>, RpDetaljeringSoneMapper>();
            services.AddTransient<ISosiElementMapper<RpFareSone>, RpFareSoneMapper>();
            services.AddTransient<ISosiElementMapper<RpGjennomføringSone>, RpGjennomføringSoneMapper>();
            services.AddTransient<ISosiElementMapper<RpInfrastrukturSone>, RpInfrastrukturSoneMapper>();
            services.AddTransient<ISosiElementMapper<RpSikringSone>, RpSikringSoneMapper>();
            services.AddTransient<ISosiElementMapper<RpStøySone>, RpStøySoneMapper>();
            services.AddTransient<ISosiElementMapper<RpBestemmelseOmråde>, RpBestemmelseOmrådeMapper>();
            services.AddTransient<ISosiElementMapper<PblMidlByggAnleggOmråde>, PblMidlByggAnleggOmrådeMapper>();
            services.AddTransient<ISosiElementMapper<RpPåskrift>, RpPåskriftMapper>();
            services.AddTransient<ISosiElementMapper<RpJuridiskPunkt>, RpJuridiskPunktMapper>();
            services.AddTransient<ISosiCurveObjectMapper<RpRegulertHøyde>, RpRegulertHøydeMapper>();
            services.AddTransient<ISosiCurveObjectMapper<RpJuridiskLinje>, RpJuridiskLinjeMapper>();
            services.AddTransient<IRpHensynSoneMapper, RpHensynSoneMapper>();
            services.AddTransient<IRpHandlingOmrådeMapper, RpHandlingOmrådeMapper>();
        }
    }
}

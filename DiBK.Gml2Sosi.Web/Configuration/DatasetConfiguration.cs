using DiBK.Gml2Sosi.Application.Models.Config;

namespace DiBK.Gml2Sosi.Web.Configuration
{
    public static class DatasetConfiguration
    {
        public static void ConfigureDatasets(this IServiceCollection services, IConfiguration configuration)
        {
            var datasets = new Dictionary<string, DatasetSettings>();
            configuration.GetSection(Datasets.SectionName).Bind(datasets);
            var datasetConfiguration = new Datasets(datasets);

            services.AddSingleton(datasetConfiguration);
        }
    }
}

namespace DiBK.Gml2Sosi.Application.Models.Config
{
    public class Datasets
    {
        public static readonly string SectionName = "Datasets";
        private readonly Dictionary<string, DatasetSettings> _datasets;

        public Datasets(Dictionary<string, DatasetSettings> datasets)
        {
            _datasets = datasets;
        }

        public DatasetSettings GetSettings(string name)
        {
            return _datasets.TryGetValue(name, out var settings) ? settings : null;
        }
    }
}

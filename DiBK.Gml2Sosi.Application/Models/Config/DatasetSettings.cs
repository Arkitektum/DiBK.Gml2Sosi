namespace DiBK.Gml2Sosi.Application.Models.Config
{
    public class DatasetSettings
    {
        public string SosiVersion { get; set; }
        public string SosiLevel { get; set; }
        public string ObjectCatalog { get; set; }
        public double Resolution { get; set; }
        public string VerticalDatum { get; set; }
        public Dictionary<string, string> CoordinateSystems { get; set; }
    }
}

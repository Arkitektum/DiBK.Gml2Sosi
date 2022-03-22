namespace DiBK.Gml2Sosi.Application.HttpClients.Codelist
{
    public class CodelistSettings
    {
        public static readonly string SectionName = "Codelists";
        public Uri Målemetode { get; set; }
        public Uri MålemetodeKode { get; set; }
        public int CacheDurationDays { get; set; }
    }
}

using DiBK.Gml2Sosi.Application.Attributes;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects
{
    [FeatureMember("RpBestemmelseOmråde")]
    public class RpBestemmelseOmråde : RpfSosiSurfaceObject
    {
        public override string ObjType { get; } = "RpBestemmelseOmråde";
        [SosiProperty("..BESTEMMELSEOMRNAVN", 7)]
        public string BestemmelseOmrådeNavn { get; set; }
        [SosiProperty("..RPBESTEMMELSEHJEMMEL", 8)]
        public string BestemmelseHjemmel { get; set; }
    }
}

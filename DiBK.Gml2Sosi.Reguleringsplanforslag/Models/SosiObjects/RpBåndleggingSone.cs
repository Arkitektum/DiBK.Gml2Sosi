using DiBK.Gml2Sosi.Application.Attributes;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects
{
    [FeatureMember("RpBåndleggingSone")]
    public class RpBåndleggingSone : RpHensynSone
    {
        public override string ObjType { get; } = "RpBåndleggingSone";
        [SosiProperty("..RPBÅNDLEGGING", 9)]
        public string Båndlegging { get; set; }
        [SosiProperty("..BÅNDLAGTFREMTIL", 10)]
        public string BåndlagtFremTil { get; set; }
    }
}

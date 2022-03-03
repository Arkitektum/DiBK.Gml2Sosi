using DiBK.Gml2Sosi.Application.Attributes;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects
{
    [FeatureMember("RpStøySone")]
    public class RpStøySone : RpHensynSone
    {
        public override string ObjType { get; } = "RpStøySone";
        [SosiProperty("..STØY", 9)]
        public string Støy { get; set; }
    }
}

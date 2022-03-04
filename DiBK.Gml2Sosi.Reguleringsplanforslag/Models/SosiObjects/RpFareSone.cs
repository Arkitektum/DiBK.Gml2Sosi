using DiBK.Gml2Sosi.Application.Attributes;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects
{
    [FeatureMember("RpFareSone")]
    public class RpFareSone : RpHensynSone
    {
        public override string ObjType { get; } = "RpFareSone";
        [SosiProperty("..RPFARE", 9)]
        public string Fare { get; set; }
    }
}

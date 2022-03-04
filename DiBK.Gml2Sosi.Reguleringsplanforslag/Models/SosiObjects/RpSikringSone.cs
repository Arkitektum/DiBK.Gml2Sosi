using DiBK.Gml2Sosi.Application.Attributes;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects
{
    [FeatureMember("RpSikringSone")]
    public class RpSikringSone : RpHensynSone
    {
        public override string ObjType { get; } = "RpSikringSone";
        [SosiProperty("..RPSIKRING", 9)]
        public string Sikring { get; set; }
    }
}

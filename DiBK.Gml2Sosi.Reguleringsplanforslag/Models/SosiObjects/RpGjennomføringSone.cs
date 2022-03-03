using DiBK.Gml2Sosi.Application.Attributes;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects
{
    [FeatureMember("RpGjennomføringSone")]
    public class RpGjennomføringSone : RpHensynSone
    {
        public override string ObjType { get; } = "RpGjennomføringSone";
        [SosiProperty("..RPGJENNOMFØRING", 9)]
        public string Gjennomføring { get; set; }
    }
}

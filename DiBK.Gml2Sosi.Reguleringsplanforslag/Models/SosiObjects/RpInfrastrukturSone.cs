using DiBK.Gml2Sosi.Application.Attributes;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects
{
    [FeatureMember("RpInfrastrukturSone")]
    public class RpInfrastrukturSone : RpHensynSone
    {
        public override string ObjType { get; } = "RpInfrastrukturSone";
        [SosiProperty("..RPINFRASTRUKTUR", 9)]
        public string Infrastruktur { get; set; }
    }
}

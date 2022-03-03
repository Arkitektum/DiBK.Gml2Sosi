using DiBK.Gml2Sosi.Application.Attributes;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects
{
    [FeatureMember("RpAngittHensynSone")]
    public class RpAngittHensynSone : RpHensynSone
    {
        public override string ObjType { get; } = "RpAngittHensynSone";
        [SosiProperty("..RPANGITTHENSYN", 9)]
        public string AngittHensyn { get; set; }
    }
}

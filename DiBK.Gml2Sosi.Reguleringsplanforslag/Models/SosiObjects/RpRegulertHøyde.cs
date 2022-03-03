using DiBK.Gml2Sosi.Application.Attributes;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects
{
    [FeatureMember("RpRegulertHøyde")]
    public class RpRegulertHøyde : SosiCurveObject
    {
        public override string ObjType { get; } = "RpRegulertHøyde";
        [SosiProperty("..NASJONALAREALPLANID", 5)]
        public NasjonalArealplanId NasjonalArealplanId { get; set; }
        [SosiProperty("..VERTNIV", 6)]
        public string Vertikalnivå { get; set; }
        [SosiProperty("..HØYDEFRAPLANBEST", 7)]
        public HøydeFraPlanbestemmelse HøydeFraPlanbestemmelse { get; set; } = new();
    }
}

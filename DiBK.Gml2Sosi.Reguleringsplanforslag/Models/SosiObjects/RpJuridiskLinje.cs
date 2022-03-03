using DiBK.Gml2Sosi.Application.Attributes;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects
{
    [FeatureMember("RpJuridiskLinje")]
    public class RpJuridiskLinje : SosiCurveObject
    {
        public override string ObjType { get; } = "RpJuridiskLinje";
        [SosiProperty("..NASJONALAREALPLANID", 5)]
        public NasjonalArealplanId NasjonalArealplanId { get; set; }
        [SosiProperty("..RPJURLINJE", 6)]
        public string JuridiskLinje { get; set; }
        [SosiProperty("..VERTNIV", 7)]
        public string Vertikalnivå { get; set; }
    }
}

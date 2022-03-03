using DiBK.Gml2Sosi.Application.Attributes;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects
{
    [FeatureMember("RpDetaljeringSone")]
    public class RpDetaljeringSone : RpHensynSone
    {
        public override string ObjType { get; } = "RpDetaljeringSone";
        [SosiProperty("..RPDETALJERING", 9)]
        public string Detaljering { get; set; }
    }
}

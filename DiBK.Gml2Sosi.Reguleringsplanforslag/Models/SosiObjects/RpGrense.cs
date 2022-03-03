using DiBK.Gml2Sosi.Application.Attributes;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects
{
    [FeatureMember("RpGrense")]
    public class RpGrense : SosiCurveObject
    {
        public override string ObjType { get; } = "RpGrense";
    }
}

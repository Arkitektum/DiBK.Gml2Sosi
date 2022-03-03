using DiBK.Gml2Sosi.Application.Attributes;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects
{
    [FeatureMember("RpFormålGrense")]
    public class RpFormålGrense : SosiCurveObject
    {
        public override string ObjType { get; } = "RpFormålGrense";
    }
}

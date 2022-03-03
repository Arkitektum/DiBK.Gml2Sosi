using DiBK.Gml2Sosi.Application.Attributes;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects
{
    [FeatureMember("RpOmråde")]
    public class RpOmråde : RpfSosiSurfaceObject
    {
        public override string ObjType { get; } = "RpOmråde";
        [SosiProperty("..PLANNAVN", 7)]
        public string Plannavn { get; set; }
        [SosiProperty("..PLANTYPE", 8)]
        public string Plantype { get; set; }
        [SosiProperty("..PLANSTAT", 9)]
        public string Planstatus { get; set; }
        [SosiProperty("..PLANBEST", 10)]
        public string Planbestemmelse { get; set; }
        [SosiProperty("..LOVREFERANSE", 11)]
        public string Lovreferanse { get; set; }
    }
}

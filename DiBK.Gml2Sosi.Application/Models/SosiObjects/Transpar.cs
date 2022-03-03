using DiBK.Gml2Sosi.Application.Attributes;

namespace DiBK.Gml2Sosi.Application.Models.SosiObjects
{
    public class Transpar
    {
        [SosiProperty("...KOORDSYS", 0)]
        public string Koordinatsystem { get; set; }
        [SosiProperty("...VERT-DATUM", 1)]
        public string VertikaltDatum { get; } = "NN2000";
        [SosiProperty("...ORIGO-NØ", 2)]
        public string OrigoNordØst { get; } = "0 0";
        [SosiProperty("...ENHET", 3)]
        public string Enhet { get; } = "0.01";
    }
}

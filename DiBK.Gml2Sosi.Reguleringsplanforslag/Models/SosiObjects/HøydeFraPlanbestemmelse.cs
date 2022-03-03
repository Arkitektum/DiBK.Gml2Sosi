using DiBK.Gml2Sosi.Application.Attributes;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects
{
    public class HøydeFraPlanbestemmelse
    {
        [SosiProperty("...REGULERTHØYDE", 0)]
        public string Regulerthøyde { get; set; }
        [SosiProperty("...HØYDE-REF", 1)]
        public string Høydereferansesystem { get; set; }
    }
}

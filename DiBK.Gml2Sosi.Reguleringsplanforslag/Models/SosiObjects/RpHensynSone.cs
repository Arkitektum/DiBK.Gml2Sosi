using DiBK.Gml2Sosi.Application.Attributes;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects
{
    public abstract class RpHensynSone : RpfSosiSurfaceObject
    {
        [SosiProperty("..HENSYNSONENAVN", 7)]
        public string HensynSonenavn { get; set; }
        [SosiProperty("..BESKRIVELSE", 8)]
        public string Beskrivelse { get; set; }
    }
}

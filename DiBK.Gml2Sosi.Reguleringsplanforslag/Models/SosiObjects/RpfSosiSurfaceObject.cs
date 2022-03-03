using DiBK.Gml2Sosi.Application.Attributes;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects
{
    public abstract class RpfSosiSurfaceObject : SosiSurfaceObject
    {
        [SosiProperty("..NASJONALAREALPLANID", 5)]
        public NasjonalArealplanId NasjonalArealplanId { get; set; }
        [SosiProperty("..VERTNIV", 6)]
        public string Vertikalnivå { get; set; }
    }
}

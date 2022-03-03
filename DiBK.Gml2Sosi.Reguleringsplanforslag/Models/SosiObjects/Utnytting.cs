using DiBK.Gml2Sosi.Application.Attributes;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects
{
    public class Utnytting
    {
        [SosiProperty("...UTNTYP", 0)]
        public string Utnyttingstype { get; set; }
        [SosiProperty("...UTNTALL", 1)]
        public string Utnyttingstall { get; set; }
        [SosiProperty("...UTNTALL_MIN", 2)]
        public string UtnyttingstallMinimum { get; set; }
    }
}

using DiBK.Gml2Sosi.Application.Attributes;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects
{
    public class NasjonalArealplanId
    {
        [SosiProperty("...KOMM", 0)]
        public string Kommunenummer { get; set; }
        [SosiProperty("...PLANID", 1)]
        public string Planidentifikasjon { get; set; }
    }
}

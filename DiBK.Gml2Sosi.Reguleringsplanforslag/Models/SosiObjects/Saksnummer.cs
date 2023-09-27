using DiBK.Gml2Sosi.Application.Attributes;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects
{
    public class Saksnummer
    {
        [SosiProperty("...SAKSÅR", 0)]
        public string Saksår { get; set; }
        [SosiProperty("...SAKSSEKVENSNUMMER", 1)]
        public string Sakssekvensnummer { get; set; }
    }
}

using DiBK.Gml2Sosi.Application.Attributes;

namespace DiBK.Gml2Sosi.Application.Models.SosiObjects
{
    public class Ident
    {
        [SosiProperty("...LOKALID", 0)]
        public string LokalId { get; set; }
        [SosiProperty("...NAVNEROM", 1)]
        public string Navnerom { get; set; }
        [SosiProperty("...VERSJONID", 2)]
        public string VersjonId { get; set; }
    }
}

using DiBK.Gml2Sosi.Application.Attributes;

namespace DiBK.Gml2Sosi.Application.Models.SosiObjects
{
    public class Område
    {
        [SosiProperty("...MIN-NØ", 0)]
        public string MinNordØst { get; set; }
        [SosiProperty("...MAX-NØ", 1)]
        public string MaxNordØst { get; set; }
    }
}

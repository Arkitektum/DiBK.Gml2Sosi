using DiBK.Gml2Sosi.Application.Attributes;

namespace DiBK.Gml2Sosi.Application.Models.SosiObjects
{
    public enum CartographicElementType
    {
        [SosiProperty(".PUNKT")]
        Punkt,
        [SosiProperty(".KURVE")]
        Kurve,
        [SosiProperty(".BUEP")]
        BueP,
        [SosiProperty(".FLATE")]
        Flate,
        [SosiProperty(".TEKST")]
        Tekst,
        [SosiProperty(".SYMBOL")]
        Symbol,
        Unknown
    }
}

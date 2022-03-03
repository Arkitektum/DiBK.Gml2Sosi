using DiBK.Gml2Sosi.Application.Attributes;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects
{
    [FeatureMember("RpBestemmelseMidlByggAnlegg")]
    public class PblMidlByggAnleggOmråde : RpfSosiSurfaceObject
    {
        public override string ObjType { get; } = "PblMidlByggAnleggOmråde";
        [SosiProperty("..SAKSNUMMER", 7)]
        public Saksnummer Saksnummer { get; set; }
        [SosiProperty("..AVGJDATO", 1)]
        public string Avgjørelsesdato { get; set; }
        [SosiProperty("..GYLDIGTILDATO", 1)]
        public string GyldigTilDato { get; set; }
    }
}

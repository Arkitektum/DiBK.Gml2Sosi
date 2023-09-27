using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects;
using System.Xml.Linq;
using Wmhelp.XPath2;
using static DiBK.Gml2Sosi.Application.Helpers.MapperHelper;
using static DiBK.Gml2Sosi.Reguleringsplanforslag.Helpers.MapperHelper;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers
{
    public class PblMidlByggAnleggOmrådeMapper : ISosiElementMapper<PblMidlByggAnleggOmråde>
    {
        private readonly ISosiObjectTypeMapper _sosiObjectMapper;
        private readonly ISosiMapper<NasjonalArealplanId> _nasjonalArealplanIdMapper;

        public PblMidlByggAnleggOmrådeMapper(
            ISosiObjectTypeMapper sosiObjectMapper,
            ISosiMapper<NasjonalArealplanId> nasjonalArealplanIdMapper)
        {
            _sosiObjectMapper = sosiObjectMapper;
            _nasjonalArealplanIdMapper = nasjonalArealplanIdMapper;
        }

        public PblMidlByggAnleggOmråde Map(XElement featureElement, GmlDocument document)
        {
            var pblMidlByggAnleggOmråde = _sosiObjectMapper.Map<PblMidlByggAnleggOmråde>(featureElement, document);
            var rpOmrådeElement = GetRpOmrådeElementByGeometry(featureElement, document);

            pblMidlByggAnleggOmråde.NasjonalArealplanId = _nasjonalArealplanIdMapper.Map(featureElement, document);
            pblMidlByggAnleggOmråde.Vertikalnivå = rpOmrådeElement.XPath2SelectElement("*:vertikalnivå")?.Value;

            var saksnummerElement = featureElement.XPath2SelectElement("*:saksnummer/*:Saksnummer");

            if (saksnummerElement != null)
            {
                pblMidlByggAnleggOmråde.Saksnummer = new Saksnummer
                {
                    Saksår = saksnummerElement.XPath2SelectElement("*:saksår")?.Value,
                    Sakssekvensnummer = saksnummerElement.XPath2SelectElement("*:sakssekvensnummer")?.Value,
                };
            }

            pblMidlByggAnleggOmråde.BestemmelseOmrådeNavn = featureElement.XPath2SelectElement("*:bestemmelseOmrådeNavn")?.Value;

            var avgjørelsesdatoElement = featureElement.XPath2SelectElement("*:avgjørelsesdato");

            pblMidlByggAnleggOmråde.Avgjørelsesdato = avgjørelsesdatoElement != null ?
                 FormatDate(avgjørelsesdatoElement) : 
                 DateTime.Today.ToString("yyyyMMdd");

            var gyldigTilDatoElement = featureElement.XPath2SelectElement("*:gyldigTilDato");

            pblMidlByggAnleggOmråde.GyldigTilDato = gyldigTilDatoElement != null ?
                FormatDate(gyldigTilDatoElement) :
                DateTime.Today.AddYears(5).ToString("yyyyMMdd");

            pblMidlByggAnleggOmråde.ElementType = CartographicElementType.Flate;

            return pblMidlByggAnleggOmråde;
        }
    }
}

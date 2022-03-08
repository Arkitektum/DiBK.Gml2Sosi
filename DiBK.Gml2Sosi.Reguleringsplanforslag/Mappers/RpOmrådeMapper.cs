using DiBK.Gml2Sosi.Application.Helpers;
using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Constants;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects;
using System.Xml.Linq;
using Wmhelp.XPath2;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers
{
    public class RpOmrådeMapper : ISosiElementMapper<RpOmråde>
    {
        private readonly ISosiObjectTypeMapper _sosiObjectTypeMapper;
        private readonly ISosiMapper<NasjonalArealplanId> _nasjonalArealplanIdMapper;

        public RpOmrådeMapper(
            ISosiObjectTypeMapper sosiObjectTypeMapper,
            ISosiMapper<NasjonalArealplanId> nasjonalArealplanIdMapper)
        {
            _sosiObjectTypeMapper = sosiObjectTypeMapper;
            _nasjonalArealplanIdMapper = nasjonalArealplanIdMapper;
        }

        public RpOmråde Map(XElement featureElement, GmlDocument document)
        {
            var rpOmråde = _sosiObjectTypeMapper.Map<RpOmråde>(featureElement, document);
            var arealplanElement = featureElement.Document.Root.Descendants(Namespace.App + "Arealplan").FirstOrDefault();

            rpOmråde.NasjonalArealplanId = _nasjonalArealplanIdMapper.Map(featureElement, document);

            rpOmråde.Plannavn = MapperHelper.FormatText(arealplanElement.XPath2SelectElement("*:plannavn"));
            rpOmråde.Plantype = arealplanElement.XPath2SelectElement("*:plantype")?.Value;
            rpOmråde.Planstatus = arealplanElement.XPath2SelectElement("*:planstatus")?.Value;
            rpOmråde.Planbestemmelse = arealplanElement.XPath2SelectElement("*:omPlanbestemmelser")?.Value;
            rpOmråde.Lovreferanse = arealplanElement.XPath2SelectElement("*:lovreferanse")?.Value;
            rpOmråde.Vertikalnivå = featureElement.XPath2SelectElement("*:vertikalnivå")?.Value;
            rpOmråde.ElementType = CartographicElementType.Flate;

            return rpOmråde;
        }
    }
}

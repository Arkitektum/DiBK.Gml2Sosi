using DiBK.Gml2Sosi.Application.Helpers;
using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Application.Models.Config;
using DiBK.Gml2Sosi.Application.Models.Geometries;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Constants;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects;
using System.Xml.Linq;
using Wmhelp.XPath2;
using static DiBK.Gml2Sosi.Reguleringsplanforslag.Helpers.MapperHelper;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers
{
    public class RpJuridiskPunktMapper : ISosiElementMapper<RpJuridiskPunkt>
    {
        private readonly ISosiObjectTypeMapper _sosiObjectTypeMapper;
        private readonly ISosiMapper<NasjonalArealplanId> _nasjonalArealplanIdMapper;
        private readonly DatasetSettings _settings;

        public RpJuridiskPunktMapper(
            ISosiObjectTypeMapper sosiObjectTypeMapper,
            ISosiMapper<NasjonalArealplanId> nasjonalArealplanIdMapper,
            Datasets datasets)
        {
            _sosiObjectTypeMapper = sosiObjectTypeMapper;
            _nasjonalArealplanIdMapper = nasjonalArealplanIdMapper;
            _settings = datasets.GetSettings(Dataset.Reguleringsplanforslag);
        }

        public RpJuridiskPunkt Map(XElement featureElement, GmlDocument document, ref int sequenceNumber)
        {
            var rpJuridiskPunkt = _sosiObjectTypeMapper.Map<RpJuridiskPunkt>(featureElement, document, ref sequenceNumber);

            rpJuridiskPunkt.NasjonalArealplanId = _nasjonalArealplanIdMapper.Map(featureElement, document);
            rpJuridiskPunkt.JuridiskPunkt = featureElement.XPath2SelectElement("*:juridiskpunkt")?.Value;
            rpJuridiskPunkt.Points = GetPoints(featureElement);
            rpJuridiskPunkt.ElementType = CartographicElementType.Symbol;

            var rpOmrådeElement = GetReferencedRpOmrådeElement(featureElement, document);

            if (rpOmrådeElement != null)
                rpJuridiskPunkt.Vertikalnivå = rpOmrådeElement.XPath2SelectElement("*:vertikalnivå")?.Value;

            return rpJuridiskPunkt;
        }

        private List<SosiPoint> GetPoints(XElement element)
        {
            var geomElement = element.XPath2SelectElement("*:posisjon/*");
            var symbolretning = element.XPath2SelectElement("*:symbolretning")?.Value;
            var punkt = GeometryHelper.GetSosiPoints(geomElement, _settings.Resolution).SingleOrDefault();
            var punkter = new List<SosiPoint> { punkt };

            if (string.IsNullOrWhiteSpace(symbolretning))
                return punkter;

            var vektorpunkt = symbolretning.Split(" ");

            if (vektorpunkt.Length != 2 || !double.TryParse(vektorpunkt[0].Trim(), out var vektorpunktX) || !double.TryParse(vektorpunkt[1].Trim(), out var vektorpunktY))
                return punkter;

            var x = Math.Round(punkt.X * _settings.Resolution + vektorpunktX, 2);
            var y = Math.Round(punkt.Y * _settings.Resolution + vektorpunktY, 2);

            punkter.Add(punkt);
            punkter.Add(SosiPoint.Create(x, y, _settings.Resolution));

            return punkter;
        }
    }
}

using DiBK.Gml2Sosi.Application.Helpers;
using DiBK.Gml2Sosi.Application.HttpClients.Codelist;
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
        private readonly ICodelistHttpClient _codelistHttpClient;
        private readonly DatasetSettings _settings;

        public RpJuridiskPunktMapper(
            ISosiObjectTypeMapper sosiObjectTypeMapper,
            ISosiMapper<NasjonalArealplanId> nasjonalArealplanIdMapper,
            ICodelistHttpClient codelistHttpClient,
            Datasets datasets)
        {
            _sosiObjectTypeMapper = sosiObjectTypeMapper;
            _nasjonalArealplanIdMapper = nasjonalArealplanIdMapper;
            _codelistHttpClient = codelistHttpClient;
            _settings = datasets.GetSettings(Dataset.Reguleringsplanforslag);
        }

        public RpJuridiskPunkt Map(XElement featureElement, GmlDocument document)
        {
            var rpJuridiskPunkt = _sosiObjectTypeMapper.Map<RpJuridiskPunkt>(featureElement, document);

            rpJuridiskPunkt.NasjonalArealplanId = _nasjonalArealplanIdMapper.Map(featureElement, document);
            rpJuridiskPunkt.Kvalitet = _codelistHttpClient.GetMålemetodeAsync(featureElement).Result;
            rpJuridiskPunkt.JuridiskPunkt = featureElement.XPath2SelectElement("*:juridiskpunkt")?.Value;
            rpJuridiskPunkt.Points = GetPoints(featureElement);
            rpJuridiskPunkt.ElementType = CartographicElementType.Symbol;

            var rpOmrådeElement = GetRpOmrådeElementByGeometry(featureElement, document);

            if (rpOmrådeElement != null)
                rpJuridiskPunkt.Vertikalnivå = rpOmrådeElement.XPath2SelectElement("*:vertikalnivå")?.Value;

            return rpJuridiskPunkt;
        }

        private List<SosiPoint> GetPoints(XElement element)
        {
            var geomElement = element.XPath2SelectElement("*:posisjon/*");
            var symbolretning = element.XPath2SelectElement("*:symbolretning")?.Value;
            var point = GeometryHelper.GetSosiPoints(geomElement, _settings.Resolution).SingleOrDefault();
            var points = new List<SosiPoint> { point };

            if (string.IsNullOrWhiteSpace(symbolretning))
                return points;

            var vectorPoints = symbolretning.Split(" ");

            if (vectorPoints.Length != 2 || !double.TryParse(vectorPoints[0].Trim(), out var vectorPointX) || !double.TryParse(vectorPoints[1].Trim(), out var vectorPointY))
                return points;

            var x = Math.Round(point.X * _settings.Resolution + vectorPointX, 2);
            var y = Math.Round(point.Y * _settings.Resolution + vectorPointY, 2);

            points.Add(point);
            points.Add(SosiPoint.Create(x, y, _settings.Resolution));

            return points;
        }
    }
}

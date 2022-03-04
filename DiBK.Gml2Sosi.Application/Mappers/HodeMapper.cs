using DiBK.Gml2Sosi.Application.Helpers;
using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Application.Models.Config;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;
using System.Xml.Linq;
using Wmhelp.XPath2;

namespace DiBK.Gml2Sosi.Application.Mappers
{
    public class HodeMapper : IHodeMapper
    {
        public void Map(GmlDocument document, DatasetSettings settings, List<SosiElement> sosiElements)
        {
            var rootElement = document.Document.Root;
            var srsNameElement = rootElement.Descendants()
                .FirstOrDefault(element => element.Attribute("srsName") != null);

            var envelopeElement = rootElement.XPath2SelectElement("*:boundedBy/*:Envelope");
            var lowerCornerElement = envelopeElement?.XPath2SelectElement("*:lowerCorner");
            var upperCornerElement = envelopeElement?.XPath2SelectElement("*:upperCorner");

            var hode = new Hode
            {
                SequenceNumber = 0,
                SosiVersjon = settings.SosiVersion,
                SosiNivå = settings.SosiLevel,
                Objektkatalog = settings.ObjectCatalog,
                Transpar = new()
                {
                    Enhet = settings.Resolution.ToString(),
                    VertikaltDatum = settings.VerticalDatum,
                    Koordinatsystem = GmlHelper.GetCoordinateSystem(srsNameElement?.Attribute("srsName").Value, settings.CoordinateSystems)
                },
                Område = new()
                {
                    MinNordØst = GetEnvelopePoint(lowerCornerElement, coord => Math.Floor(coord)),
                    MaxNordØst = GetEnvelopePoint(upperCornerElement, coord => Math.Ceiling(coord))
                }
            };

            sosiElements.Add(hode);
        }

        private static string GetEnvelopePoint(XElement element, Func<double, double> roundingFunc)
        {
            var value = element?.Value;

            if (string.IsNullOrWhiteSpace(value))
                return "0 0";

            var coordinates = value.Split(" ");

            if (coordinates.Length != 2)
                return "0 0";

            var x = double.TryParse(coordinates[0].Trim(), out var xCoord) ? (long)Math.Truncate(roundingFunc.Invoke(xCoord)) : 0;
            var y = double.TryParse(coordinates[1].Trim(), out var yCoord) ? (long)Math.Truncate(roundingFunc.Invoke(yCoord)) : 0;

            return $"{y} {x}";
        }
    }
}

using DiBK.Gml2Sosi.Application.Attributes;
using DiBK.Gml2Sosi.Application.Models.Geometries;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;
using System.Reflection;
using System.Xml.Linq;
using Wmhelp.XPath2;

namespace DiBK.Gml2Sosi.Application.Helpers
{
    public class MapperHelper
    {
        public static string GetFeatureMemberName<TSosiModel>() where TSosiModel : SosiElement
        {
            var featureMemberAttribute = typeof(TSosiModel).GetCustomAttribute<FeatureMemberAttribute>();

            if (featureMemberAttribute == null)
                throw new Exception();

            return featureMemberAttribute.FeatureMember;
        }

        public static string FormatText(XElement element)
        {
            var value = element?.Value;

            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (value.StartsWith('"') && value.EndsWith('"') || value.StartsWith('\'') && value.EndsWith('\''))
                return value;

            return $"\"{value}\"";
        }

        public static string FormatDate(XElement element)
        {
            var value = element?.Value;

            if (!string.IsNullOrWhiteSpace(value) && DateTime.TryParse(value, out var date))
                return date.ToString("yyyyMMdd");

            return null;
        }

        public static string GetLocalId(XElement element)
        {
            var featureElement = GmlHelper.GetFeatureElement(element);

            return featureElement.XPath2SelectElement("*:identifikasjon/*:Identifikasjon/*:lokalId")?.Value;
        }

        public static CartographicElementType SegmentTypeToCartographicElementType(SegmentType segmentType)
        {
            return segmentType switch
            {
                SegmentType.BueP => CartographicElementType.BueP,
                SegmentType.Kurve => CartographicElementType.Kurve,
                SegmentType.Unknown => CartographicElementType.Unknown,
                _ => CartographicElementType.Unknown,
            };
        }

        public static SegmentType CartographicElementTypeToSegmentType(CartographicElementType elementType)
        {
            return elementType switch
            {
                CartographicElementType.BueP => SegmentType.BueP,
                CartographicElementType.Kurve => SegmentType.Kurve,
                _ => SegmentType.Unknown,
            };
        }
    }
}

using System.Xml.Linq;

namespace DiBK.Gml2Sosi.Application.Models
{
    public class FeatureElement
    {
        public XElement Feature { get; set; }
        public XElement Geometry { get; set; }

        public FeatureElement(XElement feature, XElement geometry)
        {
            Feature = feature;
            Geometry = geometry;
        }
    }
}

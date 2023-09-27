using DiBK.Gml2Sosi.Application.Helpers;
using OSGeo.OGR;
using System.Xml.Linq;

namespace DiBK.Gml2Sosi.Application.Models
{
    public class IndexedGeometry : IDisposable
    {
        private bool _disposed = false;

        private IndexedGeometry(XElement element, Geometry geometry, string type)
        {
            Element = element;
            Geometry = geometry;
            Type = type;
        }

        public XElement Element { get; private set; }
        public Geometry Geometry { get; private set; }
        public string Type { get; private set; }

        public bool IsValid
        {
            get
            {
                try
                {
                    return Geometry != null && Geometry.IsValid();
                }
                catch
                {
                    return false;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing && Geometry != null)
                    Geometry.Dispose();

                _disposed = true;
            }
        }

        public static IndexedGeometry Create(XElement element)
        {
            Geometry geometry = GeometryHelper.OgrGeometryFromGml(element);

            return new IndexedGeometry(element, geometry, element.Name.LocalName);
        }
    }
}

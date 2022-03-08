using DiBK.Gml2Sosi.Application.Helpers;
using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Application.Models.Geometries;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;
using NetTopologySuite.Geometries;
using System.Xml.Linq;

namespace DiBK.Gml2Sosi.Application.Mappers
{
    public abstract class SosiCurveObjectMapper
    {
        private readonly ISosiObjectTypeMapper _sosiObjectTypeMapper;

        public SosiCurveObjectMapper(
            ISosiObjectTypeMapper sosiObjectTypeMapper)
        {
            _sosiObjectTypeMapper = sosiObjectTypeMapper;
        }

        /*protected async Task<TSosiCurveModel> MapCurveObjectAsync<TSosiCurveModel>(
            XElement featureElement, XElement geomElement, GmlDocument document, double resolution, ref int sequenceNumber)
            where TSosiCurveModel : SosiCurveObject, new()
        {
            var curveObject = _sosiObjectTypeMapper.Map<TSosiCurveModel>(featureElement, document, ref sequenceNumber);

            curveObject.Ident.LokalId = Guid.NewGuid().ToString();
            curveObject.ElementType = SosiCurveObject.GetElementType(geomElement);
            curveObject.Points = GeometryHelper.GetSosiPoints(geomElement, resolution);
            curveObject.Segment = new SosiSegment(curveObject);

            return curveObject;
        }

        protected async Task<TSosiCurveModel> MapCurveObjectAsync<TSosiCurveModel>(
            XElement featureElement, CartographicElementType elementType, LineString lineString, GmlDocument document, double resolution)
            where TSosiCurveModel : SosiCurveObject, new()
        {
            var sn = 0;
            var curveObject = _sosiObjectTypeMapper.Map<TSosiCurveModel>(featureElement, document, ref sn);

            curveObject.Ident.LokalId = Guid.NewGuid().ToString();
            curveObject.ElementType = elementType;
            curveObject.Points = GeometryHelper.GetSosiPoints(lineString, resolution);
            curveObject.Segment = new SosiSegment(curveObject);

            return curveObject;
        }*/

        protected TSosiCurveModel MapCurveObject<TSosiCurveModel>(
            XElement featureElement, XElement geomElement, GmlDocument document, double resolution)
            where TSosiCurveModel : SosiCurveObject, new()
        {
            var curveObject = _sosiObjectTypeMapper.Map<TSosiCurveModel>(featureElement, document);

            curveObject.Ident.LokalId = Guid.NewGuid().ToString();
            curveObject.ElementType = SosiCurveObject.GetElementType(geomElement);
            curveObject.Points = GeometryHelper.GetSosiPoints(geomElement, resolution);
            curveObject.Segment = new SosiSegment(curveObject);

            return curveObject;
        }

        protected TSosiCurveModel MapCurveObject<TSosiCurveModel>(
            XElement featureElement, CartographicElementType elementType, LineString lineString, GmlDocument document, double resolution)
            where TSosiCurveModel : SosiCurveObject, new()
        {
            var curveObject = _sosiObjectTypeMapper.Map<TSosiCurveModel>(featureElement, document);

            curveObject.Ident.LokalId = Guid.NewGuid().ToString();
            curveObject.ElementType = elementType;
            curveObject.Points = GeometryHelper.GetSosiPoints(lineString, resolution);
            curveObject.Segment = new SosiSegment(curveObject);

            return curveObject;
        }

        protected TSosiCurveModel MapCurveObject<TSosiCurveModel>(
            XElement featureElement, XElement geomElement, GmlDocument document, double resolution, ref int sequenceNumber) 
            where TSosiCurveModel : SosiCurveObject, new()
        {
            var curveObject = _sosiObjectTypeMapper.Map<TSosiCurveModel>(featureElement, document, ref sequenceNumber);

            curveObject.Ident.LokalId = Guid.NewGuid().ToString();
            curveObject.ElementType = SosiCurveObject.GetElementType(geomElement);
            curveObject.Points = GeometryHelper.GetSosiPoints(geomElement, resolution);
            curveObject.Segment = new SosiSegment(curveObject);

            return curveObject;
        }

        protected TSosiCurveModel MapCurveObject<TSosiCurveModel>(
            XElement featureElement, CartographicElementType elementType, LineString lineString, GmlDocument document, double resolution, ref int sequenceNumber)
            where TSosiCurveModel : SosiCurveObject, new()
        {
            var curveObject = _sosiObjectTypeMapper.Map<TSosiCurveModel>(featureElement, document, ref sequenceNumber);

            curveObject.Ident.LokalId = Guid.NewGuid().ToString();
            curveObject.ElementType = elementType;
            curveObject.Points = GeometryHelper.GetSosiPoints(lineString, resolution);
            curveObject.Segment = new SosiSegment(curveObject);

            return curveObject;
        }
    }
}

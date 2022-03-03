using DiBK.Gml2Sosi.Application.Helpers;
using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Application.Models.Geometries;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;
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

        public TSosiCurveModel MapCurveObject<TSosiCurveModel>(XElement featureElement, XElement geomElement, GmlDocument document, ref int sequenceNumber) 
            where TSosiCurveModel : SosiCurveObject, new()
        {
            var curveObject = _sosiObjectTypeMapper.Map<TSosiCurveModel>(featureElement, document, ref sequenceNumber);

            curveObject.Ident.LokalId = Guid.NewGuid().ToString();
            curveObject.ElementType = SosiCurveObject.GetElementType(geomElement);
            curveObject.Points = GeometryHelper.GetSosiPoints(geomElement, 0.01);
            curveObject.Segment = new SosiSegment(curveObject);

            return curveObject;
        }

        /*        private TSosiCurveModel MapCurveObject<TSosiCurveModel>(XElement featureElement, CartographicElementType elementType, LineString lineString, GmlDocument document, ref int sequenceNumber)
            where TSosiCurveModel : SosiCurveObject, new()
        {
            var curveObject = _sosiObjectTypeMapper.Map<TSosiCurveModel>(featureElement, document, ref sequenceNumber);

            curveObject.Ident.LokalId = Guid.NewGuid().ToString();
            curveObject.ElementType = elementType;
            curveObject.Points = GeometryHelper.GetSosiPoints(lineString);
            curveObject.Segment = new SosiSegment(curveObject);

            return curveObject;
        }*/
    }
}

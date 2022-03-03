using DiBK.Gml2Sosi.Application.Helpers;
using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;
using Microsoft.Extensions.DependencyInjection;
using Wmhelp.XPath2;

namespace DiBK.Gml2Sosi.Application.Mappers
{
    public class SosiObjectMapper : SosiCurveObjectMapper, ISosiObjectMapper
    {
        private readonly IServiceProvider _serviceProvider;

        public SosiObjectMapper(
            IServiceProvider serviceProvider,
            ISosiObjectTypeMapper sosiObjectTypeMapper) : base(sosiObjectTypeMapper)
        {
            _serviceProvider = serviceProvider;
        }

        public void MapSosiObjects<TSosiModel>(GmlDocument document, ref int sequenceNumber, List<SosiElement> sosiElements)
            where TSosiModel : SosiElement, new()
        {
            var featureMemberName = MapperHelper.GetFeatureMemberName<TSosiModel>();
            var sosiMapper = _serviceProvider.GetRequiredService<ISosiElementMapper<TSosiModel>>();
            var featureElements = document.GetFeatureElements(featureMemberName);

            foreach (var featureElement in featureElements)
                sosiElements.Add(sosiMapper.Map(featureElement, document, ref sequenceNumber));
        }

        public void MapSosiCurveObjects<TSosiCurveModel>(GmlDocument document, bool addNodes, ref int sequenceNumber, List<SosiElement> sosiElements)
            where TSosiCurveModel : SosiCurveObject, new()
        {
            var featureMemberName = MapperHelper.GetFeatureMemberName<TSosiCurveModel>();
            var curveObjects = new List<SosiCurveObject>();
            var curveMapper = _serviceProvider.GetRequiredService<ISosiCurveObjectMapper<TSosiCurveModel>>();
            var curveFeatureElements = document.GetFeatureElements(featureMemberName);

            foreach (var curveFeatureElement in curveFeatureElements)
            {
                var geomElement = GmlHelper.GetFeatureGeometryElements(curveFeatureElement).FirstOrDefault();

                if (geomElement == null)
                    continue;

                var segmentElements = geomElement.XPath2SelectElements("*:segments/*");
                var geomCount = segmentElements.Count();
                var localId = MapperHelper.GetLocalId(curveFeatureElement);
                var index = 0;

                foreach (var segmentElement in segmentElements)
                {
                    var curveObject = curveMapper.Map(curveFeatureElement, segmentElement, document, ref sequenceNumber);

                    if (geomCount > 1)
                        curveObject.Ident.LokalId = $"{localId}-{index++}";

                    curveObjects.Add(curveObject);
                }
            }

            if (addNodes)
                SosiCurveObject.AddNodesToCurves(curveObjects);

            sosiElements.AddRange(curveObjects);
        }

        public void MapSosiSurfaceAndCurveObjects<TSosiSurfaceModel, TSosiCurveModel>(GmlDocument document, ref int sequenceNumber, List<SosiElement> sosiElements, double resolution)
            where TSosiSurfaceModel : SosiSurfaceObject
            where TSosiCurveModel : SosiCurveObject, new()
        {
            var surfaceObjects = new List<SosiSurfaceObject>();
            var curveObjects = new List<SosiCurveObject>();

            var featureMemberName = MapperHelper.GetFeatureMemberName<TSosiSurfaceModel>();
            var surfaceMapper = _serviceProvider.GetRequiredService<ISosiElementMapper<TSosiSurfaceModel>>();
            var surfaceFeatureElements = document.GetFeatureElements(featureMemberName);

            foreach (var surfaceFeatureElement in surfaceFeatureElements)
            {
                var geomElement = GmlHelper.GetFeatureGeometryElements(surfaceFeatureElement).FirstOrDefault();

                if (geomElement == null)
                    continue;

                var surfaceMemberElements = geomElement.XPath2SelectElements("*:surfaceMember/*");
                var surfaceMemberCount = surfaceMemberElements.Count();
                var localId = MapperHelper.GetLocalId(surfaceFeatureElement);
                var index = 0;

                foreach (var surfaceMemberElement in surfaceMemberElements)
                {
                    var surfaceObject = surfaceMapper.Map(surfaceFeatureElement, document, ref sequenceNumber);

                    if (surfaceMemberCount > 1)
                        surfaceObject.Ident.LokalId = $"{localId}-{index++}";

                    var segmentElements = surfaceMemberElement.XPath2SelectElements("//*:segments/*");
                    var boundaryObjects = new List<TSosiCurveModel>();

                    foreach (var segmentElement in segmentElements)
                    {
                        var boundaryObject = MapCurveObject<TSosiCurveModel>(surfaceFeatureElement, segmentElement, document, ref sequenceNumber);
                        
                        boundaryObject.Ident.LokalId = Guid.NewGuid().ToString();
                        boundaryObjects.Add(boundaryObject);
                    }

                    SosiCurveObject.AddNodesToCurves(boundaryObjects);

                    surfaceObject.SetReferences(boundaryObjects);
                    surfaceObject.SetPointOnSurface(surfaceMemberElement, resolution);

                    surfaceObjects.Add(surfaceObject);
                    curveObjects.AddRange(boundaryObjects);
                }
            }

            sosiElements.AddRange(surfaceObjects);
            sosiElements.AddRange(curveObjects);
        }
    }
}

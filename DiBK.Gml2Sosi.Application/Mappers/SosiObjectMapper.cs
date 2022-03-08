using DiBK.Gml2Sosi.Application.Helpers;
using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Application.Models.Geometries;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;
using DiBK.Gml2Sosi.Application.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using NetTopologySuite.Operation.Linemerge;
using System.Xml.Linq;
using Wmhelp.XPath2;
using static DiBK.Gml2Sosi.Application.Helpers.MapperHelper;

namespace DiBK.Gml2Sosi.Application.Mappers
{
    public class SosiObjectMapper : SosiCurveObjectMapper, ISosiObjectMapper
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SosiObjectMapper> _logger;

        public SosiObjectMapper(
            IServiceProvider serviceProvider,
            ISosiObjectTypeMapper sosiObjectTypeMapper,
            ILogger<SosiObjectMapper> logger) : base(sosiObjectTypeMapper)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public void MapSosiObjects<TSosiModel>(GmlDocument document, List<SosiElement> sosiElements)
            where TSosiModel : SosiElement, new()
        {
            var sn = 0;
            var start = DateTime.Now;
            var featureMemberName = GetFeatureMemberName<TSosiModel>();
            var sosiMapper = _serviceProvider.GetRequiredService<ISosiElementMapper<TSosiModel>>();
            var featureElements = document.GetFeatureElements(featureMemberName);

            foreach (var featureElement in featureElements)
                sosiElements.Add(sosiMapper.Map(featureElement, document, ref sn));

            LogInformation<TSosiModel>(featureElements.Count, start);
        }

        public void MapSosiObjects<TSosiModel>(GmlDocument document, ref int sequenceNumber, List<SosiElement> sosiElements)
            where TSosiModel : SosiElement, new()
        {
            var start = DateTime.Now;
            var featureMemberName = GetFeatureMemberName<TSosiModel>();
            var sosiMapper = _serviceProvider.GetRequiredService<ISosiElementMapper<TSosiModel>>();
            var featureElements = document.GetFeatureElements(featureMemberName);

            foreach (var featureElement in featureElements)
                sosiElements.Add(sosiMapper.Map(featureElement, document, ref sequenceNumber));

            LogInformation<TSosiModel>(featureElements.Count, start);
        }

        public void MapSosiCurveObjects<TSosiCurveModel>(
            GmlDocument document, bool addNodes, List<SosiElement> sosiElements)
            where TSosiCurveModel : SosiCurveObject, new()
        {
            var sn = 0;
            var start = DateTime.Now;
            var featureMemberName = GetFeatureMemberName<TSosiCurveModel>();
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
                var localId = GetLocalId(curveFeatureElement);
                var index = 0;

                foreach (var segmentElement in segmentElements)
                {
                    var curveObject = curveMapper.Map(curveFeatureElement, segmentElement, document);

                    if (geomCount > 1)
                        curveObject.Ident.LokalId = $"{localId}-{index++}";

                    curveObjects.Add(curveObject);
                }
            }

            if (addNodes)
                SosiCurveObject.AddNodesToCurves(curveObjects);

            sosiElements.AddRange(curveObjects);

            LogInformation<TSosiCurveModel>(curveFeatureElements.Count, start);
        }

        public void MapSosiCurveObjects<TSosiCurveModel>(
            GmlDocument document, bool addNodes, ref int sequenceNumber, List<SosiElement> sosiElements)
            where TSosiCurveModel : SosiCurveObject, new()
        {
            var start = DateTime.Now;
            var featureMemberName = GetFeatureMemberName<TSosiCurveModel>();
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
                var localId = GetLocalId(curveFeatureElement);
                var index = 0;

                foreach (var segmentElement in segmentElements)
                {
                    var curveObject = curveMapper.Map(curveFeatureElement, segmentElement, document);

                    if (geomCount > 1)
                        curveObject.Ident.LokalId = $"{localId}-{index++}";

                    curveObjects.Add(curveObject);
                }
            }

            if (addNodes)
                SosiCurveObject.AddNodesToCurves(curveObjects);

            sosiElements.AddRange(curveObjects);

            LogInformation<TSosiCurveModel>(curveFeatureElements.Count, start);
        }

        public void MapSosiSurfaceAndCurveObjects<TSosiSurfaceModel, TSosiCurveModel>(
            GmlDocument document, double resolution, List<SosiElement> sosiElements)
            where TSosiSurfaceModel : SosiSurfaceObject
            where TSosiCurveModel : SosiCurveObject, new()
        {
            var start = DateTime.Now;
            var curveFeatureMemberName = GetFeatureMemberName<TSosiCurveModel>();
            var curveFeatureElements = document.GetFeatureElements(curveFeatureMemberName);

            if (curveFeatureElements.Any())
                MapSosiSurfaceAndCurveObjectsFromAssociations<TSosiSurfaceModel, TSosiCurveModel>(curveFeatureElements, document, resolution, sosiElements, start);
            else
                MapSosiSurfaceAndCurveObjectsFromSurfaces<TSosiSurfaceModel, TSosiCurveModel>(document, resolution, sosiElements, start);
        }

        public void MapSosiSurfaceAndCurveObjects<TSosiSurfaceModel, TSosiCurveModel>(
            GmlDocument document, double resolution, ref int sequenceNumber, List<SosiElement> sosiElements)
            where TSosiSurfaceModel : SosiSurfaceObject
            where TSosiCurveModel : SosiCurveObject, new()
        {
            var start = DateTime.Now;
            var curveFeatureMemberName = GetFeatureMemberName<TSosiCurveModel>();
            var curveFeatureElements = document.GetFeatureElements(curveFeatureMemberName);

            if (curveFeatureElements.Any())
                MapSosiSurfaceAndCurveObjectsFromAssociations<TSosiSurfaceModel, TSosiCurveModel>(curveFeatureElements, document, resolution, ref sequenceNumber, sosiElements, start);
            else
                MapSosiSurfaceAndCurveObjectsFromSurfaces<TSosiSurfaceModel, TSosiCurveModel>(document, resolution, ref sequenceNumber, sosiElements, start);
        }

        private void MapSosiSurfaceAndCurveObjectsFromAssociations<TSosiSurfaceModel, TSosiCurveModel>(
            List<XElement> featureElements, GmlDocument document, double resolution, List<SosiElement> sosiElements, DateTime start)
            where TSosiSurfaceModel : SosiSurfaceObject
            where TSosiCurveModel : SosiCurveObject, new()
        {
            var sn = 0;
            var surfaceMapper = _serviceProvider.GetRequiredService<ISosiElementMapper<TSosiSurfaceModel>>();
            var curveObjectsDict = CreateCurveObjects<TSosiCurveModel>(featureElements, document, resolution, ref sn);
            var surfaceObjects = new List<TSosiSurfaceModel>();
            var surfaceFeatureMemberName = GetFeatureMemberName<TSosiSurfaceModel>();
            var surfaceFeatureElements = document.GetFeatureElements(surfaceFeatureMemberName);

            foreach (var featureElement in surfaceFeatureElements)
            {
                var geomElement = GmlHelper.GetFeatureGeometryElements(featureElement).FirstOrDefault();
                var surfaceMemberElements = geomElement.XPath2SelectElements("*:surfaceMember/*");
                var surfaceMemberCount = surfaceMemberElements.Count();
                var surfaceObjs = new List<TSosiSurfaceModel>();
                var tempLocalId = GetLocalId(featureElement);
                var index = 0;

                foreach (var surfaceMemberElement in surfaceMemberElements)
                {
                    var localId = surfaceMemberCount == 1 ? tempLocalId : $"{tempLocalId}-{index++}";
                    var surfaceObject = surfaceMapper.Map(featureElement, document, ref sn);
                    surfaceObject.Ident.LokalId = localId;

                    surfaceObjs.Add(surfaceObject);
                }

                var xLinkElement = featureElement.XPath2SelectElements("*:avgrensesAv");

                if (!xLinkElement.Any())
                    continue;

                var segments = new List<SosiSegment>();

                foreach (var boundaryElement in xLinkElement)
                {
                    var xLink = GmlHelper.GetXLink(boundaryElement);
                    var referencedElement = document.GetElementByGmlId(xLink.GmlId);
                    var localId = GetLocalId(referencedElement);

                    if (!curveObjectsDict.TryGetValue(localId, out var crvObjects))
                        continue;

                    var curveObject = crvObjects.SingleOrDefault(crvObject => crvObject.Ident.LokalId == localId);

                    segments.Add(new SosiSegment(curveObject.Points, curveObject.SequenceNumber, curveObject.Segment.SegmentType, curveObject));
                }

                var surfaces = GeometryHelper.SegmentsToSurfaces(segments);

                for (var i = 0; i < surfaceObjs.Count; i++)
                {
                    var surface = surfaces[i];
                    var surfaceObject = surfaceObjs[i];

                    surfaceObject.SetReferences(surface);
                    surfaceObject.SetPointOnSurface(surface);

                    surfaceObjects.Add(surfaceObject);
                }
            }

            var curveObjects = curveObjectsDict.SelectMany(kvp => kvp.Value);

            sosiElements.AddRange(curveObjects);
            sosiElements.AddRange(surfaceObjects);

            LogInformation<TSosiCurveModel>(curveObjects.Count(), start);
            LogInformation<TSosiSurfaceModel>(surfaceObjects.Count, start);
        }

        private void MapSosiSurfaceAndCurveObjectsFromAssociations<TSosiSurfaceModel, TSosiCurveModel>(
            List<XElement> featureElements, GmlDocument document, double resolution, ref int sequenceNumber, List<SosiElement> sosiElements, DateTime start)
            where TSosiSurfaceModel : SosiSurfaceObject
            where TSosiCurveModel : SosiCurveObject, new()
        {
            var surfaceMapper = _serviceProvider.GetRequiredService<ISosiElementMapper<TSosiSurfaceModel>>();
            var curveObjectsDict = CreateCurveObjects<TSosiCurveModel>(featureElements, document, resolution, ref sequenceNumber);
            var surfaceObjects = new List<TSosiSurfaceModel>();
            var surfaceFeatureMemberName = GetFeatureMemberName<TSosiSurfaceModel>();
            var surfaceFeatureElements = document.GetFeatureElements(surfaceFeatureMemberName);

            foreach (var featureElement in surfaceFeatureElements)
            {
                var geomElement = GmlHelper.GetFeatureGeometryElements(featureElement).FirstOrDefault();
                var surfaceMemberElements = geomElement.XPath2SelectElements("*:surfaceMember/*");
                var surfaceMemberCount = surfaceMemberElements.Count();
                var surfaceObjs = new List<TSosiSurfaceModel>();
                var tempLocalId = GetLocalId(featureElement);
                var index = 0;

                foreach (var surfaceMemberElement in surfaceMemberElements)
                {
                    var localId = surfaceMemberCount == 1 ? tempLocalId : $"{tempLocalId}-{index++}";
                    var surfaceObject = surfaceMapper.Map(featureElement, document, ref sequenceNumber);
                    surfaceObject.Ident.LokalId = localId;

                    surfaceObjs.Add(surfaceObject);
                }

                var xLinkElement = featureElement.XPath2SelectElements("*:avgrensesAv");

                if (!xLinkElement.Any())
                    continue;

                var segments = new List<SosiSegment>();

                foreach (var boundaryElement in xLinkElement)
                {
                    var xLink = GmlHelper.GetXLink(boundaryElement);
                    var referencedElement = document.GetElementByGmlId(xLink.GmlId);
                    var localId = GetLocalId(referencedElement);

                    if (!curveObjectsDict.TryGetValue(localId, out var crvObjects))
                        continue;

                    var curveObject = crvObjects.SingleOrDefault(crvObject => crvObject.Ident.LokalId == localId);

                    segments.Add(new SosiSegment(curveObject.Points, curveObject.SequenceNumber, curveObject.Segment.SegmentType, curveObject));
                }

                var surfaces = GeometryHelper.SegmentsToSurfaces(segments);

                for (var i = 0; i < surfaceObjs.Count; i++)
                {
                    var surface = surfaces[i];
                    var surfaceObject = surfaceObjs[i];

                    surfaceObject.SetReferences(surface);
                    surfaceObject.SetPointOnSurface(surface);

                    surfaceObjects.Add(surfaceObject);
                }
            }

            var curveObjects = curveObjectsDict.SelectMany(kvp => kvp.Value);

            sosiElements.AddRange(curveObjects);
            sosiElements.AddRange(surfaceObjects);

            LogInformation<TSosiCurveModel>(curveObjects.Count(), start);
            LogInformation<TSosiSurfaceModel>(surfaceObjects.Count, start);
        }

        private void MapSosiSurfaceAndCurveObjectsFromSurfaces<TSosiSurfaceModel, TSosiCurveModel>(
            GmlDocument document, double resolution, List<SosiElement> sosiElements, DateTime start)
            where TSosiSurfaceModel : SosiSurfaceObject
            where TSosiCurveModel : SosiCurveObject, new()
        {
            var sn = 0;
            var result = CreateBoundariesFromSurfaces<TSosiSurfaceModel>(document);
            var curveObjects = new Dictionary<string, List<TSosiCurveModel>>();
            var surfaceObjects = new List<TSosiSurfaceModel>();
            var surfaceMapper = _serviceProvider.GetRequiredService<ISosiElementMapper<TSosiSurfaceModel>>();

            foreach (var grouping in result.GroupedArcs)
            {
                var firstArc = grouping.First();
                var featureElement = result.FeatureElements[firstArc.SurfaceLocalId];
                var curveObject = MapCurveObject<TSosiCurveModel>(featureElement.Feature, CartographicElementType.BueP, firstArc.LineString, document, resolution);
                var localIds = grouping.Select(arc => arc.SurfaceLocalId);

                foreach (var localId in localIds)
                {
                    if (!curveObjects.ContainsKey(localId))
                        curveObjects.Add(localId, new List<TSosiCurveModel> { curveObject });
                    else
                        curveObjects[localId].Add(curveObject);
                }
            }

            foreach (var grouping in result.GroupedDifferences)
            {
                var firstDifference = grouping.First();
                var featureElement = result.FeatureElements[firstDifference.SurfaceLocalId];
                var curveObject = MapCurveObject<TSosiCurveModel>(featureElement.Feature, CartographicElementType.Kurve, firstDifference.LineString, document, resolution);
                var localIds = grouping.Select(difference => difference.SurfaceLocalId);

                foreach (var localId in localIds)
                {
                    if (!curveObjects.ContainsKey(localId))
                        curveObjects.Add(localId, new List<TSosiCurveModel> { curveObject });
                    else
                        curveObjects[localId].Add(curveObject);
                }
            }

            foreach (var grouping in result.GroupedIntersections)
            {
                var firstIntersection = grouping.First();
                var featureElement = result.FeatureElements[firstIntersection.SurfaceLocalId];
                var curveObject = MapCurveObject<TSosiCurveModel>(featureElement.Feature, CartographicElementType.Kurve, firstIntersection.LineString, document, resolution);

                var localIds = grouping
                    .SelectMany(intersection => new[] { intersection.SurfaceLocalId, intersection.IntersectingSurfaceLocalId })
                    .Distinct()
                    .ToList();

                foreach (var localId in localIds)
                {
                    if (!curveObjects.ContainsKey(localId))
                        curveObjects.Add(localId, new List<TSosiCurveModel> { curveObject });
                    else
                        curveObjects[localId].Add(curveObject);
                }
            }

            foreach (var (localId, featureElement) in result.FeatureElements)
            {
                var surfaceObject = surfaceMapper.Map(featureElement.Feature, document, ref sn);
                surfaceObject.Ident.LokalId = localId;

                var curveObjs = curveObjects[localId];

                var clonedSegments = curveObjs
                    .Select(curveObject =>
                    {
                        var clone = curveObject.Segment.Clone();
                        clone.IsReversed = false;

                        return clone;
                    });

                var surfaces = GeometryHelper.SegmentsToSurfaces(clonedSegments);
                var firstSurface = surfaces.FirstOrDefault();

                surfaceObject.Surface = surfaces.FirstOrDefault();

                //surfaceObject.SetReferences(firstSurface);
                //surfaceObject.SetPointOnSurface(featureElement.Geometry, resolution);

                surfaceObjects.Add(surfaceObject);
            }

            var distinctCurveObjects = curveObjects
                .SelectMany(curveObject => curveObject.Value)
                .DistinctBy(curveObject => curveObject.Ident.LokalId);

            SosiCurveObject.AddNodesToCurves(distinctCurveObjects);

            sosiElements.AddRange(distinctCurveObjects);
            sosiElements.AddRange(surfaceObjects);

            LogInformation<TSosiCurveModel>(distinctCurveObjects.Count(), start);
            LogInformation<TSosiSurfaceModel>(surfaceObjects.Count, start);
        }

        private void MapSosiSurfaceAndCurveObjectsFromSurfaces<TSosiSurfaceModel, TSosiCurveModel>(
            GmlDocument document, double resolution, ref int sequenceNumber, List<SosiElement> sosiElements, DateTime start)
            where TSosiSurfaceModel : SosiSurfaceObject
            where TSosiCurveModel : SosiCurveObject, new()
        {
            var result = CreateBoundariesFromSurfaces<TSosiSurfaceModel>(document);

            var curveObjects = new Dictionary<string, List<TSosiCurveModel>>();
            var surfaceObjects = new List<TSosiSurfaceModel>();
            var surfaceMapper = _serviceProvider.GetRequiredService<ISosiElementMapper<TSosiSurfaceModel>>();

            foreach (var grouping in result.GroupedArcs)
            {
                var firstArc = grouping.First();
                var featureElement = result.FeatureElements[firstArc.SurfaceLocalId];
                var curveObject = MapCurveObject<TSosiCurveModel>(featureElement.Feature, CartographicElementType.BueP, firstArc.LineString, document, resolution, ref sequenceNumber);
                var localIds = grouping.Select(arc => arc.SurfaceLocalId);

                foreach (var localId in localIds)
                {
                    if (!curveObjects.ContainsKey(localId))
                        curveObjects.Add(localId, new List<TSosiCurveModel> { curveObject });
                    else
                        curveObjects[localId].Add(curveObject);
                }
            }

            foreach (var grouping in result.GroupedDifferences)
            {
                var firstDifference = grouping.First();
                var featureElement = result.FeatureElements[firstDifference.SurfaceLocalId];
                var curveObject = MapCurveObject<TSosiCurveModel>(featureElement.Feature, CartographicElementType.Kurve, firstDifference.LineString, document, resolution, ref sequenceNumber);
                var localIds = grouping.Select(difference => difference.SurfaceLocalId);

                foreach (var localId in localIds)
                {
                    if (!curveObjects.ContainsKey(localId))
                        curveObjects.Add(localId, new List<TSosiCurveModel> { curveObject });
                    else
                        curveObjects[localId].Add(curveObject);
                }
            }

            foreach (var grouping in result.GroupedIntersections)
            {
                var firstIntersection = grouping.First();
                var featureElement = result.FeatureElements[firstIntersection.SurfaceLocalId];
                var curveObject = MapCurveObject<TSosiCurveModel>(featureElement.Feature, CartographicElementType.Kurve, firstIntersection.LineString, document, resolution, ref sequenceNumber);

                var localIds = grouping
                    .SelectMany(intersection => new[] { intersection.SurfaceLocalId, intersection.IntersectingSurfaceLocalId })
                    .Distinct()
                    .ToList();

                foreach (var localId in localIds)
                {
                    if (!curveObjects.ContainsKey(localId))
                        curveObjects.Add(localId, new List<TSosiCurveModel> { curveObject });
                    else
                        curveObjects[localId].Add(curveObject);
                }
            }

            foreach (var (localId, featureElement) in result.FeatureElements)
            {
                var surfaceObject = surfaceMapper.Map(featureElement.Feature, document, ref sequenceNumber);
                surfaceObject.Ident.LokalId = localId;

                var curveObjs = curveObjects[localId];

                var clonedSegments = curveObjs
                    .Select(curveObject =>
                    {
                        var clone = curveObject.Segment.Clone();
                        clone.IsReversed = false;

                        return clone;
                    });

                var surfaces = GeometryHelper.SegmentsToSurfaces(clonedSegments);
                var firstSurface = surfaces.FirstOrDefault();

                //surfaceObject.SetReferences(firstSurface);
                surfaceObject.SetPointOnSurface(featureElement.Geometry, resolution);

                surfaceObjects.Add(surfaceObject);
            }

            var distinctCurveObjects = curveObjects
                .SelectMany(curveObject => curveObject.Value)
                .DistinctBy(curveObject => curveObject.SequenceNumber);

            SosiCurveObject.AddNodesToCurves(distinctCurveObjects);

            sosiElements.AddRange(distinctCurveObjects);
            sosiElements.AddRange(surfaceObjects);

            LogInformation<TSosiCurveModel>(distinctCurveObjects.Count(), start);
            LogInformation<TSosiSurfaceModel>(surfaceObjects.Count, start);
        }

        private Dictionary<string, List<TSosiCurveModel>> CreateCurveObjects<TSosiCurveModel>(
            List<XElement> featureElements, GmlDocument document, double resolution)
            where TSosiCurveModel : SosiCurveObject, new()
        {            
            var curveObjectsDict = new Dictionary<string, List<TSosiCurveModel>>();

            foreach (var featureElement in featureElements)
            {
                var geomElement = GmlHelper.GetFeatureGeometryElements(featureElement).FirstOrDefault();
                var segmentElements = geomElement.XPath2SelectElements("*:segments/*");
                var segmentElementCount = segmentElements.Count();
                var tempLocalId = GetLocalId(featureElement);
                var index = 0;

                foreach (var segmentElement in segmentElements)
                {
                    var localId = segmentElementCount == 1 ? tempLocalId : $"{tempLocalId}-{index++}";
                    var curveObject = MapCurveObject<TSosiCurveModel>(featureElement, segmentElement, document, resolution);
                    curveObject.Ident.LokalId = localId;

                    if (!curveObjectsDict.ContainsKey(tempLocalId))
                        curveObjectsDict.Add(tempLocalId, new List<TSosiCurveModel> { curveObject });
                    else
                        curveObjectsDict[tempLocalId].Add(curveObject);
                }
            }

            SosiCurveObject.AddNodesToCurves(curveObjectsDict.SelectMany(kvp => kvp.Value));

            return curveObjectsDict;
        }

        private Dictionary<string, List<TSosiCurveModel>> CreateCurveObjects<TSosiCurveModel>(
            List<XElement> featureElements, GmlDocument document, double resolution, ref int sequenceNumber)
            where TSosiCurveModel : SosiCurveObject, new()
        {
            var curveObjectsDict = new Dictionary<string, List<TSosiCurveModel>>();

            foreach (var featureElement in featureElements)
            {
                var geomElement = GmlHelper.GetFeatureGeometryElements(featureElement).FirstOrDefault();
                var segmentElements = geomElement.XPath2SelectElements("*:segments/*");
                var segmentElementCount = segmentElements.Count();
                var tempLocalId = GetLocalId(featureElement);
                var index = 0;

                foreach (var segmentElement in segmentElements)
                {
                    var localId = segmentElementCount == 1 ? tempLocalId : $"{tempLocalId}-{index++}";
                    var curveObject = MapCurveObject<TSosiCurveModel>(featureElement, segmentElement, document, resolution, ref sequenceNumber);
                    curveObject.Ident.LokalId = localId;

                    if (!curveObjectsDict.ContainsKey(tempLocalId))
                        curveObjectsDict.Add(tempLocalId, new List<TSosiCurveModel> { curveObject });
                    else
                        curveObjectsDict[tempLocalId].Add(curveObject);
                }
            }

            SosiCurveObject.AddNodesToCurves(curveObjectsDict.SelectMany(kvp => kvp.Value));

            return curveObjectsDict;
        }

        private static BoundariesFromSurfacesResult CreateBoundariesFromSurfaces<TSosiSurfaceModel>(GmlDocument document)
            where TSosiSurfaceModel : SosiSurfaceObject
        {
            var featureMemberName = GetFeatureMemberName<TSosiSurfaceModel>();
            var featureElements = document.GetFeatureElements(featureMemberName);

            var featureElementsDict = new Dictionary<string, FeatureElement>();
            var lineStringSegments = new List<LineStringSegment>();
            var arcs = new List<Arc>();

            foreach (var featureElement in featureElements)
            {
                var geomElement = GmlHelper.GetFeatureGeometryElements(featureElement).FirstOrDefault();

                if (geomElement == null)
                    continue;

                var surfaceMemberElements = geomElement.XPath2SelectElements("*:surfaceMember/*");
                var surfaceMemberCount = surfaceMemberElements.Count();
                var tempLocalId = GetLocalId(featureElement);
                var index = 0;

                foreach (var surfaceMemberElement in surfaceMemberElements)
                {
                    var segmentElements = surfaceMemberElement.XPath2SelectElements("//*:segments/*");
                    var localId = surfaceMemberCount == 1 ? tempLocalId : $"{tempLocalId}-{index++}";

                    featureElementsDict.Add(localId, new FeatureElement(featureElement, surfaceMemberElement));

                    foreach (var segmentElement in segmentElements)
                    {
                        if (segmentElement.Name.LocalName == "LineStringSegment")
                        {
                            lineStringSegments.Add(new LineStringSegment(localId, GeometryHelper.GeometryFromGml<LineString>(segmentElement)));
                        }
                        else if (segmentElement.Name.LocalName == "Arc")
                        {
                            arcs.Add(new Arc(localId, GeometryHelper.CreateLineString(segmentElement)));
                        }
                    }
                }
            }

            FindIntersections(lineStringSegments);

            var groupedArcs = arcs
                .GroupBy(arc => arc.LineString, new CustomLineStringComparer());

            var groupedDifferences = GetDifferences(lineStringSegments)
                .GroupBy(difference => difference.LineString, new CustomLineStringComparer());

            var groupedIntersections = lineStringSegments
                .SelectMany(lineStringSegment => lineStringSegment.Intersections)
                .GroupBy(intersection => intersection.LineString, new CustomLineStringComparer());

            return new BoundariesFromSurfacesResult
            {
                FeatureElements = featureElementsDict,
                GroupedArcs = groupedArcs,
                GroupedDifferences = groupedDifferences,
                GroupedIntersections = groupedIntersections
            };
        }

        private static void FindIntersections(List<LineStringSegment> lineStringSegments)
        {
            for (int i = 0; i < lineStringSegments.Count; i++)
            {
                var lineStringSegment = lineStringSegments[i];

                for (int j = i + 1; j < lineStringSegments.Count; j++)
                {
                    var otherLineStringSegment = lineStringSegments[j];

                    if (!lineStringSegment.LineString.Intersects(otherLineStringSegment.LineString))
                        continue;

                    var intersection = lineStringSegment.LineString.Intersection(otherLineStringSegment.LineString);

                    if (intersection is MultiLineString || intersection is LineString)
                    {
                        if (intersection is MultiLineString multiLineString)
                        {
                            var lineStringIntersections = MergeLineStrings(multiLineString)
                                .Select(lineString => new LineStringIntersection(lineStringSegment.SurfaceLocalId, otherLineStringSegment.SurfaceLocalId, lineString));

                            lineStringSegment.Intersections.AddRange(lineStringIntersections);
                            otherLineStringSegment.Intersections.AddRange(lineStringIntersections);
                        }
                        else if (intersection is LineString lineString)
                        {
                            var lineStringIntersection = new LineStringIntersection(lineStringSegment.SurfaceLocalId, otherLineStringSegment.SurfaceLocalId, lineString);

                            lineStringSegment.Intersections.Add(lineStringIntersection);
                            otherLineStringSegment.Intersections.Add(lineStringIntersection);
                        }
                    }
                }
            }
        }

        private static List<LineString> MergeLineStrings(Geometry multiLineString)
        {
            var lineMerger = new LineMerger();
            lineMerger.Add(multiLineString);

            return lineMerger.GetMergedLineStrings()
                .Select(geometry => (LineString)geometry)
                .ToList();
        }

        private static List<LineStringSegment> GetDifferences(List<LineStringSegment> lineStringSegments)
        {
            var differences = new List<LineStringSegment>();

            foreach (var lineStringSegment in lineStringSegments)
            {
                var intersections = lineStringSegment.Intersections.Select(intersection => intersection.LineString).ToList();

                var diffs = GetDifference(lineStringSegment.LineString, intersections)
                    .Where(lineString => !lineString.IsEmpty)
                    .Select(lineString => new LineStringSegment(lineStringSegment.SurfaceLocalId, lineString));

                differences.AddRange(diffs);
            }

            return differences;
        }

        private static List<LineString> GetDifference(LineString lineString, List<LineString> intersections)
        {
            var multiLineString = new MultiLineString(intersections.ToArray());
            var difference = lineString.Difference(multiLineString);
            var lineStrings = new List<LineString>();

            if (difference is LineString ls)
                lineStrings.Add(ls);

            if (difference is MultiLineString mls)
                for (int i = 0; i < mls.NumGeometries; i++)
                    lineStrings.Add((LineString)mls.GetGeometryN(i));

            return lineStrings;
        }
    }
}

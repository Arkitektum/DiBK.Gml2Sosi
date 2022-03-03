﻿using DiBK.Gml2Sosi.Application.Constants;
using DiBK.Gml2Sosi.Application.Helpers;
using Microsoft.AspNetCore.Http;
using System.Xml.Linq;

namespace DiBK.Gml2Sosi.Application.Models
{
    public class GmlDocument
    {
        private ILookup<string, XElement> _featureElements;
        private ILookup<string, XElement> _gmlElements;
        private ILookup<string, XElement> _geometryElements;
        public XDocument Document { get; private set; }
        public string FileName { get; private set; }

        private GmlDocument(XDocument document, string fileName)
        {
            Document = document;
            FileName = fileName;
            Initialize(document);
        }

        public List<XElement> GetFeatureElements(params string[] featureNames)
        {
            if (!featureNames.Any())
                return _featureElements.SelectMany(element => element).ToList();

            var featureElements = new List<XElement>();

            foreach (var featureName in featureNames)
                if (_featureElements.Contains(featureName))
                    featureElements.AddRange(_featureElements[featureName]);

            return featureElements;
        }

        public List<XElement> GetFeatureGeometryElements(params string[] geometryNames)
        {
            return GetGeometryElements(geometryNames, true);
        }

        public List<XElement> GetGeometryElements(params string[] geometryNames)
        {
            return GetGeometryElements(geometryNames, false);
        }

        public XElement GetElementByGmlId(string gmlId)
        {
            if (string.IsNullOrWhiteSpace(gmlId))
                return null;

            return _gmlElements[gmlId].SingleOrDefault();
        }

        private List<XElement> GetGeometryElements(IEnumerable<string> geometryNames, bool featureGeometriesOnly)
        {
            if (!geometryNames.Any())
            {
                return _geometryElements.SelectMany(element => element)
                    .Where(element => !featureGeometriesOnly || element.Parent.Name.Namespace != element.Parent.GetNamespaceOfPrefix("gml"))
                    .ToList();
            }

            var geometryElements = new List<XElement>();

            foreach (var geometryName in geometryNames)
            {
                if (!_geometryElements.Contains(geometryName))
                    continue;

                var geometryElementsOfType = _geometryElements[geometryName]
                    .Where(element => !featureGeometriesOnly || element.Parent.Name.Namespace != element.Parent.GetNamespaceOfPrefix("gml"));

                geometryElements.AddRange(geometryElementsOfType);
            }

            return geometryElements;
        }

        private void Initialize(XDocument document)
        {
            var localName = document.Root.Elements()
                .Any(element => element.Name.LocalName == "featureMember") ? "featureMember" : "featureMembers";

            _featureElements = document.Root.Elements()
                .Where(element => element.Name.LocalName == localName)
                .SelectMany(element => element.Elements())
                .ToLookup(element => element.Name.LocalName);

            _gmlElements = document.Descendants()
                .Where(element => element.Attributes(Namespace.Gml + "id").Any())
                .ToLookup(element => element.Attribute(Namespace.Gml + "id").Value);

            _geometryElements = _gmlElements
                .SelectMany(element => element)
                .Where(element => GmlHelper.GeometryElementNames.Contains(element.Name.LocalName))
                .ToLookup(element => element.Name.LocalName);
        }

        public static async Task<GmlDocument> CreateAsync(IFormFile file)
        {
            var document = await XDocument.LoadAsync(file.OpenReadStream(), LoadOptions.None, default);

            return new(document, file.FileName);
        }
    }
}

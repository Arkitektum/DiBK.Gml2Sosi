﻿using DiBK.Gml2Sosi.Application.HttpClients.Codelist;
using DiBK.Gml2Sosi.Application.Mappers;
using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Application.Models.Config;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Constants;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects;
using System.Xml.Linq;
using Wmhelp.XPath2;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers
{
    public class RpGrenseMapper : SosiCurveObjectMapper, ISosiElementMapper<RpGrense>
    {
        private readonly DatasetSettings _settings;

        public RpGrenseMapper(
            ISosiObjectTypeMapper sosiObjectTypeMapper,
            ICodelistHttpClient codelistHttpClient,
            Datasets datasets) : base(sosiObjectTypeMapper, codelistHttpClient)
        {
            _settings = datasets.GetSettings(Dataset.Reguleringsplanforslag);
        }

        public RpGrense Map(XElement featureElement, GmlDocument document)
        {
            var geomElement = featureElement.XPath2SelectElement("*:grense/*");
            var rpGrense = MapCurveObject<RpGrense>(featureElement, geomElement, document, _settings.Resolution);

            return rpGrense;
        }
    }
}

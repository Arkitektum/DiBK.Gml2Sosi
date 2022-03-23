using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;
using System.Xml.Linq;
using Wmhelp.XPath2;
using static DiBK.Gml2Sosi.Application.Helpers.MapperHelper;

namespace DiBK.Gml2Sosi.Application.Mappers
{
    public class SosiObjectTypeMapper : ISosiObjectTypeMapper
    {
        private readonly ISosiMapper<Ident> _identMapper;

        public SosiObjectTypeMapper(
            ISosiMapper<Ident> identMapper)
        {
            _identMapper = identMapper;
        }

        public TSosiModel Map<TSosiModel>(XElement featureElement, GmlDocument document) 
            where TSosiModel : SosiObjectType, new()
        {
            TSosiModel model = new();

            model.FørsteDigitaliseringsdato = FormatDate(featureElement.XPath2SelectElement("*:førsteDigitaliseringsdato"));
            model.Oppdateringsdato = FormatDate(featureElement.XPath2SelectElement("*:oppdateringsdato"));
            model.Ident = _identMapper.Map(featureElement, document);

            var idElement = featureElement.XPath2SelectElement("*:identifikasjon/*:Identifikasjon");

            model.Ident.LokalId = idElement.XPath2SelectElement("*:lokalId")?.Value;
            model.Ident.Navnerom = idElement.XPath2SelectElement("*:navnerom")?.Value;
            model.Ident.VersjonId = FormatText(idElement.XPath2SelectElement("*:versjonId"));

            return model;
        }
    }
}

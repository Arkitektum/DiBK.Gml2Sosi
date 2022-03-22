using DiBK.Gml2Sosi.Application.HttpClients.Codelist;
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
        private readonly ICodelistHttpClient _codelistHttpClient;

        public SosiObjectTypeMapper(
            ISosiMapper<Ident> identMapper,
            ICodelistHttpClient codelistHttpClient)
        {
            _identMapper = identMapper;
            _codelistHttpClient = codelistHttpClient;
        }

        public TSosiModel Map<TSosiModel>(XElement featureElement, GmlDocument document) 
            where TSosiModel : SosiObjectType, new()
        {
            TSosiModel model = new();

            model.FørsteDigitaliseringsdato = FormatDate(featureElement.XPath2SelectElement("*:førsteDigitaliseringsdato"));
            model.Oppdateringsdato = FormatDate(featureElement.XPath2SelectElement("*:oppdateringsdato"));
            model.Ident = _identMapper.Map(featureElement, document);
            model.Kvalitet = GetMålemetode(featureElement).Result;

            var idElement = featureElement.XPath2SelectElement("*:identifikasjon/*:Identifikasjon");

            model.Ident.LokalId = idElement.XPath2SelectElement("*:lokalId")?.Value;
            model.Ident.Navnerom = idElement.XPath2SelectElement("*:navnerom")?.Value;
            model.Ident.VersjonId = FormatText(idElement.XPath2SelectElement("*:versjonId"));

            return model;
        }

        private async Task<string> GetMålemetode(XElement featureElement)
        {
            var målemetodeValue = featureElement.XPath2SelectElement("*:kvalitet//*:målemetode")?.Value;            
            var målemetodeKoder = await _codelistHttpClient.GetMålemetodeKoderAsync();

            if (målemetodeKoder.Any(metode => metode.Value == målemetodeValue))
                return målemetodeValue;

            var målemetoder = await _codelistHttpClient.GetMålemetoderAsync();
            var målemetode = målemetoder.SingleOrDefault(metode => metode.Value == målemetodeValue);

            if (målemetode == null)
                return målemetodeValue;

            return målemetodeKoder.SingleOrDefault(metode => metode.Name == målemetode.Name)?.Value ?? målemetodeValue;
        }
    }
}

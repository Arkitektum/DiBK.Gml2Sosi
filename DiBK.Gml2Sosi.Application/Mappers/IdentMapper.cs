using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;
using System.Xml.Linq;
using Wmhelp.XPath2;

namespace DiBK.Gml2Sosi.Application.Mappers
{
    public class IdentMapper : ISosiMapper<Ident>
    {
        public Ident Map(XElement element, GmlDocument document)
        {
            var idElement = element.XPath2SelectElement("*:identifikasjon/*:Identifikasjon");

            return new Ident
            {
                LokalId = idElement.XPath2SelectElement("*:lokalId")?.Value,
                Navnerom = idElement.XPath2SelectElement("*:navnerom")?.Value,
                VersjonId = idElement.XPath2SelectElement("*:versjonId")?.Value,
            };
        }
    }
}

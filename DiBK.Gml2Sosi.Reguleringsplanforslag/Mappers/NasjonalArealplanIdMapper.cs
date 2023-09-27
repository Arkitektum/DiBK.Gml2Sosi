using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Constants;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects;
using System.Xml.Linq;
using Wmhelp.XPath2;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Mappers
{
    public class NasjonalArealplanIdMapper : ISosiMapper<NasjonalArealplanId>
    {
        public NasjonalArealplanId Map(XElement element, GmlDocument document)
        {
            var arealplanElement = element.Document.Root.Descendants(Namespace.App + "Arealplan").FirstOrDefault();
            var kommunenummer = arealplanElement.XPath2SelectElement("*:nasjonalArealplanId//*:kommunenummer")?.Value;
            var planId = arealplanElement.XPath2SelectElement("*:nasjonalArealplanId//*:planid")?.Value;

            return new NasjonalArealplanId
            {
                Kommunenummer = kommunenummer,
                PlanId = planId
            };
        }
    }
}

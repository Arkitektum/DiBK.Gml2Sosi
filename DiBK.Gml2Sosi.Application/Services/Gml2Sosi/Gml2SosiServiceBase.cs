using DiBK.Gml2Sosi.Application.Mappers.Interfaces;
using DiBK.Gml2Sosi.Application.Models.Config;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;
using System.Text;
using System.Xml.Linq;

namespace DiBK.Gml2Sosi.Application.Services.Gml2Sosi
{
    public abstract class Gml2SosiServiceBase
    {
        private readonly IHodeMapper _hodeMapper;

        public Gml2SosiServiceBase(
            IHodeMapper hodeMapper)
        {
            _hodeMapper = hodeMapper;
        }

        protected void MapHode(
            XDocument document, DatasetSettings datasetSettings, List<SosiElement> sosiElements)
        {
            _hodeMapper.Map(document.Document, datasetSettings, sosiElements);
        }

        protected static MemoryStream WriteToStream(List<SosiElement> sosiElements)
        {
            var memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);

            foreach (var element in sosiElements)
                element.WriteToStream(streamWriter);

            streamWriter.WriteLine(".SLUTT");
            streamWriter.Flush();
            memoryStream.Position = 0;

            return memoryStream;
        }
    }
}

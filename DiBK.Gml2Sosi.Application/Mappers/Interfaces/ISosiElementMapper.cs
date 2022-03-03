﻿using DiBK.Gml2Sosi.Application.Models;
using System.Xml.Linq;

namespace DiBK.Gml2Sosi.Application.Mappers.Interfaces
{
    public interface ISosiElementMapper<TSosiModel>
    {
        TSosiModel Map(XElement element, GmlDocument document, ref int sequenceNumber);
    }
}

using DiBK.Gml2Sosi.Application.Attributes;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects
{
    [FeatureMember("RpPåskrift")]
    public class RpPåskrift : SosiObjectType
    {
        public override string ObjType { get; } = "RpPåskrift";
        [SosiProperty("..NASJONALAREALPLANID", 5)]
        public NasjonalArealplanId NasjonalArealplanId { get; set; }
        [SosiProperty("..RPPÅSKRIFTTYPE", 6)]
        public string PåskriftType { get; set; }
        [SosiProperty("..STRENG", 7)]
        public string Tekststreng { get; set; }
        [SosiProperty("..VERTNIV", 8)]
        public string Vertikalnivå { get; set; }

        protected override void SetSosiValues()
        {
            base.SetSosiValues();

            if (!Points.Any())
                return;

            SosiValues.Add("..NØ");

            foreach (var point in Points)
                SosiValues.Add(point.ToString());
        }
    }
}

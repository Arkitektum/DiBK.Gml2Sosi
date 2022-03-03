using DiBK.Gml2Sosi.Application.Attributes;
using DiBK.Gml2Sosi.Application.Models.SosiObjects;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects
{
    [FeatureMember("RpJuridiskPunkt")]
    public class RpJuridiskPunkt : SosiObjectType
    {
        public override string ObjType { get; } = "RpJuridiskPunkt";
        [SosiProperty("..NASJONALAREALPLANID", 5)]
        public NasjonalArealplanId NasjonalArealplanId { get; set; }
        [SosiProperty("..RPJURPUNKT", 6)]
        public string JuridiskPunkt { get; set; }
        [SosiProperty("..VERTNIV", 7)]
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

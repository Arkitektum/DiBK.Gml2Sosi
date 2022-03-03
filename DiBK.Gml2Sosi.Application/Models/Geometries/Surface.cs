namespace DiBK.Gml2Sosi.Application.Models.Geometries
{
    public class Surface
    {
        public Ring Exterior { get; set; }
        public List<Ring> Interior { get; set; } = new();
    }
}

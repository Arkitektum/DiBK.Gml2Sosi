namespace DiBK.Gml2Sosi.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class SosiPropertyAttribute : Attribute
    {
        private readonly string _sosiName;
        private readonly int _order;

        public SosiPropertyAttribute(string sosiName)
        {
            _sosiName = sosiName;
        }

        public SosiPropertyAttribute(string sosiName, int order)
        {
            _sosiName = sosiName;
            _order = order;
        }

        public virtual string SosiName => _sosiName;
        public virtual int Order => _order;
    }
}

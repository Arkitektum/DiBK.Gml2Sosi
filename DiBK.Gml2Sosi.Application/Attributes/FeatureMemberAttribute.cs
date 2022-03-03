namespace DiBK.Gml2Sosi.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FeatureMemberAttribute : Attribute
    {
        private readonly string _featureMember;

        public FeatureMemberAttribute(string featureMember)
        {
            _featureMember = featureMember;
        }

        public virtual string FeatureMember => _featureMember;
    }
}

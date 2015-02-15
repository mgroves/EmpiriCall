using System.Web.Mvc;

namespace EmpiriCall
{
    public static class EmpiriCallConfig
    {
        internal static IFeatureMapper FeatureMapper { get; private set; }
        
        internal static IDependencyResolver Resolver { get; private set; }

        public static void LoadFeatureMapper(IFeatureMapper featureMapper)
        {
            FeatureMapper = featureMapper;
        }

        public static void LoadDbContainer(IDependencyResolver resolver)
        {
            Resolver = resolver;
        }
    }
}
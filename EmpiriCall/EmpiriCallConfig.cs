using System.Web.Mvc;

namespace EmpiriCall
{
    public static class EmpiriCallConfig
    {
        /// <summary>
        /// This shouldn't be used by the public
        /// </summary>
        public static IFeatureMapper FeatureMapper { get; private set; }
        
        /// <summary>
        /// This shouldn't be used by the public
        /// </summary>
        public static IDependencyResolver Resolver { get; private set; }

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
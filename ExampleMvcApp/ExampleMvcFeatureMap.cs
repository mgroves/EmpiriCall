using System;
using EmpiriCall;
using ExampleMvcApp.Controllers;

namespace ExampleMvcApp
{
    public class ExampleMvcFeatureMap : IFeatureMapper
    {
        public void Map(MapFeature map)
        {
            // in this map, I'm defining 3 features
            // Home->Index and Home->Foo correspond to the "Home" feature
            // Other->Bar corresponds to the "Other1" feature
            // Other->Baz corresponds to the "Other2" feature

            map
                .Of<HomeController>(x => x.Index(), "Home")
                .Of<HomeController>(x => x.Foo(0, null), "Home");

            map
                .Of<OtherController>(x => x.Bar(Guid.Empty), "Other1")
                .Of<OtherController>(x => x.Baz(null), "Other2");

            // note that the features are completely arbitrary
            // also note that the arguments in the above lambda are only for compilation/signature identification purposes
            // and are otherwise completely ignored
        }
    }
}
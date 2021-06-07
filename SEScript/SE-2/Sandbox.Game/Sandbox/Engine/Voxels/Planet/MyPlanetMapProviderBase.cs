using Sandbox.Definitions;
using VRage.Factory;
using VRage.Game;
using VRage.Game.ObjectBuilders;

namespace Sandbox.Engine.Voxels.Planet
{
	[MyFactorable(typeof(MyObjectFactory<MyPlanetMapProviderAttribute, MyPlanetMapProviderBase>))]
	public abstract class MyPlanetMapProviderBase
	{
		public static MyObjectFactory<MyPlanetMapProviderAttribute, MyPlanetMapProviderBase> Factory => MyObjectFactory<MyPlanetMapProviderAttribute, MyPlanetMapProviderBase>.Get();

		public abstract void Init(long seed, MyPlanetGeneratorDefinition generator, MyObjectBuilder_PlanetMapProvider builder);

		public abstract MyCubemap[] GetMaps(MyPlanetMapTypeSet types);

		public abstract MyHeightCubemap GetHeightmap();
	}
}

using VRage.Collections;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_AsteroidGeneratorDefinition), null)]
	public class MyAsteroidGeneratorDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyAsteroidGeneratorDefinition_003C_003EActor : IActivator, IActivator<MyAsteroidGeneratorDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyAsteroidGeneratorDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAsteroidGeneratorDefinition CreateInstance()
			{
				return new MyAsteroidGeneratorDefinition();
			}

			MyAsteroidGeneratorDefinition IActivator<MyAsteroidGeneratorDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public int Version;

		public int ObjectSizeMin;

		public int ObjectSizeMax;

		public int SubcellSize;

		public int SubCells;

		public int ObjectMaxInCluster;

		public int ObjectMinDistanceInCluster;

		public int ObjectMaxDistanceInClusterMin;

		public int ObjectMaxDistanceInClusterMax;

		public int ObjectSizeMinCluster;

		public int ObjectSizeMaxCluster;

		public double ObjectDensityCluster;

		public bool ClusterDispersionAbsolute;

		public bool AllowPartialClusterObjectOverlap;

		public bool UseClusterDefAsAsteroid;

		public bool RotateAsteroids;

		public bool UseLinearPowOfTwoSizeDistribution;

		public bool UseGeneratorSeed;

		public bool UseClusterVariableSize;

		public DictionaryReader<MyObjectSeedType, double> SeedTypeProbability;

		public DictionaryReader<MyObjectSeedType, double> SeedClusterTypeProbability;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_AsteroidGeneratorDefinition myObjectBuilder_AsteroidGeneratorDefinition = (MyObjectBuilder_AsteroidGeneratorDefinition)builder;
			Version = int.Parse(myObjectBuilder_AsteroidGeneratorDefinition.Id.SubtypeId);
			SubCells = myObjectBuilder_AsteroidGeneratorDefinition.SubCells;
			ObjectSizeMax = myObjectBuilder_AsteroidGeneratorDefinition.ObjectSizeMax;
			SubcellSize = 4096 + myObjectBuilder_AsteroidGeneratorDefinition.ObjectSizeMax * 2;
			ObjectSizeMin = myObjectBuilder_AsteroidGeneratorDefinition.ObjectSizeMin;
			RotateAsteroids = myObjectBuilder_AsteroidGeneratorDefinition.RotateAsteroids;
			UseGeneratorSeed = myObjectBuilder_AsteroidGeneratorDefinition.UseGeneratorSeed;
			ObjectMaxInCluster = myObjectBuilder_AsteroidGeneratorDefinition.ObjectMaxInCluster;
			ObjectDensityCluster = myObjectBuilder_AsteroidGeneratorDefinition.ObjectDensityCluster;
			ObjectSizeMaxCluster = myObjectBuilder_AsteroidGeneratorDefinition.ObjectSizeMaxCluster;
			ObjectSizeMinCluster = myObjectBuilder_AsteroidGeneratorDefinition.ObjectSizeMinCluster;
			UseClusterVariableSize = myObjectBuilder_AsteroidGeneratorDefinition.UseClusterVariableSize;
			UseClusterDefAsAsteroid = myObjectBuilder_AsteroidGeneratorDefinition.UseClusterDefAsAsteroid;
			ClusterDispersionAbsolute = myObjectBuilder_AsteroidGeneratorDefinition.ClusterDispersionAbsolute;
			ObjectMinDistanceInCluster = myObjectBuilder_AsteroidGeneratorDefinition.ObjectMinDistanceInCluster;
			ObjectMaxDistanceInClusterMax = myObjectBuilder_AsteroidGeneratorDefinition.ObjectMaxDistanceInClusterMax;
			ObjectMaxDistanceInClusterMin = myObjectBuilder_AsteroidGeneratorDefinition.ObjectMaxDistanceInClusterMin;
			AllowPartialClusterObjectOverlap = myObjectBuilder_AsteroidGeneratorDefinition.AllowPartialClusterObjectOverlap;
			UseLinearPowOfTwoSizeDistribution = myObjectBuilder_AsteroidGeneratorDefinition.UseLinearPowOfTwoSizeDistribution;
			SeedTypeProbability = myObjectBuilder_AsteroidGeneratorDefinition.SeedTypeProbability.Dictionary;
			SeedClusterTypeProbability = myObjectBuilder_AsteroidGeneratorDefinition.SeedClusterTypeProbability.Dictionary;
		}
	}
}

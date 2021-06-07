using VRage.Game;

namespace Sandbox.Definitions
{
	public struct CubeBlockEffect
	{
		public string Name;

		public string Origin;

		public float Delay;

		public bool Loop;

		public float SpawnTimeMin;

		public float SpawnTimeMax;

		public float Duration;

		public CubeBlockEffect(string Name, string Origin, float Delay, bool Loop, float SpawnTimeMin, float SpawnTimeMax, float Duration)
		{
			this.Name = Name;
			this.Origin = Origin;
			this.Delay = Delay;
			this.Loop = Loop;
			this.SpawnTimeMin = SpawnTimeMin;
			this.SpawnTimeMax = SpawnTimeMax;
			this.Duration = Duration;
		}

		public CubeBlockEffect(MyObjectBuilder_CubeBlockDefinition.CubeBlockEffect Effect)
		{
			Name = Effect.Name;
			Origin = Effect.Origin;
			Delay = Effect.Delay;
			Loop = Effect.Loop;
			SpawnTimeMin = Effect.SpawnTimeMin;
			SpawnTimeMax = Effect.SpawnTimeMax;
			Duration = Effect.Duration;
		}
	}
}

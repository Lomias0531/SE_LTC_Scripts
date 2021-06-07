using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_LaserAntennaDefinition), null)]
	public class MyLaserAntennaDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyLaserAntennaDefinition_003C_003EActor : IActivator, IActivator<MyLaserAntennaDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyLaserAntennaDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyLaserAntennaDefinition CreateInstance()
			{
				return new MyLaserAntennaDefinition();
			}

			MyLaserAntennaDefinition IActivator<MyLaserAntennaDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyStringHash ResourceSinkGroup;

		public float PowerInputIdle;

		public float PowerInputTurning;

		public float PowerInputLasing;

		public float RotationRate;

		public float MaxRange;

		public bool RequireLineOfSight;

		public int MinElevationDegrees;

		public int MaxElevationDegrees;

		public int MinAzimuthDegrees;

		public int MaxAzimuthDegrees;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_LaserAntennaDefinition myObjectBuilder_LaserAntennaDefinition = (MyObjectBuilder_LaserAntennaDefinition)builder;
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_LaserAntennaDefinition.ResourceSinkGroup);
			PowerInputIdle = myObjectBuilder_LaserAntennaDefinition.PowerInputIdle;
			PowerInputTurning = myObjectBuilder_LaserAntennaDefinition.PowerInputTurning;
			PowerInputLasing = myObjectBuilder_LaserAntennaDefinition.PowerInputLasing;
			RotationRate = myObjectBuilder_LaserAntennaDefinition.RotationRate;
			MaxRange = myObjectBuilder_LaserAntennaDefinition.MaxRange;
			RequireLineOfSight = myObjectBuilder_LaserAntennaDefinition.RequireLineOfSight;
			MinElevationDegrees = myObjectBuilder_LaserAntennaDefinition.MinElevationDegrees;
			MaxElevationDegrees = myObjectBuilder_LaserAntennaDefinition.MaxElevationDegrees;
			MinAzimuthDegrees = myObjectBuilder_LaserAntennaDefinition.MinAzimuthDegrees;
			MaxAzimuthDegrees = myObjectBuilder_LaserAntennaDefinition.MaxAzimuthDegrees;
		}
	}
}

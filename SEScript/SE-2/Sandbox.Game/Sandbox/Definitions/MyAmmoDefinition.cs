using VRage.Game;
using VRage.Game.Definitions;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_AmmoDefinition), null)]
	public abstract class MyAmmoDefinition : MyDefinitionBase
	{
		public MyAmmoType AmmoType;

		public float DesiredSpeed;

		public float SpeedVar;

		public float MaxTrajectory;

		public bool IsExplosive;

		public float BackkickForce;

		public MyStringHash PhysicalMaterial;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_AmmoDefinition myObjectBuilder_AmmoDefinition = builder as MyObjectBuilder_AmmoDefinition;
			DesiredSpeed = myObjectBuilder_AmmoDefinition.BasicProperties.DesiredSpeed;
			SpeedVar = MathHelper.Clamp(myObjectBuilder_AmmoDefinition.BasicProperties.SpeedVariance, 0f, 1f);
			MaxTrajectory = myObjectBuilder_AmmoDefinition.BasicProperties.MaxTrajectory;
			IsExplosive = myObjectBuilder_AmmoDefinition.BasicProperties.IsExplosive;
			BackkickForce = myObjectBuilder_AmmoDefinition.BasicProperties.BackkickForce;
			PhysicalMaterial = MyStringHash.GetOrCompute(myObjectBuilder_AmmoDefinition.BasicProperties.PhysicalMaterial);
		}

		public abstract float GetDamageForMechanicalObjects();
	}
}

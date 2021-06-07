using VRage.Audio;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_PrefabThrowerDefinition), null)]
	public class MyPrefabThrowerDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyPrefabThrowerDefinition_003C_003EActor : IActivator, IActivator<MyPrefabThrowerDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyPrefabThrowerDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPrefabThrowerDefinition CreateInstance()
			{
				return new MyPrefabThrowerDefinition();
			}

			MyPrefabThrowerDefinition IActivator<MyPrefabThrowerDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float? Mass;

		public float MaxSpeed;

		public float MinSpeed;

		public float PushTime;

		public string PrefabToThrow;

		public MyCueId ThrowSound;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_PrefabThrowerDefinition myObjectBuilder_PrefabThrowerDefinition = builder as MyObjectBuilder_PrefabThrowerDefinition;
			if (myObjectBuilder_PrefabThrowerDefinition.Mass.HasValue)
			{
				Mass = myObjectBuilder_PrefabThrowerDefinition.Mass;
			}
			MaxSpeed = myObjectBuilder_PrefabThrowerDefinition.MaxSpeed;
			MinSpeed = myObjectBuilder_PrefabThrowerDefinition.MinSpeed;
			PushTime = myObjectBuilder_PrefabThrowerDefinition.PushTime;
			PrefabToThrow = myObjectBuilder_PrefabThrowerDefinition.PrefabToThrow;
			ThrowSound = new MyCueId(MyStringHash.GetOrCompute(myObjectBuilder_PrefabThrowerDefinition.ThrowSound));
		}
	}
}

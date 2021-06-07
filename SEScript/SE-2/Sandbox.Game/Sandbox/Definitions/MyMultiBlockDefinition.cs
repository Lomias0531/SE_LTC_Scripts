using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_MultiBlockDefinition), null)]
	public class MyMultiBlockDefinition : MyDefinitionBase
	{
		public class MyMultiBlockPartDefinition
		{
			public MyDefinitionId Id;

			public Vector3I Min;

			public Vector3I Max;

			public Base6Directions.Direction Forward;

			public Base6Directions.Direction Up;
		}

		private class Sandbox_Definitions_MyMultiBlockDefinition_003C_003EActor : IActivator, IActivator<MyMultiBlockDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyMultiBlockDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyMultiBlockDefinition CreateInstance()
			{
				return new MyMultiBlockDefinition();
			}

			MyMultiBlockDefinition IActivator<MyMultiBlockDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyMultiBlockPartDefinition[] BlockDefinitions;

		public Vector3I Min;

		public Vector3I Max;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_MultiBlockDefinition myObjectBuilder_MultiBlockDefinition = builder as MyObjectBuilder_MultiBlockDefinition;
			if (myObjectBuilder_MultiBlockDefinition.BlockDefinitions != null && myObjectBuilder_MultiBlockDefinition.BlockDefinitions.Length != 0)
			{
				BlockDefinitions = new MyMultiBlockPartDefinition[myObjectBuilder_MultiBlockDefinition.BlockDefinitions.Length];
				for (int i = 0; i < myObjectBuilder_MultiBlockDefinition.BlockDefinitions.Length; i++)
				{
					BlockDefinitions[i] = new MyMultiBlockPartDefinition();
					MyObjectBuilder_MultiBlockDefinition.MyOBMultiBlockPartDefinition myOBMultiBlockPartDefinition = myObjectBuilder_MultiBlockDefinition.BlockDefinitions[i];
					BlockDefinitions[i].Id = myOBMultiBlockPartDefinition.Id;
					BlockDefinitions[i].Min = myOBMultiBlockPartDefinition.Position;
					BlockDefinitions[i].Forward = myOBMultiBlockPartDefinition.Orientation.Forward;
					BlockDefinitions[i].Up = myOBMultiBlockPartDefinition.Orientation.Up;
				}
			}
		}
	}
}

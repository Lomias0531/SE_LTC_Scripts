using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_DestructionDefinition), null)]
	public class MyDestructionDefinition : MyDefinitionBase
	{
		public class MyFracturedPieceDefinition
		{
			public MyDefinitionId Id;

			public int Age;
		}

		private class Sandbox_Definitions_MyDestructionDefinition_003C_003EActor : IActivator, IActivator<MyDestructionDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyDestructionDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyDestructionDefinition CreateInstance()
			{
				return new MyDestructionDefinition();
			}

			MyDestructionDefinition IActivator<MyDestructionDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float DestructionDamage;

		public new string[] Icons;

		public float ConvertedFractureIntegrityRatio;

		public MyFracturedPieceDefinition[] FracturedPieceDefinitions;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_DestructionDefinition myObjectBuilder_DestructionDefinition = builder as MyObjectBuilder_DestructionDefinition;
			DestructionDamage = myObjectBuilder_DestructionDefinition.DestructionDamage;
			Icons = myObjectBuilder_DestructionDefinition.Icons;
			ConvertedFractureIntegrityRatio = myObjectBuilder_DestructionDefinition.ConvertedFractureIntegrityRatio;
			if (myObjectBuilder_DestructionDefinition.FracturedPieceDefinitions != null && myObjectBuilder_DestructionDefinition.FracturedPieceDefinitions.Length != 0)
			{
				FracturedPieceDefinitions = new MyFracturedPieceDefinition[myObjectBuilder_DestructionDefinition.FracturedPieceDefinitions.Length];
				for (int i = 0; i < myObjectBuilder_DestructionDefinition.FracturedPieceDefinitions.Length; i++)
				{
					MyFracturedPieceDefinition myFracturedPieceDefinition = new MyFracturedPieceDefinition();
					myFracturedPieceDefinition.Id = myObjectBuilder_DestructionDefinition.FracturedPieceDefinitions[i].Id;
					myFracturedPieceDefinition.Age = myObjectBuilder_DestructionDefinition.FracturedPieceDefinitions[i].Age;
					FracturedPieceDefinitions[i] = myFracturedPieceDefinition;
				}
			}
		}

		public void Merge(MyDestructionDefinition src)
		{
			DestructionDamage = src.DestructionDamage;
			Icons = src.Icons;
			ConvertedFractureIntegrityRatio = src.ConvertedFractureIntegrityRatio;
			FracturedPieceDefinitions = src.FracturedPieceDefinitions;
		}
	}
}

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
	[MyDefinitionType(typeof(MyObjectBuilder_InventoryComponentDefinition), null)]
	public class MyInventoryComponentDefinition : MyComponentDefinitionBase
	{
		private class Sandbox_Game_EntityComponents_MyInventoryComponentDefinition_003C_003EActor : IActivator, IActivator<MyInventoryComponentDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyInventoryComponentDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyInventoryComponentDefinition CreateInstance()
			{
				return new MyInventoryComponentDefinition();
			}

			MyInventoryComponentDefinition IActivator<MyInventoryComponentDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float Volume;

		public float Mass;

		public bool RemoveEntityOnEmpty;

		public bool MultiplierEnabled;

		public int MaxItemCount;

		public MyInventoryConstraint InputConstraint;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_InventoryComponentDefinition myObjectBuilder_InventoryComponentDefinition = builder as MyObjectBuilder_InventoryComponentDefinition;
			Volume = myObjectBuilder_InventoryComponentDefinition.Volume;
			if (myObjectBuilder_InventoryComponentDefinition.Size.HasValue)
			{
				Volume = ((Vector3)myObjectBuilder_InventoryComponentDefinition.Size.Value).Volume;
			}
			Mass = myObjectBuilder_InventoryComponentDefinition.Mass;
			RemoveEntityOnEmpty = myObjectBuilder_InventoryComponentDefinition.RemoveEntityOnEmpty;
			MultiplierEnabled = myObjectBuilder_InventoryComponentDefinition.MultiplierEnabled;
			MaxItemCount = myObjectBuilder_InventoryComponentDefinition.MaxItemCount;
			if (myObjectBuilder_InventoryComponentDefinition.InputConstraint != null)
			{
				InputConstraint = new MyInventoryConstraint(MyStringId.GetOrCompute(myObjectBuilder_InventoryComponentDefinition.InputConstraint.Description), myObjectBuilder_InventoryComponentDefinition.InputConstraint.Icon, myObjectBuilder_InventoryComponentDefinition.InputConstraint.IsWhitelist);
				foreach (SerializableDefinitionId entry in myObjectBuilder_InventoryComponentDefinition.InputConstraint.Entries)
				{
					if (string.IsNullOrEmpty(entry.SubtypeName))
					{
						InputConstraint.AddObjectBuilderType(entry.TypeId);
					}
					else
					{
						InputConstraint.Add(entry);
					}
				}
			}
		}

		public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
		{
			MyObjectBuilder_InventoryComponentDefinition myObjectBuilder_InventoryComponentDefinition = base.GetObjectBuilder() as MyObjectBuilder_InventoryComponentDefinition;
			myObjectBuilder_InventoryComponentDefinition.Volume = Volume;
			myObjectBuilder_InventoryComponentDefinition.Mass = Mass;
			myObjectBuilder_InventoryComponentDefinition.RemoveEntityOnEmpty = RemoveEntityOnEmpty;
			myObjectBuilder_InventoryComponentDefinition.MultiplierEnabled = MultiplierEnabled;
			myObjectBuilder_InventoryComponentDefinition.MaxItemCount = MaxItemCount;
			if (InputConstraint != null)
			{
				myObjectBuilder_InventoryComponentDefinition.InputConstraint = new MyObjectBuilder_InventoryComponentDefinition.InventoryConstraintDefinition
				{
					IsWhitelist = InputConstraint.IsWhitelist,
					Icon = InputConstraint.Icon,
					Description = InputConstraint.Description
				};
				foreach (MyObjectBuilderType constrainedType in InputConstraint.ConstrainedTypes)
				{
					myObjectBuilder_InventoryComponentDefinition.InputConstraint.Entries.Add(new MyDefinitionId(constrainedType));
				}
				{
					foreach (MyDefinitionId constrainedId in InputConstraint.ConstrainedIds)
					{
						myObjectBuilder_InventoryComponentDefinition.InputConstraint.Entries.Add(constrainedId);
					}
					return myObjectBuilder_InventoryComponentDefinition;
				}
			}
			return myObjectBuilder_InventoryComponentDefinition;
		}
	}
}

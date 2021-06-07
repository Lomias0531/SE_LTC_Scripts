using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_PhysicalModelCollectionDefinition), null)]
	public class MyPhysicalModelCollectionDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyPhysicalModelCollectionDefinition_003C_003EActor : IActivator, IActivator<MyPhysicalModelCollectionDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyPhysicalModelCollectionDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPhysicalModelCollectionDefinition CreateInstance()
			{
				return new MyPhysicalModelCollectionDefinition();
			}

			MyPhysicalModelCollectionDefinition IActivator<MyPhysicalModelCollectionDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyDiscreteSampler<MyDefinitionId> Items;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_PhysicalModelCollectionDefinition obj = builder as MyObjectBuilder_PhysicalModelCollectionDefinition;
			List<MyDefinitionId> list = new List<MyDefinitionId>();
			List<float> list2 = new List<float>();
			MyPhysicalModelItem[] items = obj.Items;
			foreach (MyPhysicalModelItem myPhysicalModelItem in items)
			{
				Type t = MyObjectBuilderType.ParseBackwardsCompatible(myPhysicalModelItem.TypeId);
				MyDefinitionId item = new MyDefinitionId(t, myPhysicalModelItem.SubtypeId);
				list.Add(item);
				list2.Add(myPhysicalModelItem.Weight);
			}
			Items = new MyDiscreteSampler<MyDefinitionId>(list, list2);
		}
	}
}

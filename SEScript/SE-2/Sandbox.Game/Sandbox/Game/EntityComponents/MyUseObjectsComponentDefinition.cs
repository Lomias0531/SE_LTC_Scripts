using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;

namespace Sandbox.Game.EntityComponents
{
	[MyDefinitionType(typeof(MyObjectBuilder_UseObjectsComponentDefinition), null)]
	public class MyUseObjectsComponentDefinition : MyComponentDefinitionBase
	{
		private class Sandbox_Game_EntityComponents_MyUseObjectsComponentDefinition_003C_003EActor : IActivator, IActivator<MyUseObjectsComponentDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyUseObjectsComponentDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyUseObjectsComponentDefinition CreateInstance()
			{
				return new MyUseObjectsComponentDefinition();
			}

			MyUseObjectsComponentDefinition IActivator<MyUseObjectsComponentDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public bool LoadFromModel;

		public string UseObjectFromModelBBox;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_UseObjectsComponentDefinition myObjectBuilder_UseObjectsComponentDefinition = builder as MyObjectBuilder_UseObjectsComponentDefinition;
			LoadFromModel = myObjectBuilder_UseObjectsComponentDefinition.LoadFromModel;
			UseObjectFromModelBBox = myObjectBuilder_UseObjectsComponentDefinition.UseObjectFromModelBBox;
		}
	}
}

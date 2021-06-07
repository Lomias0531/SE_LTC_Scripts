using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Game.EntityComponents
{
	[MyComponentType(typeof(MyModelComponent))]
	[MyComponentBuilder(typeof(MyObjectBuilder_ModelComponent), true)]
	public class MyModelComponent : MyEntityComponentBase
	{
		private class Sandbox_Game_EntityComponents_MyModelComponent_003C_003EActor : IActivator, IActivator<MyModelComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyModelComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyModelComponent CreateInstance()
			{
				return new MyModelComponent();
			}

			MyModelComponent IActivator<MyModelComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public static MyStringHash ModelChanged = MyStringHash.GetOrCompute("ModelChanged");

		public MyModelComponentDefinition Definition
		{
			get;
			private set;
		}

		public MyModel Model
		{
			get
			{
				if (base.Entity == null)
				{
					return null;
				}
				return (base.Entity as MyEntity).Model;
			}
		}

		public MyModel ModelCollision
		{
			get
			{
				if (base.Entity == null)
				{
					return null;
				}
				return (base.Entity as MyEntity).ModelCollision;
			}
		}

		public override string ComponentTypeDebugString => string.Format("Model Component {0}", (Definition != null) ? Definition.Model : "invalid");

		public override void Init(MyComponentDefinitionBase definition)
		{
			base.Init(definition);
			Definition = (definition as MyModelComponentDefinition);
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			InitEntity();
			if (Definition != null)
			{
				this.RaiseEntityEvent(ModelChanged, new MyEntityContainerEventExtensions.ModelChangedParams(Definition.Model, Definition.Size, Definition.Mass, Definition.Volume, Definition.DisplayNameText, Definition.Icons));
			}
		}

		public void InitEntity()
		{
			if (Definition != null)
			{
				MyEntity obj = base.Entity as MyEntity;
				obj.Init(new StringBuilder(Definition.DisplayNameText), Definition.Model, null, null);
				obj.DisplayNameText = Definition.DisplayNameText;
			}
		}
	}
}

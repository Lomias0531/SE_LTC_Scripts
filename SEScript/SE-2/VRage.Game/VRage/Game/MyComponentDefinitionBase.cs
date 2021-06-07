using System.Xml.Serialization;
using VRage.Game.Definitions;
using VRage.Network;

namespace VRage.Game
{
	[MyDefinitionType(typeof(MyObjectBuilder_ComponentDefinitionBase), null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyComponentDefinitionBase : MyDefinitionBase
	{
		private class VRage_Game_MyComponentDefinitionBase_003C_003EActor : IActivator, IActivator<MyComponentDefinitionBase>
		{
			private sealed override object CreateInstance()
			{
				return new MyComponentDefinitionBase();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyComponentDefinitionBase CreateInstance()
			{
				return new MyComponentDefinitionBase();
			}

			MyComponentDefinitionBase IActivator<MyComponentDefinitionBase>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
		}

		public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
		{
			return base.GetObjectBuilder();
		}

		public override string ToString()
		{
			return $"ComponentDefinitionId={Id.TypeId}";
		}
	}
}

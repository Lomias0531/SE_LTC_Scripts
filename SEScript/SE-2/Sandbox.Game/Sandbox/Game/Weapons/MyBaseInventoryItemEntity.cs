using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Game.Components;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Weapons
{
	public class MyBaseInventoryItemEntity : MyEntity
	{
		private class Sandbox_Game_Weapons_MyBaseInventoryItemEntity_003C_003EActor : IActivator, IActivator<MyBaseInventoryItemEntity>
		{
			private sealed override object CreateInstance()
			{
				return new MyBaseInventoryItemEntity();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBaseInventoryItemEntity CreateInstance()
			{
				return new MyBaseInventoryItemEntity();
			}

			MyBaseInventoryItemEntity IActivator<MyBaseInventoryItemEntity>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyPhysicalItemDefinition m_definition;

		private float m_amount;

		public string[] IconTextures => m_definition.Icons;

		public MyBaseInventoryItemEntity()
		{
			base.Render = new MyRenderComponentInventoryItem();
		}

		public override void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
			base.Init(objectBuilder);
			m_definition = MyDefinitionManager.Static.GetPhysicalItemDefinition(objectBuilder.GetId());
			Init(null, m_definition.Model, null, null);
			base.Render.SkipIfTooSmall = false;
			base.Render.NeedsDraw = true;
			this.InitSpherePhysics(MyMaterialType.METAL, base.Model, 1f, 1f, 1f, 0, RigidBodyFlag.RBF_DEFAULT);
			base.Physics.Enabled = true;
		}
	}
}

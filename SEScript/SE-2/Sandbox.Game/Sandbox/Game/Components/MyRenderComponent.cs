using Sandbox.Game.Entities;
using VRage.Game.Components;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Components
{
	public class MyRenderComponent : MyRenderComponentBase
	{
		private class Sandbox_Game_Components_MyRenderComponent_003C_003EActor : IActivator, IActivator<MyRenderComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyRenderComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRenderComponent CreateInstance()
			{
				return new MyRenderComponent();
			}

			MyRenderComponent IActivator<MyRenderComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected MyModel m_model;

		public MyModel Model
		{
			get
			{
				return m_model;
			}
			set
			{
				m_model = value;
			}
		}

		public override object ModelStorage
		{
			get
			{
				return Model;
			}
			set
			{
				Model = (MyModel)value;
			}
		}

		public override bool NeedsDraw
		{
			get
			{
				return (base.Container.Entity.Flags & EntityFlags.NeedsDraw) != 0;
			}
			set
			{
				if (value == NeedsDraw)
				{
					return;
				}
				base.Container.Entity.Flags &= ~EntityFlags.NeedsDraw;
				if (value)
				{
					base.Container.Entity.Flags |= EntityFlags.NeedsDraw;
				}
				if (base.Container.Entity.InScene)
				{
					if (value)
					{
						MyEntities.RegisterForDraw(base.Container.Entity);
					}
					else
					{
						MyEntities.UnregisterForDraw(base.Container.Entity);
					}
				}
			}
		}

		public MyRenderComponent()
		{
			m_parentIDs = (uint[])m_parentIDs.Clone();
			m_renderObjectIDs = (uint[])m_renderObjectIDs.Clone();
		}

		public override void AddRenderObjects()
		{
			if (m_model != null && m_renderObjectIDs[0] == uint.MaxValue)
			{
				SetRenderObjectID(0, MyRenderProxy.CreateRenderEntity(base.Container.Entity.GetFriendlyName() + " " + base.Container.Entity.EntityId, m_model.AssetName, base.Container.Entity.PositionComp.WorldMatrix, MyMeshDrawTechnique.MESH, GetRenderFlags(), GetRenderCullingOptions(), m_diffuseColor, m_colorMaskHsv, Transparency, float.MaxValue, DepthBias, m_model.ScaleFactor, Transparency == 0f && FadeIn));
				if (m_textureChanges != null)
				{
					MyRenderProxy.ChangeMaterialTexture(m_renderObjectIDs[0], m_textureChanges);
				}
			}
		}

		public override void SetRenderObjectID(int index, uint ID)
		{
			m_renderObjectIDs[index] = ID;
			MyEntities.AddRenderObjectToMap(ID, base.Container.Entity);
			PropagateVisibilityUpdates();
		}

		public override void ReleaseRenderObjectID(int index)
		{
			if (m_renderObjectIDs[index] != uint.MaxValue)
			{
				MyEntities.RemoveRenderObjectFromMap(m_renderObjectIDs[index]);
				MyRenderProxy.RemoveRenderObject(m_renderObjectIDs[index], MyRenderProxy.ObjectType.Invalid, FadeOut);
				m_renderObjectIDs[index] = uint.MaxValue;
				m_parentIDs[index] = uint.MaxValue;
			}
		}

		public override void Draw()
		{
		}

		public override bool IsVisible()
		{
			if (!MyEntities.IsVisible(base.Container.Entity))
			{
				return false;
			}
			if (!base.Visible)
			{
				return false;
			}
			if (!base.Container.Entity.InScene)
			{
				return false;
			}
			return true;
		}
	}
}

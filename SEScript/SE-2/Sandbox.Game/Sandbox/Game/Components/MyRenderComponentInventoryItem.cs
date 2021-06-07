using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Graphics;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Components
{
	internal class MyRenderComponentInventoryItem : MyRenderComponent
	{
		private class Sandbox_Game_Components_MyRenderComponentInventoryItem_003C_003EActor : IActivator, IActivator<MyRenderComponentInventoryItem>
		{
			private sealed override object CreateInstance()
			{
				return new MyRenderComponentInventoryItem();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRenderComponentInventoryItem CreateInstance()
			{
				return new MyRenderComponentInventoryItem();
			}

			MyRenderComponentInventoryItem IActivator<MyRenderComponentInventoryItem>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyBaseInventoryItemEntity m_invetoryItem;

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			m_invetoryItem = (base.Container.Entity as MyBaseInventoryItemEntity);
		}

		public override void Draw()
		{
			base.Draw();
			Vector3 position = Vector3.Transform(base.Container.Entity.PositionComp.GetPosition(), MySector.MainCamera.ViewMatrix);
			Vector4 vector = Vector4.Transform(position, MySector.MainCamera.ProjectionMatrix);
			if (position.Z > 0f)
			{
				vector.X *= -1f;
				vector.Y *= -1f;
			}
			if (!(vector.W <= 0f))
			{
				Vector2 normalizedCoord = new Vector2(vector.X / vector.W / 2f + 0.5f, (0f - vector.Y) / vector.W / 2f + 0.5f);
				normalizedCoord = MyGuiManager.GetHudPixelCoordFromNormalizedCoord(normalizedCoord);
				for (int i = 0; i < m_invetoryItem.IconTextures.Length; i++)
				{
					MyGuiManager.DrawSprite(m_invetoryItem.IconTextures[i], normalizedCoord, new Rectangle(0, 0, 128, 128), Color.White, 0f, new Vector2(0.5f));
				}
			}
		}
	}
}

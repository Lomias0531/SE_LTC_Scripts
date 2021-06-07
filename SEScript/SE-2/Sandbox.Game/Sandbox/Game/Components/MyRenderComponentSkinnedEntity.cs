using Sandbox.Game.Entities;
using VRage.Network;
using VRageRender;
using VRageRender.Animations;
using VRageRender.Messages;

namespace Sandbox.Game.Components
{
	internal class MyRenderComponentSkinnedEntity : MyRenderComponent
	{
		private class Sandbox_Game_Components_MyRenderComponentSkinnedEntity_003C_003EActor : IActivator, IActivator<MyRenderComponentSkinnedEntity>
		{
			private sealed override object CreateInstance()
			{
				return new MyRenderComponentSkinnedEntity();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRenderComponentSkinnedEntity CreateInstance()
			{
				return new MyRenderComponentSkinnedEntity();
			}

			MyRenderComponentSkinnedEntity IActivator<MyRenderComponentSkinnedEntity>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private bool m_sentSkeletonMessage;

		protected MySkinnedEntity m_skinnedEntity;

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			m_skinnedEntity = (base.Container.Entity as MySkinnedEntity);
		}

		public override void AddRenderObjects()
		{
			if (m_model != null && !IsRenderObjectAssigned(0))
			{
				SetRenderObjectID(0, MyRenderProxy.CreateRenderCharacter(base.Container.Entity.DisplayName, m_model.AssetName, base.Container.Entity.PositionComp.WorldMatrix, m_diffuseColor, base.ColorMaskHsv, GetRenderFlags(), FadeIn));
				m_sentSkeletonMessage = false;
				SetVisibilityUpdates(state: true);
				UpdateCharacterSkeleton();
			}
		}

		private void UpdateCharacterSkeleton()
		{
			if (m_sentSkeletonMessage)
			{
				return;
			}
			m_sentSkeletonMessage = true;
			MyCharacterBone[] characterBones = m_skinnedEntity.AnimationController.CharacterBones;
			MySkeletonBoneDescription[] array = new MySkeletonBoneDescription[characterBones.Length];
			for (int i = 0; i < characterBones.Length; i++)
			{
				if (characterBones[i].Parent == null)
				{
					array[i].Parent = -1;
				}
				else
				{
					array[i].Parent = characterBones[i].Parent.Index;
				}
				array[i].SkinTransform = characterBones[i].SkinTransform;
			}
			MyRenderProxy.SetCharacterSkeleton(base.RenderObjectIDs[0], array, base.Model.Animations.Skeleton.ToArray());
		}

		public override void Draw()
		{
			base.Draw();
			UpdateCharacterSkeleton();
			MyRenderProxy.SetCharacterTransforms(base.RenderObjectIDs[0], m_skinnedEntity.BoneAbsoluteTransforms, m_skinnedEntity.DecalBoneUpdates);
		}
	}
}

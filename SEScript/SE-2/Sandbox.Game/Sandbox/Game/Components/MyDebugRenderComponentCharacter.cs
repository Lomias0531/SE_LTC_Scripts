using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Animations;

namespace Sandbox.Game.Components
{
	internal class MyDebugRenderComponentCharacter : MyDebugRenderComponent
	{
		private MyCharacter m_character;

		private List<Matrix> m_simulatedBonesDebugDraw = new List<Matrix>();

		private List<Matrix> m_simulatedBonesAbsoluteDebugDraw = new List<Matrix>();

		private long m_counter;

		private float m_lastDamage;

		private float m_lastCharacterVelocity;

		public MyDebugRenderComponentCharacter(MyCharacter character)
			: base(character)
		{
			m_character = character;
		}

		public override void DebugDraw()
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_MISC && m_character.CurrentWeapon != null)
			{
				MyRenderProxy.DebugDrawAxis(((MyEntity)m_character.CurrentWeapon).WorldMatrix, 1.4f, depthRead: false);
				MyRenderProxy.DebugDrawText3D(((MyEntity)m_character.CurrentWeapon).WorldMatrix.Translation, "Weapon", Color.White, 0.7f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
				MyRenderProxy.DebugDrawSphere((m_character.AnimationController.CharacterBones[m_character.WeaponBone].AbsoluteTransform * m_character.PositionComp.WorldMatrix).Translation, 0.02f, Color.White, 1f, depthRead: false);
				MyRenderProxy.DebugDrawText3D((m_character.AnimationController.CharacterBones[m_character.WeaponBone].AbsoluteTransform * m_character.PositionComp.WorldMatrix).Translation, "Weapon Bone", Color.White, 1f, depthRead: false);
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_MISC && m_character.IsUsing != null)
			{
				Matrix m = m_character.IsUsing.WorldMatrix;
				m.Translation = Vector3.Zero;
				m *= Matrix.CreateFromAxisAngle(m.Up, 3.141593f);
				Vector3 value = m_character.IsUsing.PositionComp.GetPosition() - m_character.IsUsing.WorldMatrix.Up * MyDefinitionManager.Static.GetCubeSize(MyCubeSize.Large) / 2.0;
				value = (m.Translation = value + m.Up * 0.28f - m.Forward * 0.22f);
				MyRenderProxy.DebugDrawAxis(m, 1.4f, depthRead: false);
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_SUIT_BATTERY_CAPACITY)
			{
				MatrixD worldMatrix = m_character.PositionComp.WorldMatrix;
				MyRenderProxy.DebugDrawText3D(worldMatrix.Translation + 2.0 * worldMatrix.Up, $"{m_character.SuitBattery.ResourceSource.RemainingCapacity} MWh", Color.White, 1f, depthRead: true, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			}
			m_simulatedBonesDebugDraw.Clear();
			m_simulatedBonesAbsoluteDebugDraw.Clear();
			if (!MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_BONES)
			{
				return;
			}
			m_character.AnimationController.UpdateTransformations();
			for (int i = 0; i < m_character.AnimationController.CharacterBones.Length; i++)
			{
				MyCharacterBone myCharacterBone = m_character.AnimationController.CharacterBones[i];
				if (myCharacterBone.Parent != null)
				{
					MatrixD matrix = Matrix.CreateScale(0.1f) * myCharacterBone.AbsoluteTransform * m_character.PositionComp.WorldMatrix;
					Vector3 vector2 = matrix.Translation;
					Vector3 vector3 = (myCharacterBone.Parent.AbsoluteTransform * m_character.PositionComp.WorldMatrix).Translation;
					MyRenderProxy.DebugDrawLine3D(vector3, vector2, Color.White, Color.White, depthRead: false);
					MyRenderProxy.DebugDrawText3D((vector3 + vector2) * 0.5f, myCharacterBone.Name + " (" + i + ")", Color.Red, 0.5f, depthRead: false);
					MyRenderProxy.DebugDrawAxis(matrix, 0.1f, depthRead: false);
				}
			}
		}
	}
}

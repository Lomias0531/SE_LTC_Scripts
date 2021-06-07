using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using VRage.Game.GUI;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyHudControlGravityIndicator
	{
		private readonly MyObjectBuilder_GuiTexture m_overlayTexture;

		private readonly MyObjectBuilder_GuiTexture m_fillTexture;

		private readonly MyObjectBuilder_GuiTexture m_velocityTexture;

		private Vector2 m_position;

		private Vector2 m_size;

		private Vector2 m_sizeOnScreen;

		private Vector2 m_screenPosition;

		private Vector2 m_origin;

		private Vector2 m_scale;

		private Vector2 m_screenSize;

		private Vector2 m_velocitySizeOnScreen;

		private MyGuiDrawAlignEnum m_originAlign;

		private ConditionBase m_visibleCondition;

		public MyHudControlGravityIndicator(MyObjectBuilder_GravityIndicatorVisualStyle definition)
		{
			m_position = definition.OffsetPx;
			m_size = definition.SizePx;
			m_velocitySizeOnScreen = definition.VelocitySizePx;
			m_fillTexture = MyGuiTextures.Static.GetTexture(definition.FillTexture);
			m_overlayTexture = MyGuiTextures.Static.GetTexture(definition.OverlayTexture);
			m_velocityTexture = MyGuiTextures.Static.GetTexture(definition.VelocityTexture);
			m_originAlign = definition.OriginAlign;
			m_visibleCondition = definition.VisibleCondition;
			if (definition.VisibleCondition != null)
			{
				InitStatConditions(definition.VisibleCondition);
			}
			RecalculatePosition();
		}

		private void RecalculatePosition()
		{
			float num = MyHud.HudDefinition.CustomUIScale ?? MyGuiManager.GetSafeScreenScale();
			m_sizeOnScreen = m_size * num;
			m_velocitySizeOnScreen *= num;
			m_screenSize = new Vector2(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y);
			m_screenPosition = m_position * num;
			m_screenPosition = MyUtils.AlignCoord(m_screenPosition, MySandboxGame.ScreenSize, m_originAlign);
			m_origin = m_screenPosition + m_sizeOnScreen / 2f;
		}

		public void Draw(float alpha)
		{
			if (m_visibleCondition != null && !m_visibleCondition.Eval())
			{
				return;
			}
			if (Math.Abs(MySector.MainCamera.Viewport.Width - m_screenSize.X) > 1E-05f || Math.Abs(MySector.MainCamera.Viewport.Height - m_screenSize.Y) > 1E-05f)
			{
				RecalculatePosition();
			}
			IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
			if (controlledEntity != null && controlledEntity.Entity != null)
			{
				MatrixD worldMatrix = controlledEntity.Entity.PositionComp.WorldMatrix;
				Vector3D translation = worldMatrix.Translation;
				Vector3D forward = worldMatrix.Forward;
				Vector3D right = worldMatrix.Right;
				Vector3D up = worldMatrix.Up;
				Vector3 vector = controlledEntity.Physics()?.LinearVelocity ?? Vector3.Zero;
				Vector3 vector2 = MyGravityProviderSystem.CalculateTotalGravityInPoint(translation);
				Color color = MyGuiControlBase.ApplyColorMaskModifiers(Color.White, enabled: true, alpha);
				RectangleF destination;
				if (vector2 != Vector3.Zero)
				{
					vector2.Normalize();
					double num = (forward.Dot(vector2) + 1.0) / 2.0;
					Vector2D vector2D = new Vector2D(right.Dot(vector2), up.Dot(vector2));
					double num2 = (vector2D.LengthSquared() > 9.9999997473787516E-06) ? Math.Atan2(vector2D.Y, vector2D.X) : 0.0;
					num2 += Math.PI;
					int num3 = (int)((double)m_sizeOnScreen.Y * num);
					destination = new RectangleF(m_screenPosition.X, m_screenPosition.Y + m_sizeOnScreen.Y - (float)num3, m_sizeOnScreen.X, num3);
					int num4 = (int)((double)m_fillTexture.SizePx.Y * num);
					Rectangle? sourceRectangle = new Rectangle(0, m_fillTexture.SizePx.Y - num4, m_fillTexture.SizePx.X, num4);
					Vector2 rightVector = new Vector2((float)Math.Sin(num2), (float)Math.Cos(num2));
					MyRenderProxy.DrawSpriteExt(m_fillTexture.Path, ref destination, sourceRectangle, color, ref rightVector, ref m_origin);
				}
				if (vector != Vector3.Zero && m_velocityTexture != null)
				{
					Vector2 value = new Vector2((float)right.Dot(vector), 0f - (float)up.Dot(vector));
					float transitionAlpha = Math.Min(MyMath.Clamp(vector.Length() / (MyGridPhysics.ShipMaxLinearVelocity() + 7f) / 0.05f, 0f, 1f), alpha);
					Vector2 position = m_screenPosition + m_sizeOnScreen * 0.5f - m_velocitySizeOnScreen * 0.5f;
					float num5 = value.Length();
					float num6 = MyMath.Clamp(1f - (float)Math.Exp((0f - num5) * 0.01f), 0f, 1f);
					value *= 1f / num5 * num6;
					position += value * (m_sizeOnScreen / 2f);
					Rectangle? sourceRectangle = null;
					destination = new RectangleF(position, m_velocitySizeOnScreen);
					MyRenderProxy.DrawSpriteExt(m_velocityTexture.Path, ref destination, sourceRectangle, MyGuiControlBase.ApplyColorMaskModifiers(Color.White, enabled: true, transitionAlpha), ref Vector2.UnitX, ref m_origin);
				}
				destination = new RectangleF(m_screenPosition, m_sizeOnScreen);
				MyRenderProxy.DrawSpriteExt(m_overlayTexture.Path, ref destination, null, color, ref Vector2.UnitX, ref m_origin);
			}
		}

		private void InitStatConditions(ConditionBase conditionBase)
		{
			StatCondition statCondition = conditionBase as StatCondition;
			if (statCondition != null)
			{
				IMyHudStat stat = MyHud.Stats.GetStat(statCondition.StatId);
				statCondition.SetStat(stat);
				return;
			}
			Condition condition = conditionBase as Condition;
			if (condition != null)
			{
				ConditionBase[] terms = condition.Terms;
				foreach (ConditionBase conditionBase2 in terms)
				{
					InitStatConditions(conditionBase2);
				}
			}
		}
	}
}

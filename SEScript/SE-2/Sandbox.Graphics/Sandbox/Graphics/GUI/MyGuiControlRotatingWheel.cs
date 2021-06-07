using VRage.Library;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiControlRotatingWheel : MyGuiControlBase
	{
		private float m_rotationSpeed;

		private float m_rotatingAngle;

		private float m_wheelScale;

		private string m_texture;

		private Vector2 m_textureSize;

		public bool MultipleSpinningWheels;

		public bool ManualRotationUpdate;

		public MyGuiControlRotatingWheel(Vector2? position = null, Vector4? colorMask = null, float scale = 0.36f, MyGuiDrawAlignEnum align = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, string texture = "Textures\\GUI\\screens\\screen_loading_wheel.dds", bool manualRotationUpdate = true, bool multipleSpinningWheels = true, Vector2? textureResolution = null, float radiansPerSecond = 1.5f)
			: base(position, null, colorMask, null, null, isActiveControl: false, canHaveFocus: false, MyGuiControlHighlightType.WHEN_ACTIVE, align)
		{
			UpdateRotation();
			m_wheelScale = scale;
			m_texture = texture;
			m_textureSize = MyRenderProxy.GetTextureSize(m_texture);
			MultipleSpinningWheels = multipleSpinningWheels;
			ManualRotationUpdate = manualRotationUpdate;
			m_rotationSpeed = radiansPerSecond;
		}

		public override void Update()
		{
			if (ManualRotationUpdate && base.Visible)
			{
				UpdateRotation();
			}
			base.Update();
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
			Vector2 positionAbsolute = GetPositionAbsolute();
			Color color = new Color(transitionAlpha * new Color(0, 0, 0, 80).ToVector4());
			DrawWheel(positionAbsolute + MyGuiConstants.SHADOW_OFFSET, m_wheelScale, color, m_rotatingAngle, m_rotationSpeed);
			Color color2 = MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha);
			DrawWheel(positionAbsolute, m_wheelScale, color2, m_rotatingAngle, m_rotationSpeed);
			if (MultipleSpinningWheels)
			{
				DrawWheel(positionAbsolute, 0.6f * m_wheelScale, color2, (0f - m_rotatingAngle) * 1.1f, 0f - m_rotationSpeed);
				DrawWheel(positionAbsolute, 0.36f * m_wheelScale, color2, m_rotatingAngle * 1.2f, m_rotationSpeed);
			}
		}

		private void UpdateRotation()
		{
			m_rotatingAngle = (float)MyEnvironment.TickCount / 1000f * m_rotationSpeed;
		}

		private void DrawWheel(Vector2 position, float scale, Color color, float rotationAngle, float rotationSpeed)
		{
			Vector2 normalizedSize = MyGuiManager.GetNormalizedSize(m_textureSize, scale);
			MyGuiManager.DrawSpriteBatch(m_texture, position, normalizedSize, color, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, useFullClientArea: false, waitTillLoaded: false, null, rotationAngle, ManualRotationUpdate ? 0f : rotationSpeed);
		}
	}
}

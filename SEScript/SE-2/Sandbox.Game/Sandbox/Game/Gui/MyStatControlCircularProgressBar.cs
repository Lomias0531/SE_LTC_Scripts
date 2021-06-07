using Sandbox.Graphics.GUI;
using System;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GUI
{
	public class MyStatControlCircularProgressBar : MyStatControlBase
	{
		private static MyGameTimer TIMER = new MyGameTimer();

		private float m_progression;

		private Vector2 m_segmentOrigin;

		private Vector2 m_segmentSize;

		private Vector2 m_invScale;

		private int m_animatedSegmentIndex;

		private double m_animationTimeSwitchedSegment;

		private double m_animationTimeStarted;

		private bool m_animating;

		private readonly MyGuiSizedTexture m_backgroundTexture;

		private readonly MyGuiSizedTexture m_texture;

		private readonly MyGuiSizedTexture? m_firstTexture;

		private readonly MyGuiSizedTexture? m_lastTexture;

		private float m_textureRotationAngle;

		private float m_textureRotationOffset;

		public int NumberOfSegments
		{
			get;
			set;
		}

		public bool Animate
		{
			get;
			set;
		}

		public double SegmentAnimationMs
		{
			get;
			set;
		}

		public double AnimationDelay
		{
			get;
			set;
		}

		public bool ShowEmptySegments
		{
			get;
			set;
		}

		public Vector4 EmptySegmentColorMask
		{
			get;
			set;
		}

		public Vector4 FullSegmentColorMask
		{
			get;
			set;
		}

		public Vector4 AnimatedSegmentColorMask
		{
			get;
			set;
		}

		public float TextureRotationAngle
		{
			get
			{
				return MathHelper.ToDegrees(m_textureRotationAngle);
			}
			set
			{
				m_textureRotationAngle = MathHelper.ToRadians(value);
			}
		}

		public float TextureRotationOffset
		{
			get
			{
				return MathHelper.ToDegrees(m_textureRotationOffset);
			}
			set
			{
				m_textureRotationOffset = MathHelper.ToRadians(value);
			}
		}

		public Vector2 SegmentSize
		{
			get
			{
				return m_segmentSize;
			}
			set
			{
				m_segmentSize = value;
				RecalcScale();
			}
		}

		public Vector2 SegmentOrigin
		{
			get
			{
				return m_segmentOrigin;
			}
			set
			{
				m_segmentOrigin = value;
				RecalcScale();
			}
		}

		public MyStatControlCircularProgressBar(MyStatControls parent, MyObjectBuilder_GuiTexture texture, MyObjectBuilder_GuiTexture backgroundTexture = null, MyObjectBuilder_GuiTexture firstTexture = null, MyObjectBuilder_GuiTexture lastTexture = null)
			: base(parent)
		{
			MyGuiSizedTexture myGuiSizedTexture;
			if (backgroundTexture != null)
			{
				myGuiSizedTexture = new MyGuiSizedTexture
				{
					Texture = backgroundTexture.Path,
					SizePx = backgroundTexture.SizePx
				};
				m_backgroundTexture = myGuiSizedTexture;
			}
			myGuiSizedTexture = (m_texture = new MyGuiSizedTexture
			{
				Texture = texture.Path,
				SizePx = texture.SizePx
			});
			if (firstTexture != null)
			{
				myGuiSizedTexture = new MyGuiSizedTexture
				{
					Texture = firstTexture.Path,
					SizePx = firstTexture.SizePx
				};
				m_firstTexture = myGuiSizedTexture;
			}
			if (lastTexture != null)
			{
				myGuiSizedTexture = new MyGuiSizedTexture
				{
					Texture = lastTexture.Path,
					SizePx = lastTexture.SizePx
				};
				m_lastTexture = myGuiSizedTexture;
			}
			ShowEmptySegments = true;
			EmptySegmentColorMask = new Vector4(1f, 1f, 1f, 0.5f);
			FullSegmentColorMask = Vector4.One;
			AnimatedSegmentColorMask = new Vector4(1f, 1f, 1f, 0.8f);
			NumberOfSegments = 10;
			AnimationDelay = 2000.0;
			SegmentAnimationMs = 50.0;
			m_textureRotationAngle = 0.36f;
			m_segmentOrigin = new Vector2(m_texture.SizePx.X / 2f, m_texture.SizePx.Y / 2f);
		}

		private void RecalcScale()
		{
			m_invScale = 1f / (m_segmentSize / m_texture.SizePx);
		}

		public override void Draw(float transitionAlpha)
		{
			Vector4 value = Vector4.One;
			base.BlinkBehavior.UpdateBlink();
			if (base.BlinkBehavior.Blink)
			{
				transitionAlpha = base.BlinkBehavior.CurrentBlinkAlpha;
				if (base.BlinkBehavior.ColorMask.HasValue)
				{
					value = MyGuiControlBase.ApplyColorMaskModifiers(base.BlinkBehavior.ColorMask.Value, enabled: true, transitionAlpha);
				}
			}
			float num = base.StatCurrent / base.StatMaxValue;
			double num2 = 0.0;
			if (Animate)
			{
				num2 = TIMER.Elapsed.Milliseconds;
				if (!m_animating && num2 - m_animationTimeStarted > AnimationDelay)
				{
					m_animating = true;
					m_animationTimeStarted = num2;
					m_animatedSegmentIndex = 0;
					m_animationTimeSwitchedSegment = num2;
				}
			}
			Rectangle? sourceRectangle = null;
			RectangleF rectangleF = default(RectangleF);
			rectangleF.Position = base.Position + new Vector2(0f - SegmentOrigin.X, SegmentOrigin.Y);
			rectangleF.Size = SegmentSize;
			RectangleF destination = rectangleF;
			float num3 = 1f / (float)NumberOfSegments;
			for (int i = 0; i < NumberOfSegments; i++)
			{
				Vector2 rightVector = new Vector2((float)Math.Cos(m_textureRotationAngle * (float)i + m_textureRotationOffset), (float)Math.Sin(m_textureRotationAngle * (float)i + m_textureRotationOffset));
				Vector4 v = EmptySegmentColorMask * value;
				sourceRectangle = null;
				destination.Position = base.Position + new Vector2(0f - SegmentOrigin.X, SegmentOrigin.Y);
				destination.Size = SegmentSize;
				Vector2 origin = base.Position + base.Size / 2f;
				MyGuiSizedTexture myGuiSizedTexture = m_texture;
				if (i == 0 && m_firstTexture.HasValue)
				{
					myGuiSizedTexture = m_firstTexture.Value;
				}
				else if (i == NumberOfSegments - 1 && m_lastTexture.HasValue)
				{
					myGuiSizedTexture = m_lastTexture.Value;
				}
				if (ShowEmptySegments)
				{
					MyRenderProxy.DrawSpriteExt(myGuiSizedTexture.Texture, ref destination, sourceRectangle, v, ref rightVector, ref origin);
				}
				bool flag = true;
				if (m_animating && m_animatedSegmentIndex == i)
				{
					v = AnimatedSegmentColorMask * value;
					if (num2 - m_animationTimeSwitchedSegment > SegmentAnimationMs)
					{
						m_animationTimeSwitchedSegment = num2;
						m_animatedSegmentIndex++;
					}
					flag = false;
				}
				else if ((float)i < num * (float)NumberOfSegments)
				{
					if (num / ((float)(i + 1) * num3) < 1f)
					{
						float num4 = num % num3 * (float)NumberOfSegments;
						float num5 = 1f - num4;
						sourceRectangle = new Rectangle(0, (int)(num5 * m_texture.SizePx.Y), (int)m_texture.SizePx.X, (int)(num4 * m_texture.SizePx.Y));
						destination.Size = SegmentSize * new Vector2(1f, num4);
						destination.Position = base.Position + new Vector2(0f - SegmentOrigin.X, SegmentOrigin.Y + SegmentSize.Y * num5);
					}
					v = FullSegmentColorMask * value;
					flag = false;
				}
				if (m_animatedSegmentIndex >= NumberOfSegments)
				{
					m_animating = false;
				}
				if (!flag)
				{
					MyRenderProxy.DrawSpriteExt(myGuiSizedTexture.Texture, ref destination, sourceRectangle, v, ref rightVector, ref origin);
				}
			}
			if (!string.IsNullOrEmpty(m_backgroundTexture.Texture))
			{
				destination = new RectangleF(base.Position - base.Size / 2f, base.Size);
				MyRenderProxy.DrawSprite(m_texture.Texture, ref destination, sourceRectangle, Color.White, 0f);
			}
		}
	}
}

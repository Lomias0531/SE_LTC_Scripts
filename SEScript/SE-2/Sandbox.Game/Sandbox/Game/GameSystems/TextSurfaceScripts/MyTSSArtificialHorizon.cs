using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Localization;
using Sandbox.Graphics;
using Sandbox.ModAPI.Ingame;
using System;
using System.Text;
using VRage;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace Sandbox.Game.GameSystems.TextSurfaceScripts
{
	[MyTextSurfaceScript("TSS_ArtificialHorizon", "DisplayName_TSS_ArtificialHorizon")]
	public class MyTSSArtificialHorizon : MyTSSCommon
	{
		private const int HUD_SCALING = 1200;

		private const double PLANET_GRAVITY_THRESHOLD_SQ = 0.0025;

		private const float LADDER_TEXT_SIZE_MULTIPLIER = 0.7f;

		private const int ALTITUDE_WARNING_TIME_THRESHOLD = 24;

		private const int RADAR_ALTITUDE_THRESHOLD = 500;

		private static readonly float ANGLE_STEP = 0.0872664452f;

		private MyCubeGrid m_grid;

		private MatrixD m_ownerTransform;

		private Vector2 m_innerSize;

		private float m_maxScale;

		private float m_screenDiag;

		private int m_tickCounter;

		private int m_lastRadarAlt;

		private double m_lastSeaLevelAlt;

		private bool m_showAltWarning;

		private int m_altWarningShownAt;

		private readonly Vector2 m_textBoxSize;

		private readonly Vector2 m_textOffsetInsideBox;

		private readonly Vector2 m_ladderStepSize;

		private readonly Vector2 m_ladderStepTextOffset;

		private MyPlanet m_nearestPlanet;

		public override ScriptUpdate NeedsUpdate => ScriptUpdate.Update10;

		public MyTSSArtificialHorizon(IMyTextSurface surface, IMyCubeBlock block, Vector2 size)
			: base(surface, block, size)
		{
			if (m_block != null)
			{
				m_grid = (m_block.CubeGrid as MyCubeGrid);
			}
			m_maxScale = Math.Min(m_scale.X, m_scale.Y);
			m_innerSize = new Vector2(1.2f, 1f);
			MyTextSurfaceScriptBase.FitRect(size, ref m_innerSize);
			m_screenDiag = (float)Math.Sqrt(m_innerSize.X * m_innerSize.X + m_innerSize.Y * m_innerSize.Y);
			m_fontScale = 1f * m_maxScale;
			m_fontId = "White";
			m_ownerTransform = m_grid.PositionComp.WorldMatrix;
			m_ownerTransform.Translation = m_block.GetPosition();
			m_nearestPlanet = MyGamePruningStructure.GetClosestPlanet(m_ownerTransform.Translation);
			m_textBoxSize = new Vector2(89f, 32f) * m_maxScale;
			m_textOffsetInsideBox = new Vector2(5f, 0f) * m_maxScale;
			m_ladderStepSize = new Vector2(150f, 31f) * m_maxScale;
			m_ladderStepTextOffset = new Vector2(0f, m_ladderStepSize.Y * 0.5f);
		}

		public override void Run()
		{
			base.Run();
			using (MySpriteDrawFrame frame = m_surface.DrawFrame())
			{
				if (m_grid != null && m_grid.Physics != null)
				{
					m_block.Orientation.GetMatrix(out Matrix result);
					m_ownerTransform = result * m_grid.PositionComp.WorldMatrix;
					m_ownerTransform.Translation = m_block.GetPosition();
					m_ownerTransform.Orthogonalize();
					Vector3 gravity = MyGravityProviderSystem.CalculateNaturalGravityInPoint(m_ownerTransform.Translation);
					if ((double)gravity.LengthSquared() >= 0.0025)
					{
						DrawPlanetDisplay(frame, gravity, m_ownerTransform);
					}
					else
					{
						DrawSpaceDisplay(frame, m_ownerTransform);
					}
					m_tickCounter++;
				}
			}
		}

		private void DrawPlanetDisplay(MySpriteDrawFrame frame, Vector3 gravity, MatrixD worldTrans)
		{
			gravity.Normalize();
			Vector3D vector3D = Vector3D.Reject(worldTrans.Forward, gravity);
			vector3D.Normalize();
			Vector3D normal = Vector3D.Reject(vector3D, worldTrans.Forward);
			normal = Vector3D.TransformNormal(normal, MatrixD.Invert(worldTrans));
			Vector2 screenForward2D = new Vector2((float)normal.X, 0f - (float)normal.Y) * 1200f * m_maxScale;
			double num = 0.0 - (Math.Acos(Vector3.Dot(Vector3D.Normalize(Vector3D.Reject(gravity, worldTrans.Forward)), worldTrans.Left)) - 1.570796012878418);
			if (gravity.Dot(worldTrans.Up) >= 0f)
			{
				num = Math.PI - num;
			}
			double pitchAngle = Math.Acos(gravity.Dot(worldTrans.Forward)) - 1.570796012878418;
			DrawHorizon(frame, screenForward2D, num);
			DrawLadder(frame, gravity, worldTrans, pitchAngle, vector3D, num);
			if (m_tickCounter % 100 == 0)
			{
				m_nearestPlanet = MyGamePruningStructure.GetClosestPlanet(worldTrans.Translation);
			}
			if (m_nearestPlanet != null)
			{
				int num2 = DrawAltitudeWarning(frame, worldTrans, m_nearestPlanet);
				m_lastSeaLevelAlt = DrawAltimeter(frame, worldTrans, m_nearestPlanet, num2, m_textBoxSize);
				m_lastRadarAlt = num2;
			}
			Vector3 linearVelocity = m_grid.Physics.LinearVelocity;
			DrawPullUpWarning(frame, linearVelocity, worldTrans, num);
			Vector2 drawPos = m_halfSize + new Vector2(-205f, 80f) * m_maxScale;
			frame = DrawSpeedIndicator(frame, drawPos, m_textBoxSize, linearVelocity);
			DrawVelocityVector(frame, linearVelocity, worldTrans);
			DrawBoreSight(frame);
		}

		private void DrawHorizon(MySpriteDrawFrame frame, Vector2 screenForward2D, double rollAngle)
		{
			Vector2 value = new Vector2(m_screenDiag);
			Vector2 value2 = new Vector2(0f, m_screenDiag * 0.5f);
			value2.Rotate(rollAngle);
			MySprite sprite = new MySprite(SpriteType.TEXTURE, "Grid", m_halfSize + value2 + screenForward2D, value, new Color(m_foregroundColor, 0.5f), null, TextAlignment.CENTER, (float)rollAngle);
			frame.Add(sprite);
			sprite.Position = m_halfSize - value2 + screenForward2D;
			frame.Add(sprite);
			value2 = new Vector2(0f, m_screenDiag * 1.5f);
			value2.Rotate(rollAngle);
			sprite.Position = m_halfSize + value2 + screenForward2D;
			frame.Add(sprite);
			sprite.Position = m_halfSize - value2 + screenForward2D;
			frame.Add(sprite);
			MySprite sprite2 = new MySprite(SpriteType.TEXTURE, "SquareTapered", m_halfSize + screenForward2D, new Vector2(m_screenDiag, 3f * m_maxScale), m_foregroundColor, null, TextAlignment.CENTER, (float)rollAngle);
			frame.Add(sprite2);
		}

		private void DrawLadder(MySpriteDrawFrame frame, Vector3 gravity, MatrixD worldTrans, double pitchAngle, Vector3D horizonForward, double rollAngle)
		{
			int num = (int)Math.Round(pitchAngle / (double)ANGLE_STEP);
			for (int i = num - 5; i <= num + 5; i++)
			{
				if (i != 0)
				{
					Vector3D normal = Vector3D.Reject((MatrixD.CreateRotationX((float)i * ANGLE_STEP) * MatrixD.CreateWorld(worldTrans.Translation, horizonForward, -(Vector3D)gravity)).Forward, worldTrans.Forward);
					normal = Vector3D.TransformNormal(normal, MatrixD.Invert(worldTrans));
					Vector2 value = new Vector2((float)normal.X, 0f - (float)normal.Y) * 1200f * m_maxScale;
					string data = ((float)i * ANGLE_STEP < 0f) ? "AH_GravityHudNegativeDegrees" : "AH_GravityHudPositiveDegrees";
					MySprite sprite = new MySprite(SpriteType.TEXTURE, data, m_halfSize + value, m_ladderStepSize, m_foregroundColor, null, TextAlignment.CENTER, (float)rollAngle);
					frame.Add(sprite);
					float scale = m_fontScale * 0.7f;
					int num2 = Math.Abs(i * 5);
					string text = (i > 18) ? (180 - i * 5).ToString() : num2.ToString();
					Vector2 value2 = new Vector2((0f - m_ladderStepSize.X) * 0.55f, 0f);
					value2.Rotate(rollAngle);
					MySprite sprite2 = MySprite.CreateText(text, m_fontId, m_foregroundColor, scale, TextAlignment.RIGHT);
					sprite2.Position = m_halfSize + value + value2 - m_ladderStepTextOffset;
					frame.Add(sprite2);
					value2 = new Vector2(m_ladderStepSize.X * 0.55f, 0f);
					value2.Rotate(rollAngle);
					MySprite sprite3 = MySprite.CreateText(text, m_fontId, m_foregroundColor, scale, TextAlignment.LEFT);
					sprite3.Position = m_halfSize + value + value2 - m_ladderStepTextOffset;
					frame.Add(sprite3);
				}
			}
		}

		private int DrawAltitudeWarning(MySpriteDrawFrame frame, MatrixD worldTrans, MyPlanet nearestPlanet)
		{
			float height = m_grid.PositionComp.LocalAABB.Height;
			float num = 100f + height;
			int num2 = (int)Vector3D.Distance(nearestPlanet.GetClosestSurfacePointGlobal(worldTrans.Translation), worldTrans.Translation);
			if ((float)m_lastRadarAlt >= num && (float)num2 < num)
			{
				m_showAltWarning = true;
				m_altWarningShownAt = m_tickCounter;
			}
			if (m_tickCounter - m_altWarningShownAt > 24)
			{
				m_showAltWarning = false;
			}
			if (m_showAltWarning)
			{
				StringBuilder stringBuilder = MyTexts.Get(MySpaceTexts.DisplayName_TSS_ArtificialHorizon_AltitudeWarning);
				Vector2 value = MyGuiManager.MeasureStringRaw(m_fontId, stringBuilder, m_fontScale);
				MySprite sprite = MySprite.CreateText(stringBuilder.ToString(), m_fontId, m_foregroundColor, m_fontScale, TextAlignment.LEFT);
				sprite.Position = m_halfSize + new Vector2(0f, 100f) - value * 0.5f;
				frame.Add(sprite);
			}
			return num2;
		}

		private double DrawAltimeter(MySpriteDrawFrame frame, MatrixD worldTrans, MyPlanet nearestPlanet, int radarAltitude, Vector2 textBoxSize)
		{
			double num = Vector3D.Distance(nearestPlanet.PositionComp.GetPosition(), worldTrans.Translation);
			num -= (double)nearestPlanet.AverageRadius;
			string text = (radarAltitude < 500) ? radarAltitude.ToString() : ((int)num).ToString();
			Vector2 value = m_halfSize + new Vector2(115f, 80f) * m_maxScale;
			AddTextBox(frame, value + textBoxSize * 0.5f, textBoxSize, text, m_fontId, m_fontScale, m_foregroundColor, m_foregroundColor, "AH_TextBox", m_textOffsetInsideBox.X);
			if (radarAltitude < 500)
			{
				MySprite sprite = MySprite.CreateText("R", m_fontId, m_foregroundColor, m_fontScale, TextAlignment.LEFT);
				Vector2 value2 = value + textBoxSize * 0.5f;
				sprite.Position = value2 + new Vector2(textBoxSize.X, 0f - textBoxSize.Y) * 0.5f + m_textOffsetInsideBox;
				frame.Add(sprite);
			}
			double num2 = (num - m_lastSeaLevelAlt) * 6.0;
			AddTextBox(frame, value + new Vector2(textBoxSize.X * 0.5f, (0f - textBoxSize.Y) * 0.5f), textBoxSize, ((int)num2).ToString(), m_fontId, m_fontScale, m_foregroundColor, m_foregroundColor, null, m_textOffsetInsideBox.X);
			return num;
		}

		private void DrawPullUpWarning(MySpriteDrawFrame frame, Vector3 velocity, MatrixD worldTrans, double rollAngle)
		{
			Vector3 value = m_grid.Mass / 16000f * velocity;
			if (MyPhysics.CastRay(worldTrans.Translation, worldTrans.Translation + value, 14).HasValue && m_tickCounter >= 0 && m_tickCounter % 10 > 2)
			{
				MySprite sprite = new MySprite(SpriteType.TEXTURE, "AH_PullUp", m_halfSize, new Vector2(150f, 180f), m_foregroundColor, null, TextAlignment.CENTER, (float)rollAngle);
				frame.Add(sprite);
			}
		}

		private void DrawVelocityVector(MySpriteDrawFrame frame, Vector3 velocity, MatrixD worldTrans)
		{
			if (Vector3.Dot(velocity, worldTrans.Forward) >= -0.1f)
			{
				float num = velocity.LengthSquared();
				velocity.Normalize();
				Vector3D normal = Vector3D.Reject(velocity, worldTrans.Forward);
				normal = Vector3D.TransformNormal(normal, MatrixD.Invert(worldTrans));
				Vector2 value = new Vector2((float)normal.X, 0f - (float)normal.Y) * 1200f * m_maxScale;
				if (num < 9f)
				{
					value = new Vector2(0f, 0f);
				}
				MySprite sprite = new MySprite(SpriteType.TEXTURE, "AH_VelocityVector", m_halfSize + value, new Vector2(50f, 50f) * m_maxScale, m_foregroundColor);
				frame.Add(sprite);
			}
		}

		private void DrawBoreSight(MySpriteDrawFrame frame)
		{
			MySprite sprite = new MySprite(SpriteType.TEXTURE, "AH_BoreSight", m_size * 0.5f + new Vector2(0f, 19f) * m_maxScale, new Vector2(50f, 50f) * m_maxScale, m_foregroundColor, null, TextAlignment.CENTER, -MathF.PI / 2f);
			frame.Add(sprite);
		}

		private void DrawSpaceDisplay(MySpriteDrawFrame frame, MatrixD worldTrans)
		{
			AddBackground(frame, new Color(m_foregroundColor, 0.66f));
			Vector3 linearVelocity = m_grid.Physics.LinearVelocity;
			DrawVelocityVector(frame, linearVelocity, worldTrans);
			DrawBoreSight(frame);
			Vector2 vector = m_halfSize + new Vector2(-205f, 80f) * m_maxScale;
			Vector2 vector2 = m_halfSize + new Vector2(205f, 80f) * m_maxScale;
			DrawSpeedIndicator(frame, vector, m_textBoxSize, linearVelocity);
			Color barBgColor = new Color(m_foregroundColor, 0.1f);
			float num = Math.Max(MyGridPhysics.ShipMaxLinearVelocity(), 1f);
			float ratio = linearVelocity.Length() / num;
			Vector2 size = new Vector2(vector2.X - vector.X - m_textBoxSize.X - m_textOffsetInsideBox.X, m_textBoxSize.Y);
			AddProgressBar(frame, vector + new Vector2(size.X * 0.5f + m_textBoxSize.X + m_textOffsetInsideBox.X, m_textBoxSize.Y / 2f), size, ratio, barBgColor, m_foregroundColor);
		}

		private MySpriteDrawFrame DrawSpeedIndicator(MySpriteDrawFrame frame, Vector2 drawPos, Vector2 textBoxSize, Vector3 velocity)
		{
			AddTextBox(text: ((int)velocity.Length()).ToString(), frame: frame, position: drawPos + textBoxSize * 0.5f, size: textBoxSize, font: m_fontId, scale: m_fontScale, bgColor: m_foregroundColor, textColor: m_foregroundColor, bgSprite: "AH_TextBox", textOffset: m_textOffsetInsideBox.X);
			return frame;
		}
	}
}

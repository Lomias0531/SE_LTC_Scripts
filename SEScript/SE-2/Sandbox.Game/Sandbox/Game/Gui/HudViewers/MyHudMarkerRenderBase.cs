using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Gui;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GUI.HudViewers
{
	public class MyHudMarkerRenderBase
	{
		public class MyMarkerStyle
		{
			public string Font
			{
				get;
				set;
			}

			public MyHudTexturesEnum TextureDirectionIndicator
			{
				get;
				set;
			}

			public MyHudTexturesEnum TextureTarget
			{
				get;
				set;
			}

			public Color Color
			{
				get;
				set;
			}

			public float TextureTargetRotationSpeed
			{
				get;
				set;
			}

			public float TextureTargetScale
			{
				get;
				set;
			}

			public MyMarkerStyle(string font, MyHudTexturesEnum textureDirectionIndicator, MyHudTexturesEnum textureTarget, Color color, float textureTargetRotationSpeed = 0f, float textureTargetScale = 1f)
			{
				Font = font;
				TextureDirectionIndicator = textureDirectionIndicator;
				TextureTarget = textureTarget;
				Color = color;
				TextureTargetRotationSpeed = textureTargetRotationSpeed;
				TextureTargetScale = textureTargetScale;
			}
		}

		public class DistanceComparer : IComparer<MyHudEntityParams>
		{
			public int Compare(MyHudEntityParams x, MyHudEntityParams y)
			{
				return Vector3D.DistanceSquared(MySector.MainCamera.Position, y.Position).CompareTo(Vector3D.DistanceSquared(MySector.MainCamera.Position, x.Position));
			}
		}

		protected const double LS_METRES = 299792458.00013667;

		protected const double LY_METRES = 9.460730473E+15;

		protected MyGuiScreenHudBase m_hudScreen;

		protected List<MyMarkerStyle> m_markerStyles;

		protected int[] m_markerStylesForBlocks;

		protected DistanceComparer m_distanceComparer = new DistanceComparer();

		public MyHudMarkerRenderBase(MyGuiScreenHudBase hudScreen)
		{
			m_hudScreen = hudScreen;
			m_markerStyles = new List<MyMarkerStyle>();
			int num = AllocateMarkerStyle("White", MyHudTexturesEnum.DirectionIndicator, MyHudTexturesEnum.Target_neutral, MyHudConstants.MARKER_COLOR_WHITE);
			int num2 = AllocateMarkerStyle("Red", MyHudTexturesEnum.DirectionIndicator, MyHudTexturesEnum.Target_enemy, MyHudConstants.MARKER_COLOR_WHITE);
			int num3 = AllocateMarkerStyle("DarkBlue", MyHudTexturesEnum.DirectionIndicator, MyHudTexturesEnum.Target_me, MyHudConstants.MARKER_COLOR_WHITE);
			int num4 = AllocateMarkerStyle("Green", MyHudTexturesEnum.DirectionIndicator, MyHudTexturesEnum.Target_friend, MyHudConstants.MARKER_COLOR_WHITE);
			m_markerStylesForBlocks = new int[MyUtils.GetMaxValueFromEnum<MyRelationsBetweenPlayerAndBlock>() + 1];
			m_markerStylesForBlocks[3] = num;
			m_markerStylesForBlocks[4] = num2;
			m_markerStylesForBlocks[1] = num3;
			m_markerStylesForBlocks[2] = num4;
			m_markerStylesForBlocks[0] = num4;
		}

		public virtual void Update()
		{
		}

		public virtual void Draw()
		{
		}

		public int AllocateMarkerStyle(string font, MyHudTexturesEnum directionIcon, MyHudTexturesEnum targetIcon, Color color)
		{
			int count = m_markerStyles.Count;
			m_markerStyles.Add(new MyMarkerStyle(font, directionIcon, targetIcon, color));
			return count;
		}

		public void OverrideStyleForRelation(MyRelationsBetweenPlayerAndBlock relation, string font, MyHudTexturesEnum directionIcon, MyHudTexturesEnum targetIcon, Color color)
		{
			int styleForRelation = GetStyleForRelation(relation);
			m_markerStyles[styleForRelation] = new MyMarkerStyle(font, directionIcon, targetIcon, color);
		}

		public int GetStyleForRelation(MyRelationsBetweenPlayerAndBlock relation)
		{
			return m_markerStylesForBlocks[(int)relation];
		}

		public virtual void DrawLocationMarkers(MyHudLocationMarkers locationMarkers)
		{
		}

		protected void AddTexturedQuad(MyHudTexturesEnum texture, Vector2 position, Vector2 upVector, Color color, float halfWidth, float halfHeight)
		{
			Vector2 rightVector = new Vector2(0f - upVector.Y, upVector.X);
			MyAtlasTextureCoordinate textureCoord = m_hudScreen.GetTextureCoord(texture);
			Vector2 vector = new Vector2(MyGuiManager.GetSafeFullscreenRectangle().Width, MyGuiManager.GetSafeFullscreenRectangle().Height);
			float x = vector.X / MyGuiManager.GetHudSize().X;
			float y = vector.Y / MyGuiManager.GetHudSize().Y;
			Vector2 position2 = position;
			if (MyVideoSettingsManager.IsTripleHead())
			{
				position2.X += 1f;
			}
			float num = vector.Y / 1080f;
			halfWidth *= num;
			halfHeight *= num;
			MyRenderProxy.DrawSpriteAtlas(m_hudScreen.TextureAtlas, position2, textureCoord.Offset, textureCoord.Size, rightVector, new Vector2(x, y), color, new Vector2(halfWidth, halfHeight));
		}

		protected void AddTexturedQuad(string texture, Vector2 position, Vector2 upVector, Color color, float halfWidth, float halfHeight)
		{
			Vector2 vector = new Vector2(MyGuiManager.GetSafeFullscreenRectangle().Width, MyGuiManager.GetSafeFullscreenRectangle().Height);
			float num = vector.X / MyGuiManager.GetHudSize().X;
			float num2 = vector.Y / MyGuiManager.GetHudSize().Y;
			if (MyVideoSettingsManager.IsTripleHead())
			{
				position.X += 1f;
			}
			position.X *= num;
			position.Y *= num2;
			RectangleF destination = new RectangleF(position.X - halfWidth, position.Y - halfHeight, halfWidth * 2f, halfHeight * 2f);
			MyRenderProxy.DrawSprite(texture, ref destination, null, color, 0f);
		}
	}
}

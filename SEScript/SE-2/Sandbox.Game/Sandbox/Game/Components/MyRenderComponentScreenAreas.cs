using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Collections;
using VRage.Game.Definitions;
using VRage.Game.Entity;
using VRage.Game.GUI.TextPanel;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Components
{
	public class MyRenderComponentScreenAreas : MyRenderComponentCubeBlock
	{
		private class PanelScreenArea
		{
			public uint[] RenderObjectIDs;

			public string Material;
		}

		private class Sandbox_Game_Components_MyRenderComponentScreenAreas_003C_003EActor
		{
		}

		private MyEntity m_entity;

		private List<PanelScreenArea> m_screenAreas = new List<PanelScreenArea>();

		public MyRenderComponentScreenAreas(MyEntity entity)
		{
			m_entity = entity;
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
		}

		public void UpdateModelProperties()
		{
			for (int i = 0; i < m_screenAreas.Count; i++)
			{
				for (int j = 0; j < m_screenAreas[i].RenderObjectIDs.Length; j++)
				{
					if (m_screenAreas[i].RenderObjectIDs[j] != uint.MaxValue)
					{
						MyRenderProxy.UpdateModelProperties(m_screenAreas[i].RenderObjectIDs[j], m_screenAreas[i].Material, (RenderFlags)0, (RenderFlags)0, null, null);
					}
				}
			}
		}

		public void ChangeTexture(int area, string path)
		{
			if (area >= m_screenAreas.Count)
			{
				return;
			}
			if (string.IsNullOrEmpty(path))
			{
				for (int i = 0; i < m_screenAreas[area].RenderObjectIDs.Length; i++)
				{
					if (m_screenAreas[area].RenderObjectIDs[i] != uint.MaxValue)
					{
						MyRenderProxy.ChangeMaterialTexture(m_screenAreas[area].RenderObjectIDs[i], m_screenAreas[area].Material);
						MyRenderProxy.UpdateModelProperties(m_screenAreas[area].RenderObjectIDs[i], m_screenAreas[area].Material, (RenderFlags)0, RenderFlags.Visible, null, null);
					}
				}
				return;
			}
			for (int j = 0; j < m_screenAreas[area].RenderObjectIDs.Length; j++)
			{
				if (m_screenAreas[area].RenderObjectIDs[j] != uint.MaxValue)
				{
					MyRenderProxy.ChangeMaterialTexture(m_screenAreas[area].RenderObjectIDs[j], m_screenAreas[area].Material, path);
					if (m_screenAreas[area].Material != "TransparentScreenArea")
					{
						MyRenderProxy.UpdateModelProperties(m_screenAreas[area].RenderObjectIDs[j], m_screenAreas[area].Material, RenderFlags.Visible, (RenderFlags)0, null, null);
					}
				}
			}
		}

		public string GenerateOffscreenTextureName(long entityId, int area)
		{
			return $"LCDOffscreenTexture_{entityId}_{m_screenAreas[area].Material}";
		}

		public void CreateTexture(int area, Vector2I textureSize)
		{
			MyRenderProxy.CreateGeneratedTexture(GenerateOffscreenTextureName(m_entity.EntityId, area), textureSize.X, textureSize.Y);
		}

		internal static Vector2 CalcAspectFactor(Vector2I textureSize, Vector2 aspectRatio)
		{
			Vector2 value = (textureSize.X > textureSize.Y) ? new Vector2(1f, textureSize.X / textureSize.Y) : new Vector2(textureSize.Y / textureSize.X, 1f);
			return aspectRatio * value;
		}

		internal static Vector2 CalcShift(Vector2I textureSize, Vector2 aspectFactor)
		{
			return textureSize * (aspectFactor - Vector2.One) / 2f;
		}

		public void RenderSpritesToTexture(int area, ListReader<MySprite> sprites, Vector2I textureSize, Vector2 aspectRatio, Color backgroundColor, byte backgroundAlpha)
		{
			string text = GenerateOffscreenTextureName(m_entity.EntityId, area);
			Vector2 vector = CalcAspectFactor(textureSize, aspectRatio);
			Vector2 vector2 = CalcShift(textureSize, vector);
			for (int i = 0; i < sprites.Count; i++)
			{
				MySprite mySprite = sprites[i];
				Vector2 value = mySprite.Size ?? ((Vector2)textureSize);
				Vector2 vector3 = mySprite.Position ?? ((Vector2)(textureSize / 2));
				Color color = mySprite.Color ?? Color.White;
				vector3 += vector2;
				switch (mySprite.Type)
				{
				case SpriteType.TEXTURE:
				{
					MyLCDTextureDefinition definition2 = MyDefinitionManager.Static.GetDefinition<MyLCDTextureDefinition>(MyStringHash.GetOrCompute(mySprite.Data));
					if (definition2 != null)
					{
						switch (mySprite.Alignment)
						{
						case TextAlignment.LEFT:
							vector3 += new Vector2(value.X * 0.5f, 0f);
							break;
						case TextAlignment.RIGHT:
							vector3 -= new Vector2(value.X * 0.5f, 0f);
							break;
						}
						Vector2 rightVector = new Vector2(1f, 0f);
						if (Math.Abs(mySprite.RotationOrScale) > 1E-05f)
						{
							rightVector = new Vector2((float)Math.Cos(mySprite.RotationOrScale), (float)Math.Sin(mySprite.RotationOrScale));
						}
						MyRenderProxy.DrawSpriteAtlas(definition2.SpritePath ?? definition2.TexturePath, vector3, Vector2.Zero, Vector2.One, rightVector, Vector2.One, color, value / 2f, text);
					}
					break;
				}
				case SpriteType.TEXT:
				{
					switch (mySprite.Alignment)
					{
					case TextAlignment.RIGHT:
						vector3 -= new Vector2(value.X, 0f);
						break;
					case TextAlignment.CENTER:
						vector3 -= new Vector2(value.X * 0.5f, 0f);
						break;
					}
					MyFontDefinition definition = MyDefinitionManager.Static.GetDefinition<MyFontDefinition>(MyStringHash.GetOrCompute(mySprite.FontId));
					int textureWidthinPx = (int)Math.Round(value.X);
					MyRenderProxy.DrawStringAligned((int)(definition?.Id.SubtypeId ?? MyStringHash.GetOrCompute("Debug")), vector3, color, mySprite.Data ?? string.Empty, mySprite.RotationOrScale, float.PositiveInfinity, text, textureWidthinPx, (MyRenderTextAlignmentEnum)mySprite.Alignment);
					break;
				}
				}
			}
			backgroundColor.A = backgroundAlpha;
			uint[] renderObjectIDs = m_screenAreas[area].RenderObjectIDs;
			int j;
			for (j = 0; j < renderObjectIDs.Length && renderObjectIDs[j] == uint.MaxValue; j++)
			{
			}
			if (j < renderObjectIDs.Length)
			{
				MyRenderProxy.RenderOffscreenTexture(text, vector, backgroundColor);
			}
		}

		public override void AddRenderObjects()
		{
			base.AddRenderObjects();
			UpdateRenderAreas();
		}

		protected void UpdateRenderAreas()
		{
			for (int i = 0; i < base.RenderObjectIDs.Length; i++)
			{
				for (int j = 0; j < m_screenAreas.Count; j++)
				{
					m_screenAreas[j].RenderObjectIDs[i] = base.RenderObjectIDs[i];
				}
			}
		}

		public override void ReleaseRenderObjectID(int index)
		{
			base.ReleaseRenderObjectID(index);
			for (int i = 0; i < m_screenAreas.Count; i++)
			{
				m_screenAreas[i].RenderObjectIDs[index] = uint.MaxValue;
			}
		}

		public void AddScreenArea(uint[] renderObjectIDs, string materialName)
		{
			m_screenAreas.Add(new PanelScreenArea
			{
				RenderObjectIDs = renderObjectIDs.ToArray(),
				Material = materialName
			});
		}

		public void ReleaseTexture(int area)
		{
			MyRenderProxy.UnloadTexture(GenerateOffscreenTextureName(m_entity.EntityId, area));
		}
	}
}

using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage.Game;
using VRage.Game.Gui;
using VRage.Generics;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenHudBase : MyGuiScreenBase
	{
		private static readonly MyStringId ID_SQUARE = MyStringId.GetOrCompute("Square");

		protected string m_atlas;

		protected MyAtlasTextureCoordinate[] m_atlasCoords;

		protected float m_textScale;

		protected StringBuilder m_hudIndicatorText = new StringBuilder();

		protected StringBuilder m_helperSB = new StringBuilder();

		protected MyObjectsPoolSimple<MyHudText> m_texts;

		public string TextureAtlas => m_atlas;

		public MyGuiScreenHudBase()
			: base(Vector2.Zero)
		{
			base.CanBeHidden = true;
			base.CanHideOthers = false;
			base.CanHaveFocus = false;
			m_drawEvenWithoutFocus = true;
			m_closeOnEsc = false;
			m_texts = new MyObjectsPoolSimple<MyHudText>(2000);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenHudBase";
		}

		public override void LoadContent()
		{
			LoadTextureAtlas(out m_atlas, out m_atlasCoords);
			base.LoadContent();
		}

		public static void LoadTextureAtlas(out string atlasFile, out MyAtlasTextureCoordinate[] atlasCoords)
		{
			MyTextureAtlasUtils.LoadTextureAtlas(MyEnumsToStrings.HudTextures, "Textures\\HUD\\", "Textures\\HUD\\HudAtlas.tai", out atlasFile, out atlasCoords);
		}

		public override void UnloadContent()
		{
			m_atlas = null;
			m_atlasCoords = null;
			base.UnloadContent();
		}

		public static Vector2 ConvertHudToNormalizedGuiPosition(ref Vector2 hudPos)
		{
			Rectangle safeFullscreenRectangle = MyGuiManager.GetSafeFullscreenRectangle();
			Vector2 value = new Vector2(safeFullscreenRectangle.Width, safeFullscreenRectangle.Height);
			Vector2 value2 = new Vector2(safeFullscreenRectangle.X, safeFullscreenRectangle.Y);
			Rectangle safeGuiRectangle = MyGuiManager.GetSafeGuiRectangle();
			Vector2 value3 = new Vector2(safeGuiRectangle.Width, safeGuiRectangle.Height);
			Vector2 value4 = new Vector2(safeGuiRectangle.X, safeGuiRectangle.Y);
			return (hudPos * value + value2 - value4) / value3;
		}

		protected static Vector2 ConvertNormalizedGuiToHud(ref Vector2 normGuiPos)
		{
			Rectangle safeFullscreenRectangle = MyGuiManager.GetSafeFullscreenRectangle();
			Vector2 value = new Vector2(safeFullscreenRectangle.Width, safeFullscreenRectangle.Height);
			Vector2 value2 = new Vector2(safeFullscreenRectangle.X, safeFullscreenRectangle.Y);
			Rectangle safeGuiRectangle = MyGuiManager.GetSafeGuiRectangle();
			Vector2 value3 = new Vector2(safeGuiRectangle.Width, safeGuiRectangle.Height);
			Vector2 value4 = new Vector2(safeGuiRectangle.X, safeGuiRectangle.Y);
			return (normGuiPos * value3 + value4 - value2) / value;
		}

		public static void HandleSelectedObjectHighlight(MyHudSelectedObject selection, MyHudObjectHighlightStyleData? data)
		{
			if (selection.PreviousObject.Instance != null)
			{
				RemoveObjectHighlightInternal(ref selection.PreviousObject, reset: true);
			}
			switch (selection.State)
			{
			case MyHudSelectedObjectState.VisibleStateSet:
				if (selection.Visible && (selection.CurrentObject.Style == MyHudObjectHighlightStyle.OutlineHighlight || selection.CurrentObject.Style == MyHudObjectHighlightStyle.EdgeHighlight || selection.CurrentObject.Style == MyHudObjectHighlightStyle.DummyHighlight || selection.VisibleRenderID != selection.CurrentObject.Instance.RenderObjectID))
				{
					DrawSelectedObjectHighlight(selection, data);
				}
				break;
			case MyHudSelectedObjectState.MarkedForVisible:
				DrawSelectedObjectHighlight(selection, data);
				break;
			case MyHudSelectedObjectState.MarkedForNotVisible:
				RemoveObjectHighlight(selection);
				break;
			}
		}

		private static void DrawSelectedObjectHighlight(MyHudSelectedObject selection, MyHudObjectHighlightStyleData? data)
		{
			if (selection.InteractiveObject.RenderObjectID == uint.MaxValue)
			{
				return;
			}
			switch (selection.HighlightStyle)
			{
			case MyHudObjectHighlightStyle.None:
				return;
			case MyHudObjectHighlightStyle.DummyHighlight:
				DrawSelectedObjectHighlightDummy(selection, data.Value.AtlasTexture, data.Value.TextureCoord);
				break;
			case MyHudObjectHighlightStyle.OutlineHighlight:
				if (selection.SectionNames != null && selection.SectionNames.Length == 0 && selection.SubpartIndices == null)
				{
					DrawSelectedObjectHighlightDummy(selection, data.Value.AtlasTexture, data.Value.TextureCoord);
				}
				else
				{
					DrawSelectedObjectHighlightOutline(selection);
				}
				break;
			case MyHudObjectHighlightStyle.EdgeHighlight:
				DrawSelectedObjectHighlightOutline(selection, edgeHighlight: true);
				break;
			default:
				throw new Exception("Unknown highlight style");
			}
			selection.Visible = true;
		}

		private static void RemoveObjectHighlight(MyHudSelectedObject selection)
		{
			RemoveObjectHighlightInternal(ref selection.CurrentObject, reset: false);
			selection.Visible = false;
		}

		private static void RemoveObjectHighlightInternal(ref MyHudSelectedObjectStatus status, bool reset)
		{
			MyHudObjectHighlightStyle style = status.Style;
			if ((uint)(style - 2) <= 1u && MySession.Static.GetComponent<MyHighlightSystem>() != null && !MySession.Static.GetComponent<MyHighlightSystem>().IsReserved(status.Instance.Owner.EntityId) && status.Instance.RenderObjectID != uint.MaxValue)
			{
				MyRenderProxy.UpdateModelHighlight(status.Instance.RenderObjectID, null, status.SubpartIndices, null, -1f, 0f, status.Instance.InstanceID);
			}
			if (reset)
			{
				status.Reset();
			}
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			MyHud.Crosshair.RecreateControls(constructor);
		}

		public override bool Update(bool hasFocus)
		{
			bool result = base.Update(hasFocus);
			if (MySandboxGame.Config.ShowCrosshair)
			{
				MyHud.Crosshair.Update();
			}
			return result;
		}

		public override bool Draw()
		{
			bool result = base.Draw();
			if (MySandboxGame.Config.ShowCrosshair && !MyHud.CutsceneHud)
			{
				MyHud.Crosshair.Draw(m_atlas, m_atlasCoords);
			}
			return result;
		}

		private static void DrawSelectedObjectHighlightOutline(MyHudSelectedObject selection, bool edgeHighlight = false)
		{
			Color color = selection.Color;
			if (edgeHighlight)
			{
				color.A = 0;
			}
			float contourHighlightThickness = MySector.EnvironmentDefinition.ContourHighlightThickness;
			float highlightPulseInSeconds = MySector.EnvironmentDefinition.HighlightPulseInSeconds;
			MyHighlightSystem component = MySession.Static.GetComponent<MyHighlightSystem>();
			if (component != null && !component.IsReserved(selection.InteractiveObject.Owner.EntityId))
			{
				MyRenderProxy.UpdateModelHighlight(selection.InteractiveObject.RenderObjectID, selection.SectionNames, selection.SubpartIndices, color, contourHighlightThickness, highlightPulseInSeconds, selection.InteractiveObject.InstanceID);
			}
		}

		public static void DrawSelectedObjectHighlightDummy(MyHudSelectedObject selection, string atlasTexture, MyAtlasTextureCoordinate textureCoord)
		{
			Rectangle safeFullscreenRectangle = MyGuiManager.GetSafeFullscreenRectangle();
			Vector2 scale = new Vector2(safeFullscreenRectangle.Width, safeFullscreenRectangle.Height);
			MatrixD worldMatrix = selection.InteractiveObject.ActivationMatrix * MySector.MainCamera.ViewMatrix * MySector.MainCamera.ProjectionMatrix;
			BoundingBoxD boundingBoxD = new BoundingBoxD(-Vector3D.Half, Vector3D.Half).TransformSlow(ref worldMatrix);
			Vector2 vector = new Vector2((float)boundingBoxD.Min.X, (float)boundingBoxD.Min.Y);
			Vector2 vector2 = new Vector2((float)boundingBoxD.Max.X, (float)boundingBoxD.Max.Y);
			Vector2 vector3 = vector - vector2;
			vector = vector * 0.5f + 0.5f * Vector2.One;
			vector2 = vector2 * 0.5f + 0.5f * Vector2.One;
			vector.Y = 1f - vector.Y;
			vector2.Y = 1f - vector2.Y;
			float textureScale = (float)Math.Pow(Math.Abs(vector3.X), 0.34999999403953552) * 2.5f;
			if (selection.InteractiveObject.ShowOverlay)
			{
				BoundingBoxD localbox = new BoundingBoxD(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f));
				Color color = Color.Gold;
				color *= 0.4f;
				MatrixD worldMatrix2 = selection.InteractiveObject.ActivationMatrix;
				MatrixD worldToLocal = MatrixD.Invert(selection.InteractiveObject.WorldMatrix);
				MySimpleObjectDraw.DrawAttachedTransparentBox(ref worldMatrix2, ref localbox, ref color, selection.InteractiveObject.RenderObjectID, ref worldToLocal, MySimpleObjectRasterizer.Solid, 0, 0.05f, ID_SQUARE, null, onlyFrontFaces: true);
			}
			if (MyFakes.ENABLE_USE_OBJECT_CORNERS)
			{
				DrawSelectionCorner(atlasTexture, selection, textureCoord, scale, vector, -Vector2.UnitY, textureScale);
				DrawSelectionCorner(atlasTexture, selection, textureCoord, scale, new Vector2(vector.X, vector2.Y), Vector2.UnitX, textureScale);
				DrawSelectionCorner(atlasTexture, selection, textureCoord, scale, new Vector2(vector2.X, vector.Y), -Vector2.UnitX, textureScale);
				DrawSelectionCorner(atlasTexture, selection, textureCoord, scale, vector2, Vector2.UnitY, textureScale);
			}
		}

		public static void DrawSelectionCorner(string atlasTexture, MyHudSelectedObject selection, MyAtlasTextureCoordinate textureCoord, Vector2 scale, Vector2 pos, Vector2 rightVector, float textureScale)
		{
			if (MyVideoSettingsManager.IsTripleHead())
			{
				pos.X *= 3f;
			}
			MyRenderProxy.DrawSpriteAtlas(atlasTexture, pos, textureCoord.Offset, textureCoord.Size, rightVector, scale, selection.Color, selection.HalfSize / MyGuiManager.GetHudSize() * textureScale);
		}

		public MyHudText AllocateText()
		{
			return m_texts.Allocate();
		}

		public void DrawTexts()
		{
			if (m_texts.GetAllocatedCount() <= 0)
			{
				return;
			}
			for (int i = 0; i < m_texts.GetAllocatedCount(); i++)
			{
				MyHudText allocatedItem = m_texts.GetAllocatedItem(i);
				if (allocatedItem.GetStringBuilder().Length != 0)
				{
					string font = allocatedItem.Font;
					allocatedItem.Position /= MyGuiManager.GetHudSize();
					Vector2 position = ConvertHudToNormalizedGuiPosition(ref allocatedItem.Position);
					Vector2 textSize = MyGuiManager.MeasureString(font, allocatedItem.GetStringBuilder(), MyGuiSandbox.GetDefaultTextScaleWithLanguage());
					textSize *= allocatedItem.Scale;
					MyGuiTextShadows.DrawShadow(ref position, ref textSize, null, (float)(int)allocatedItem.Color.A / 255f, allocatedItem.Alignement);
					MyGuiManager.DrawString(font, allocatedItem.GetStringBuilder(), position, allocatedItem.Scale, allocatedItem.Color, allocatedItem.Alignement);
				}
			}
			m_texts.ClearAllAllocated();
		}

		public MyAtlasTextureCoordinate GetTextureCoord(MyHudTexturesEnum texture)
		{
			return m_atlasCoords[(uint)texture];
		}
	}
}

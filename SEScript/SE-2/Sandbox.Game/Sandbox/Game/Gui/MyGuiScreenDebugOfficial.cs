using Sandbox.Engine.Utils;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	internal class MyGuiScreenDebugOfficial : MyGuiScreenDebugBase
	{
		private static readonly Vector2 SCREEN_SIZE = new Vector2(0.4f, 1.2f);

		private static readonly float HIDDEN_PART_RIGHT = 0.04f;

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugOfficial";
		}

		public MyGuiScreenDebugOfficial()
			: base(new Vector2(MyGuiManager.GetMaxMouseCoord().X - SCREEN_SIZE.X * 0.5f + HIDDEN_PART_RIGHT, 0.5f), SCREEN_SIZE, MyGuiConstants.SCREEN_BACKGROUND_COLOR, isTopMostScreen: false)
		{
			base.CanBeHidden = true;
			base.CanHideOthers = false;
			m_canCloseInCloseAllScreenCalls = true;
			m_canShareInput = true;
			m_isTopScreen = false;
			m_isTopMostScreen = false;
			RecreateControls(constructor: true);
		}

		public override bool CloseScreen()
		{
			return base.CloseScreen();
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			Vector2 value = new Vector2(-0.05f, 0f);
			Vector2 vector = new Vector2(0.02f, 0.02f);
			Vector2 value2 = new Vector2(0.008f, 0.005f);
			float scale = 0.8f;
			float num = 0.02f;
			float num2 = SCREEN_SIZE.X - HIDDEN_PART_RIGHT - vector.X * 2f;
			float num3 = (SCREEN_SIZE.Y - 1f) / 2f;
			m_currentPosition = -m_size.Value / 2f;
			m_currentPosition += vector;
			m_currentPosition.Y += num3;
			m_scale = scale;
			AddCaption(MyCommonTexts.ScreenDebugOfficial_Caption, Color.White.ToVector4(), vector + new Vector2(0f - HIDDEN_PART_RIGHT, num3));
			m_currentPosition.Y += MyGuiConstants.SCREEN_CAPTION_DELTA_Y * 2f;
			AddCheckBox(MyCommonTexts.ScreenDebugOfficial_EnableDebugDraw, () => MyDebugDrawSettings.ENABLE_DEBUG_DRAW, delegate(bool b)
			{
				MyDebugDrawSettings.ENABLE_DEBUG_DRAW = b;
			}, enabled: true, null, Color.White.ToVector4(), value);
			m_currentPosition.Y += num;
			AddCheckBox(MyCommonTexts.ScreenDebugOfficial_ModelDummies, () => MyDebugDrawSettings.DEBUG_DRAW_MODEL_DUMMIES, delegate(bool b)
			{
				MyDebugDrawSettings.DEBUG_DRAW_MODEL_DUMMIES = b;
			}, enabled: true, null, Color.White.ToVector4(), value);
			AddCheckBox(MyCommonTexts.ScreenDebugOfficial_MountPoints, () => MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS, delegate(bool b)
			{
				MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS = b;
			}, enabled: true, null, Color.White.ToVector4(), value);
			AddCheckBox(MyCommonTexts.ScreenDebugOfficial_PhysicsPrimitives, () => MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_SHAPES, delegate(bool b)
			{
				MyDebugDrawSettings.DEBUG_DRAW_PHYSICS |= b;
				MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_SHAPES = b;
			}, enabled: true, null, Color.White.ToVector4(), value);
			m_currentPosition.Y += num;
			CreateDebugButton(num2, MyCommonTexts.ScreenDebugOfficial_ReloadTextures, ReloadTextures);
			CreateDebugButton(num2, MyCommonTexts.ScreenDebugOfficial_ReloadModels, ReloadModels);
			CreateDebugButton(num2, MyCommonTexts.ScreenDebugOfficial_SavePrefab, SavePrefab, MyClipboardComponent.Static != null && MyClipboardComponent.Static.Clipboard.HasCopiedGrids(), MyCommonTexts.ToolTipSaveShip);
			AddSubcaption(MyTexts.GetString(MyCommonTexts.ScreenDebugOfficial_ErrorLogCaption), Color.White.ToVector4(), new Vector2(0f - HIDDEN_PART_RIGHT, 0f));
			CreateDebugButton(num2, MyCommonTexts.ScreenDebugOfficial_OpenErrorLog, CreateErrorLogScreen);
			CreateDebugButton(num2, MyCommonTexts.ScreenDebugOfficial_CopyErrorLogToClipboard, CopyErrorLogToClipboard);
			m_currentPosition.Y += num;
			Vector2 vector2 = MyGuiManager.GetMaxMouseCoord() / 2f - m_currentPosition;
			vector2.X = num2;
			vector2.Y -= vector.Y;
			m_currentPosition.X += value2.X / 2f;
			MyGuiControlPanel myGuiControlPanel = new MyGuiControlPanel(m_currentPosition - value2, vector2 + new Vector2(value2.X, value2.Y * 2f), null, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			myGuiControlPanel.BackgroundTexture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST;
			Controls.Add(myGuiControlPanel);
			MyGuiControlMultilineText myGuiControlMultilineText = AddMultilineText(vector2);
			if (MyDefinitionErrors.GetErrors().Count() == 0)
			{
				myGuiControlMultilineText.AppendText(MyTexts.Get(MyCommonTexts.ScreenDebugOfficial_NoErrorText));
				return;
			}
			ListReader<MyDefinitionErrors.Error> errors = MyDefinitionErrors.GetErrors();
			Dictionary<string, Tuple<int, TErrorSeverity>> dictionary = new Dictionary<string, Tuple<int, TErrorSeverity>>();
			foreach (MyDefinitionErrors.Error item in errors)
			{
				string key = item.ModName ?? "Local Content";
				if (dictionary.ContainsKey(key))
				{
					if (dictionary[key].Item2 == item.Severity)
					{
						Tuple<int, TErrorSeverity> tuple = dictionary[key];
						dictionary[key] = new Tuple<int, TErrorSeverity>(tuple.Item1 + 1, tuple.Item2);
					}
				}
				else
				{
					dictionary[key] = new Tuple<int, TErrorSeverity>(1, item.Severity);
				}
			}
			List<Tuple<string, int, TErrorSeverity>> list = new List<Tuple<string, int, TErrorSeverity>>();
			foreach (KeyValuePair<string, Tuple<int, TErrorSeverity>> item2 in dictionary)
			{
				list.Add(new Tuple<string, int, TErrorSeverity>(item2.Key, item2.Value.Item1, item2.Value.Item2));
			}
			Comparison<Tuple<string, int, TErrorSeverity>> comparison = (Tuple<string, int, TErrorSeverity> e1, Tuple<string, int, TErrorSeverity> e2) => e2.Item3 - e1.Item3;
			list.Sort(comparison);
			foreach (Tuple<string, int, TErrorSeverity> item3 in list)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(item3.Item1);
				stringBuilder.Append(" [");
				if (item3.Item3 == TErrorSeverity.Critical)
				{
					stringBuilder.Append(MyDefinitionErrors.Error.GetSeverityName(item3.Item3, plural: false));
					stringBuilder.Append("]");
				}
				else
				{
					stringBuilder.Append(item3.Item2.ToString());
					stringBuilder.Append(" ");
					stringBuilder.Append(MyDefinitionErrors.Error.GetSeverityName(item3.Item3, item3.Item2 != 1));
					stringBuilder.Append("]");
				}
				myGuiControlMultilineText.AppendText(stringBuilder, myGuiControlMultilineText.Font, myGuiControlMultilineText.TextScaleWithLanguage, MyDefinitionErrors.Error.GetSeverityColor(item3.Item3).ToVector4());
				myGuiControlMultilineText.AppendLine();
			}
		}

		private void CreateDebugButton(float usableWidth, MyStringId text, Action<MyGuiControlButton> onClick, bool enabled = true, MyStringId? tooltip = null)
		{
			MyGuiControlButton myGuiControlButton = AddButton(MyTexts.Get(text), onClick);
			myGuiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.Rectangular;
			myGuiControlButton.TextScale = m_scale;
			myGuiControlButton.Size = new Vector2(usableWidth, myGuiControlButton.Size.Y);
			myGuiControlButton.Position += new Vector2((0f - HIDDEN_PART_RIGHT) / 2f, 0f);
			myGuiControlButton.Enabled = enabled;
			if (tooltip.HasValue)
			{
				myGuiControlButton.SetToolTip(tooltip.Value);
			}
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyInput.Static.IsNewKeyPressed(MyKeys.F11))
			{
				if (MySession.Static.IsServer && (MySession.Static.CreativeMode || MySession.Static.CreativeToolsEnabled(Sync.MyId) || MySession.Static.IsUserAdmin(Sync.MyId)))
				{
					MyScreenManager.AddScreen(new MyGuiScreenScriptingTools());
				}
				CloseScreen();
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.F12) || MyInput.Static.IsNewKeyPressed(MyKeys.F10))
			{
				CloseScreen();
			}
		}

		private void CreateErrorLogScreen(MyGuiControlButton obj)
		{
			MyGuiSandbox.AddScreen(new MyGuiScreenDebugErrors());
		}

		private void ReloadTextures(MyGuiControlButton obj)
		{
			MyRenderProxy.ReloadTextures();
			MyHud.Notifications.Add(new MyHudNotificationDebug("Reloaded all textures in the game (modder only feature)", 2500, "Red"));
		}

		private void ReloadModels(MyGuiControlButton obj)
		{
			MyRenderProxy.ReloadModels();
			MyHud.Notifications.Add(new MyHudNotificationDebug("Reloaded all models in the game (modder only feature)", 2500, "Red"));
		}

		private void OpenBotsScreen(MyGuiControlButton obj)
		{
			MyGuiSandbox.AddScreen(new MyGuiScreenBotSettings());
		}

		private void SavePrefab(MyGuiControlButton obj)
		{
			string text = MyUtils.StripInvalidChars(MyClipboardComponent.Static.Clipboard.CopiedGridsName);
			string text2 = Path.Combine(MyFileSystem.UserDataPath, "Export", text + ".sbc");
			int num = 1;
			try
			{
				while (MyFileSystem.FileExists(text2))
				{
					text2 = Path.Combine(MyFileSystem.UserDataPath, "Export", text + "_" + num + ".sbc");
					num++;
				}
				MyClipboardComponent.Static.Clipboard.SaveClipboardAsPrefab(text, text2);
			}
			catch (Exception ex)
			{
				MySandboxGame.Log.WriteLine($"Failed to write prefab at file {text2}, message: {ex.Message}, stack:{ex.StackTrace}");
			}
		}

		private void CopyErrorLogToClipboard(MyGuiControlButton obj)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (MyDefinitionErrors.GetErrors().Count() == 0)
			{
				stringBuilder.Append((object)MyTexts.Get(MyCommonTexts.ScreenDebugOfficial_NoErrorText));
			}
			foreach (MyDefinitionErrors.Error error in MyDefinitionErrors.GetErrors())
			{
				stringBuilder.Append(error.ToString());
				stringBuilder.AppendLine();
			}
			MyVRage.Platform.Clipboard = stringBuilder.ToString();
		}
	}
}

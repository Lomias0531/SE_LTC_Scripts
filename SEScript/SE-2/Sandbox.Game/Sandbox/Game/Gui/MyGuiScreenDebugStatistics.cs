using Sandbox.Engine.Utils;
using Sandbox.Game.Audio;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Audio;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenDebugStatistics : MyGuiScreenDebugBase
	{
		private static StringBuilder m_frameDebugText = new StringBuilder(1024);

		private static StringBuilder m_frameDebugTextRA = new StringBuilder(2048);

		private static List<StringBuilder> m_texts = new List<StringBuilder>(32);

		private static List<StringBuilder> m_rightAlignedtexts = new List<StringBuilder>(32);

		private List<MyKeys> m_pressedKeys = new List<MyKeys>(10);

		private static List<StringBuilder> m_statsStrings = new List<StringBuilder>();

		private static int m_stringIndex = 0;

		public static StringBuilder StringBuilderCache
		{
			get
			{
				if (m_stringIndex >= m_statsStrings.Count)
				{
					m_statsStrings.Add(new StringBuilder(1024));
				}
				return m_statsStrings[m_stringIndex++].Clear();
			}
		}

		public MyGuiScreenDebugStatistics()
			: base(new Vector2(0.5f, 0.5f), default(Vector2), null, isTopMostScreen: true)
		{
			m_isTopMostScreen = true;
			m_drawEvenWithoutFocus = true;
			base.CanHaveFocus = false;
			m_canShareInput = false;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugStatistics";
		}

		public void AddToFrameDebugText(string s)
		{
			m_frameDebugText.AppendLine(s);
		}

		public void AddToFrameDebugText(StringBuilder s)
		{
			m_frameDebugText.AppendStringBuilder(s);
			m_frameDebugText.AppendLine();
		}

		public void AddDebugTextRA(string s)
		{
			m_frameDebugTextRA.Append(s);
			m_frameDebugTextRA.AppendLine();
		}

		public void AddDebugTextRA(StringBuilder s)
		{
			m_frameDebugTextRA.AppendStringBuilder(s);
			m_frameDebugTextRA.AppendLine();
		}

		public void ClearFrameDebugText()
		{
			m_frameDebugText.Clear();
			m_frameDebugTextRA.Clear();
		}

		public Vector2 GetScreenLeftTopPosition()
		{
			float num = 25f * MyGuiManager.GetSafeScreenScale();
			MyGuiManager.GetSafeFullscreenRectangle();
			return MyGuiManager.GetNormalizedCoordinateFromScreenCoordinate_FULLSCREEN(new Vector2(num, num));
		}

		public Vector2 GetScreenRightTopPosition()
		{
			float num = 25f * MyGuiManager.GetSafeScreenScale();
			return MyGuiManager.GetNormalizedCoordinateFromScreenCoordinate_FULLSCREEN(new Vector2((float)MyGuiManager.GetSafeFullscreenRectangle().Width - num, num));
		}

		public override bool Draw()
		{
			if (!base.Draw())
			{
				return false;
			}
			float dEBUG_STATISTICS_ROW_DISTANCE = MyGuiConstants.DEBUG_STATISTICS_ROW_DISTANCE;
			float dEBUG_STATISTICS_TEXT_SCALE = MyGuiConstants.DEBUG_STATISTICS_TEXT_SCALE;
			m_stringIndex = 0;
			m_texts.Clear();
			m_rightAlignedtexts.Clear();
			m_texts.Add(StringBuilderCache.GetFormatedFloat("FPS: ", MyFpsManager.GetFps()));
			m_texts.Add(new StringBuilder("Renderer: ").Append(MyRenderProxy.RendererInterfaceName()));
			if (MySector.MainCamera != null)
			{
				m_texts.Add(GetFormatedVector3(StringBuilderCache, "Camera pos: ", MySector.MainCamera.Position));
			}
			m_texts.Add(MyScreenManager.GetGuiScreensForDebug());
			m_texts.Add(StringBuilderCache.GetFormatedBool("Paused: ", MySandboxGame.IsPaused));
			m_texts.Add(StringBuilderCache.GetFormatedTimeSpan("Total GAME-PLAY Time: ", TimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds)));
			m_texts.Add(StringBuilderCache.GetFormatedTimeSpan("Total Session Time: ", (MySession.Static == null) ? new TimeSpan(0L) : MySession.Static.ElapsedPlayTime));
			m_texts.Add(StringBuilderCache.GetFormatedTimeSpan("Total Foot Time: ", (MySession.Static == null) ? new TimeSpan(0L) : MySession.Static.TimeOnFoot));
			m_texts.Add(StringBuilderCache.GetFormatedTimeSpan("Total Jetpack Time: ", (MySession.Static == null) ? new TimeSpan(0L) : MySession.Static.TimeOnJetpack));
			m_texts.Add(StringBuilderCache.GetFormatedTimeSpan("Total Small Ship Time: ", (MySession.Static == null) ? new TimeSpan(0L) : MySession.Static.TimePilotingSmallShip));
			m_texts.Add(StringBuilderCache.GetFormatedTimeSpan("Total Big Ship Time: ", (MySession.Static == null) ? new TimeSpan(0L) : MySession.Static.TimePilotingBigShip));
			m_texts.Add(StringBuilderCache.GetFormatedTimeSpan("Total Time: ", TimeSpan.FromMilliseconds(MySandboxGame.TotalTimeInMilliseconds)));
			m_texts.Add(StringBuilderCache.GetFormatedLong("GC.GetTotalMemory: ", GC.GetTotalMemory(forceFullCollection: false), " bytes"));
			m_texts.Add(StringBuilderCache.GetFormatedFloat("Allocated videomemory: ", 0f, " MB"));
			m_texts.Add(StringBuilderCache.GetFormatedInt("Sound Instances Total: ", MyAudio.Static.GetSoundInstancesTotal2D()).Append(" 2d / ").AppendInt32(MyAudio.Static.GetSoundInstancesTotal3D())
				.Append(" 3d"));
			if (MyMusicController.Static != null)
			{
				if (MyMusicController.Static.CategoryPlaying.Equals(MyStringId.NullOrEmpty))
				{
					m_texts.Add(StringBuilderCache.Append("No music playing, last category: " + MyMusicController.Static.CategoryLast.ToString() + ", next track in ").AppendDecimal(Math.Max(0f, MyMusicController.Static.NextMusicTrackIn), 1).Append("s"));
				}
				else
				{
					m_texts.Add(StringBuilderCache.Append("Playing music category: " + MyMusicController.Static.CategoryPlaying.ToString()));
				}
			}
			if (MyPerGameSettings.UseReverbEffect && MyFakes.AUDIO_ENABLE_REVERB)
			{
				m_texts.Add(StringBuilderCache.Append("Current reverb effect: " + (MyAudio.Static.EnableReverb ? MyEntityReverbDetectorComponent.CurrentReverbPreset.ToLower() : "disabled")));
			}
			StringBuilder stringBuilderCache = StringBuilderCache;
			MyAudio.Static.WriteDebugInfo(stringBuilderCache);
			m_texts.Add(stringBuilderCache);
			for (int i = 0; i < 8; i++)
			{
				m_texts.Add(StringBuilderCache.Clear());
			}
			MyInput.Static.GetPressedKeys(m_pressedKeys);
			AddPressedKeys("Current keys              : ", m_pressedKeys);
			m_texts.Add(StringBuilderCache.Clear());
			m_texts.Add(m_frameDebugText);
			m_rightAlignedtexts.Add(m_frameDebugTextRA);
			Vector2 screenLeftTopPosition = GetScreenLeftTopPosition();
			Vector2 screenRightTopPosition = GetScreenRightTopPosition();
			for (int j = 0; j < m_texts.Count; j++)
			{
				MyGuiManager.DrawString("White", m_texts[j], screenLeftTopPosition + new Vector2(0f, (float)j * dEBUG_STATISTICS_ROW_DISTANCE), dEBUG_STATISTICS_TEXT_SCALE, Color.Yellow);
			}
			for (int k = 0; k < m_rightAlignedtexts.Count; k++)
			{
				MyGuiManager.DrawString("White", m_rightAlignedtexts[k], screenRightTopPosition + new Vector2(-0.3f, (float)k * dEBUG_STATISTICS_ROW_DISTANCE), dEBUG_STATISTICS_TEXT_SCALE, Color.Yellow);
			}
			ClearFrameDebugText();
			return true;
		}

		private static StringBuilder GetFormatedVector3(StringBuilder sb, string before, Vector3D value, string after = "")
		{
			sb.Clear();
			sb.Append(before);
			sb.Append("{");
			sb.ConcatFormat("{0: #,000} ", value.X);
			sb.ConcatFormat("{0: #,000} ", value.Y);
			sb.ConcatFormat("{0: #,000} ", value.Z);
			sb.Append("}");
			sb.Append(after);
			return sb;
		}

		private void AddPressedKeys(string groupName, List<MyKeys> keys)
		{
			StringBuilder stringBuilderCache = StringBuilderCache;
			stringBuilderCache.Append(groupName);
			for (int i = 0; i < keys.Count; i++)
			{
				if (i > 0)
				{
					stringBuilderCache.Append(", ");
				}
				stringBuilderCache.Append(MyInput.Static.GetKeyName(keys[i]));
			}
			m_texts.Add(stringBuilderCache);
		}

		private StringBuilder GetShadowText(string text, int cascade, int value)
		{
			StringBuilder stringBuilderCache = StringBuilderCache;
			stringBuilderCache.Clear();
			stringBuilderCache.ConcatFormat("{0} (c {1}): ", text, cascade);
			stringBuilderCache.Concat(value);
			return stringBuilderCache;
		}

		private StringBuilder GetLodText(string text, int lod, int value)
		{
			StringBuilder stringBuilderCache = StringBuilderCache;
			stringBuilderCache.Clear();
			stringBuilderCache.ConcatFormat("{0}_LOD{1}: ", text, lod);
			stringBuilderCache.Concat(value);
			return stringBuilderCache;
		}
	}
}

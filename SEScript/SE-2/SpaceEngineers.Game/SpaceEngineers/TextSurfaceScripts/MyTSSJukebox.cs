using Sandbox.Definitions;
using Sandbox.Game.GameSystems.TextSurfaceScripts;
using Sandbox.Game.Localization;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.Entities.Blocks;
using System.Text;
using VRage;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace SpaceEngineers.TextSurfaceScripts
{
	[MyTextSurfaceScript("TSS_Jukebox", "DisplayName_TSS_VendingMachine")]
	public class MyTSSJukebox : MyTSSCommon
	{
		private static float DEFAULT_SCREEN_SIZE = 512f;

		private MyJukebox m_jukebox;

		private long m_tickCtr;

		public override ScriptUpdate NeedsUpdate => ScriptUpdate.Update10;

		public MyTSSJukebox(IMyTextSurface surface, IMyCubeBlock block, Vector2 size)
			: base(surface, block, size)
		{
			m_jukebox = (block as MyJukebox);
			m_fontId = "White";
		}

		public override void Dispose()
		{
			base.Dispose();
		}

		public override void Run()
		{
			base.Run();
			using (MySpriteDrawFrame frame = m_surface.DrawFrame())
			{
				Vector2 topLeftCorner = m_halfSize - m_surface.SurfaceSize * 0.5f;
				_ = m_surface.SurfaceSize.Y / m_surface.SurfaceSize.X;
				if (m_jukebox == null)
				{
					MySprite sprite = MySprite.CreateText(MyTexts.GetString(MySpaceTexts.VendingMachine_Script_DataUnavailable), m_fontId, m_foregroundColor, DEFAULT_SCREEN_SIZE / m_size.X);
					sprite.Position = m_halfSize;
					frame.Add(sprite);
					return;
				}
				MySoundCategoryDefinition.SoundDescription currentSoundDescription = m_jukebox.GetCurrentSoundDescription();
				DrawStatusMessage(frame, topLeftCorner, currentSoundDescription, m_jukebox.IsJukeboxPlaying);
			}
			m_tickCtr++;
		}

		private void DrawStatusMessage(MySpriteDrawFrame frame, Vector2 topLeftCorner, MySoundCategoryDefinition.SoundDescription selectedTrack, bool isPlaying)
		{
			Vector2 vector = topLeftCorner + new Vector2(m_surface.SurfaceSize.X * 0.5f, m_surface.SurfaceSize.Y * 0.32f);
			if (selectedTrack == null)
			{
				DrawMessage(frame, vector, MyTexts.GetString(MySpaceTexts.Jukebox_Script_NoTracksAvailable), m_scale.Y * 0.9f, drawBg: false);
				Vector2 position = vector + new Vector2(0f, m_surface.SurfaceSize.Y * 0.09f);
				DrawMessage(frame, position, MyTexts.GetString(MySpaceTexts.Jukebox_Script_SelectInTerminal), m_scale.Y * 0.4f, drawBg: false);
				return;
			}
			DrawMessage(frame, vector, selectedTrack.SoundText, m_scale.Y * 0.9f, drawBg: false);
			Vector2 position2 = vector + new Vector2(0f, m_surface.SurfaceSize.Y * 0.09f);
			if (isPlaying)
			{
				DrawMessage(frame, position2, MyTexts.GetString(MySpaceTexts.Jukebox_Script_Playing), m_scale.Y * 0.5f, drawBg: false);
				Vector2 value = vector + new Vector2(0f, m_surface.SurfaceSize.Y * 0.3f);
				Vector2 value2 = new Vector2(m_surface.SurfaceSize.X * 0.1f);
				MySprite sprite = new MySprite(SpriteType.TEXTURE, "Screen_LoadingBar", value, value2, null, null, TextAlignment.CENTER, (float)(-m_tickCtr) * 0.12f);
				frame.Add(sprite);
				MySprite sprite2 = new MySprite(SpriteType.TEXTURE, "Screen_LoadingBar", value, value2 * 0.66f, null, null, TextAlignment.CENTER, (float)m_tickCtr * 0.12f);
				frame.Add(sprite2);
				MySprite sprite3 = new MySprite(SpriteType.TEXTURE, "Screen_LoadingBar", value, value2 * 0.41f, null, null, TextAlignment.CENTER, (float)(-m_tickCtr) * 0.12f);
				frame.Add(sprite3);
			}
			else
			{
				DrawMessage(frame, position2, MyTexts.GetString(MySpaceTexts.Jukebox_Script_Stopped), m_scale.Y * 0.5f, drawBg: false);
			}
		}

		private void DrawMessage(MySpriteDrawFrame frame, Vector2 position, string messageString, float fontSize, bool drawBg = true)
		{
			Vector2 value = m_surface.MeasureStringInPixels(new StringBuilder(messageString), m_fontId, fontSize * 1.5f);
			if (drawBg)
			{
				MySprite sprite = MySprite.CreateSprite("SquareSimple", position, value * 1.05f);
				sprite.Color = Color.Black;
				frame.Add(sprite);
			}
			MySprite sprite2 = MySprite.CreateText(messageString, m_fontId, m_foregroundColor, fontSize * 1.5f);
			sprite2.Position = position - new Vector2(0f, value.Y * 0.5f);
			frame.Add(sprite2);
		}
	}
}

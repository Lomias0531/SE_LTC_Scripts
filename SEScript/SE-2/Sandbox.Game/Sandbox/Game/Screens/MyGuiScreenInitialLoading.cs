using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenInitialLoading : MyGuiScreenBase
	{
		private static MyGuiScreenInitialLoading m_instance;

		public static MyGuiScreenInitialLoading GetInstance
		{
			get
			{
				if (m_instance == null)
				{
					m_instance = new MyGuiScreenInitialLoading();
				}
				return m_instance;
			}
		}

		public static bool CloseInstance()
		{
			if (m_instance == null)
			{
				return false;
			}
			m_instance.CloseScreen();
			return true;
		}

		public override string GetFriendlyName()
		{
			return "Initial Loading screen";
		}

		private MyGuiScreenInitialLoading()
			: base(Vector2.Zero)
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			Vector2 normalizedCoord = new Vector2(1f, 0.9f);
			Vector2 normalizedSize = MyGuiManager.GetNormalizedSize(MyRenderProxy.GetTextureSize("Textures\\GUI\\screens\\screen_loading_wheel.dds"), 1f);
			MyGuiManager.DrawSpriteBatch("Textures\\GUI\\screens\\screen_loading_wheel.dds", normalizedCoord, 0.36f * normalizedSize, Color.White, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, useFullClientArea: false, waitTillLoaded: false, null, 0f, 1f);
			MyGuiManager.DrawSpriteBatch("Textures\\GUI\\screens\\screen_loading_wheel.dds", normalizedCoord, 0.216000021f * normalizedSize, Color.White, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, useFullClientArea: false, waitTillLoaded: false, null, 0f, -1f);
			MyGuiManager.DrawSpriteBatch("Textures\\GUI\\screens\\screen_loading_wheel.dds", normalizedCoord, 0.129600018f * normalizedSize, Color.White, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, useFullClientArea: false, waitTillLoaded: false, null, 0f, 1.5f);
		}
	}
}

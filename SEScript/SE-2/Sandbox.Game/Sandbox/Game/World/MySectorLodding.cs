using VRageRender;

namespace Sandbox.Game.World
{
	public class MySectorLodding
	{
		public MyNewLoddingSettings CurrentSettings = new MyNewLoddingSettings();

		private MyNewLoddingSettings m_lowSettings = new MyNewLoddingSettings();

		private MyNewLoddingSettings m_mediumSettings = new MyNewLoddingSettings();

		private MyNewLoddingSettings m_highSettings = new MyNewLoddingSettings();

		private MyNewLoddingSettings m_extremeSettings = new MyNewLoddingSettings();

		private MyRenderQualityEnum m_selectedQuality = MyRenderQualityEnum.HIGH;

		public MyNewLoddingSettings LowSettings => m_lowSettings;

		public MyNewLoddingSettings MediumSettings => m_mediumSettings;

		public MyNewLoddingSettings HighSettings => m_highSettings;

		public MyNewLoddingSettings ExtremeSettings => m_extremeSettings;

		public void UpdatePreset(MyNewLoddingSettings lowLoddingSettings, MyNewLoddingSettings mediumLoddingSettings, MyNewLoddingSettings highLoddingSettings, MyNewLoddingSettings extremeLoddingSettings)
		{
			m_lowSettings.CopyFrom(lowLoddingSettings);
			m_mediumSettings.CopyFrom(mediumLoddingSettings);
			m_highSettings.CopyFrom(highLoddingSettings);
			m_extremeSettings.CopyFrom(extremeLoddingSettings);
			SelectQuality(m_selectedQuality);
		}

		public void SelectQuality(MyRenderQualityEnum quality)
		{
			m_selectedQuality = quality;
			MyNewLoddingSettings settings;
			switch (quality)
			{
			default:
				return;
			case MyRenderQualityEnum.LOW:
				settings = LowSettings;
				break;
			case MyRenderQualityEnum.NORMAL:
				settings = MediumSettings;
				break;
			case MyRenderQualityEnum.HIGH:
				settings = HighSettings;
				break;
			case MyRenderQualityEnum.EXTREME:
				settings = ExtremeSettings;
				break;
			}
			CurrentSettings.CopyFrom(settings);
			MyRenderProxy.UpdateNewLoddingSettings(settings);
		}
	}
}

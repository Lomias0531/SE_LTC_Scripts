using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Mvvm;
using System;
using System.Globalization;
using VRage;

namespace Sandbox.Graphics
{
	public class MyLocalizationService : ILocalizationService
	{
		public BitmapImage GetLocalizedImage(string key, CultureInfo cultureInfo)
		{
			throw new NotImplementedException();
		}

		public SoundSource GetLocalizedSound(string key, CultureInfo cultureInfo)
		{
			throw new NotImplementedException();
		}

		public string GetLocalizedString(string key, CultureInfo cultureInfo)
		{
			return MyTexts.GetString(key);
		}
	}
}

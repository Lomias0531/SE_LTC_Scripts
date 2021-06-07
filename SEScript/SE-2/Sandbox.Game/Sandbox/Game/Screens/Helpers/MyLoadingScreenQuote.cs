using VRage.Utils;

namespace Sandbox.Game.Screens.Helpers
{
	internal class MyLoadingScreenQuote : MyLoadingScreenText
	{
		public readonly MyStringId Author;

		public MyLoadingScreenQuote(MyStringId text, MyStringId author)
			: base(text)
		{
			Author = author;
		}

		public static void Init()
		{
			MyStringId nullOrEmpty = MyStringId.NullOrEmpty;
			int num = 0;
			while ((nullOrEmpty = MyStringId.TryGet($"Quote{num:00}Text")) != MyStringId.NullOrEmpty)
			{
				MyStringId author = MyStringId.TryGet($"Quote{num:00}Author");
				MyLoadingScreenText.m_textsShared.Add(new MyLoadingScreenQuote(nullOrEmpty, author));
				num++;
			}
		}
	}
}

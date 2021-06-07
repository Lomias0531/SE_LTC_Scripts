namespace Sandbox.Engine.Platform.VideoMode
{
	public struct MyAspectRatio
	{
		public readonly MyAspectRatioEnum AspectRatioEnum;

		public readonly float AspectRatioNumber;

		public readonly string TextShort;

		public readonly bool IsTripleHead;

		public readonly bool IsSupported;

		public MyAspectRatio(bool isTripleHead, MyAspectRatioEnum aspectRatioEnum, float aspectRatioNumber, string textShort, bool isSupported)
		{
			IsTripleHead = isTripleHead;
			AspectRatioEnum = aspectRatioEnum;
			AspectRatioNumber = aspectRatioNumber;
			TextShort = textShort;
			IsSupported = isSupported;
		}
	}
}

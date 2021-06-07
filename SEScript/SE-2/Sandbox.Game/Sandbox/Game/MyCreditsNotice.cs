using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace Sandbox.Game
{
	public class MyCreditsNotice
	{
		public string LogoTexture;

		public Vector2? LogoTextureSize;

		public float? LogoScale;

		public float LogoOffset = 0.07f;

		public readonly List<StringBuilder> CreditNoticeLines;

		public MyCreditsNotice()
		{
			CreditNoticeLines = new List<StringBuilder>();
		}
	}
}

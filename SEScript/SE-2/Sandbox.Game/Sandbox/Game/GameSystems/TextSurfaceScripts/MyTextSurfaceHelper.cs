using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Sandbox.Game.GameSystems.TextSurfaceScripts
{
	public static class MyTextSurfaceHelper
	{
		public const string DEFAULT_FONT_ID = "Monospace";

		public const string DEFAULT_BG_TEXTURE = "Grid";

		public const string LEFT_BRACKET_TEXTURE = "DecorativeBracketLeft";

		public const string RIGHT_BRACKET_TEXTURE = "DecorativeBracketRight";

		public const string BLANK_TEXTURE = "SquareTapered";

		public const float BACKGROUND_SIZE = 682.6667f;

		public static readonly Vector2 BACKGROUND_SHIFT = new Vector2(682.6667f, 0f);

		public static readonly MySprite DEFAULT_BACKGROUND = new MySprite(SpriteType.TEXTURE, "Grid", new Vector2(-341.333344f, 0f), new Vector2(682.6667f));
	}
}

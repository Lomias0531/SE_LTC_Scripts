using System.Text;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	internal abstract class MyRichLabelPart
	{
		public virtual Vector2 Size
		{
			get;
			protected set;
		}

		public abstract bool Draw(Vector2 position, float alphamask, ref int charactersLeft);

		public abstract bool HandleInput(Vector2 position);

		public virtual void AppendTextTo(StringBuilder builder)
		{
		}
	}
}

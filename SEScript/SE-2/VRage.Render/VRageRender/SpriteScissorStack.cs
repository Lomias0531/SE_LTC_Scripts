using System.Collections.Generic;
using VRageMath;

namespace VRageRender
{
	/// <summary>
	/// Stores stack of scissor rectangles where top rectangle has already
	/// been cut using all the rectangles below it, so that only one
	/// rectangle is checked during scissor test.
	/// </summary>
	public class SpriteScissorStack
	{
		private readonly Stack<Rectangle> m_rectangleStack = new Stack<Rectangle>();

		public bool Empty => m_rectangleStack.Count == 0;

		public void Push(Rectangle scissorRect)
		{
			if (!Empty)
			{
				Rectangle value = m_rectangleStack.Peek();
				Rectangle.Intersect(ref scissorRect, ref value, out scissorRect);
			}
			m_rectangleStack.Push(scissorRect);
		}

		public void Pop()
		{
			if (!Empty)
			{
				m_rectangleStack.Pop();
			}
		}

		public RectangleF? Peek()
		{
			if (!Empty)
			{
				Rectangle rectangle = m_rectangleStack.Peek();
				return new RectangleF(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
			}
			return null;
		}

		/// <summary>
		/// Cuts the destination rectangle using top of the scissor stack.
		/// Source rectangle is modified using scaled change of destination
		/// as well.
		/// </summary>
		public void Cut(ref RectangleF destination, ref RectangleF source)
		{
			if (!Empty)
			{
				RectangleF other = destination;
				Rectangle rectangle = m_rectangleStack.Peek();
				RectangleF value = new RectangleF(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
				RectangleF.Intersect(ref destination, ref value, out destination);
				if (!destination.Equals(other))
				{
					Vector2 value2 = source.Size / other.Size;
					Vector2 value3 = destination.Size - other.Size;
					Vector2 value4 = destination.Position - other.Position;
					source.Position += value4 * value2;
					source.Size += value3 * value2;
				}
			}
		}
	}
}

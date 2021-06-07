using VRageMath;

namespace VRage
{
	public interface IMyImeActiveControl
	{
		bool IsImeActive
		{
			get;
			set;
		}

		void DeactivateIme();

		void InsertChar(bool conpositionEnd, char character);

		void InsertCharMultiple(bool conpositionEnd, string chars);

		void KeypressBackspace(bool conpositionEnd);

		void KeypressBackspaceMultiple(bool conpositionEnd, int count);

		void KeypressDelete(bool conpositionEnd);

		void KeypressEnter(bool conpositionEnd);

		void KeypressRedo();

		void KeypressUndo();

		/// <summary>
		/// Returns max number of characters.
		/// </summary>
		/// <returns></returns>
		int GetMaxLength();

		int GetSelectionLength();

		int GetTextLength();

		Vector2 GetCornerPosition();

		Vector2 GetCarriagePosition(int shiftX);
	}
}

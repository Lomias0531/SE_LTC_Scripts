namespace Sandbox.Gui
{
	public struct GridLength
	{
		public float Size;

		public GridUnitType UnitType;

		public GridLength(float size, GridUnitType unitType = GridUnitType.Ratio)
		{
			Size = size;
			UnitType = unitType;
		}
	}
}

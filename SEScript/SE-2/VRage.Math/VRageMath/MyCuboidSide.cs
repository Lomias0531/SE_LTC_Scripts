namespace VRageMath
{
	public class MyCuboidSide
	{
		public Plane Plane;

		public Line[] Lines = new Line[4];

		public MyCuboidSide()
		{
			Lines[0] = default(Line);
			Lines[1] = default(Line);
			Lines[2] = default(Line);
			Lines[3] = default(Line);
		}

		public void CreatePlaneFromLines()
		{
			Plane = new Plane(Lines[0].From, Vector3.Cross(Lines[1].Direction, Lines[0].Direction));
		}
	}
}

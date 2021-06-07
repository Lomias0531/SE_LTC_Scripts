using System.Collections.Generic;

namespace VRageMath
{
	public class MyCuboid
	{
		public MyCuboidSide[] Sides = new MyCuboidSide[6];

		public IEnumerable<Line> UniqueLines
		{
			get
			{
				yield return Sides[0].Lines[0];
				yield return Sides[0].Lines[1];
				yield return Sides[0].Lines[2];
				yield return Sides[0].Lines[3];
				yield return Sides[1].Lines[0];
				yield return Sides[1].Lines[1];
				yield return Sides[1].Lines[2];
				yield return Sides[1].Lines[3];
				yield return Sides[2].Lines[0];
				yield return Sides[2].Lines[2];
				yield return Sides[4].Lines[1];
				yield return Sides[5].Lines[2];
			}
		}

		public IEnumerable<Vector3> Vertices
		{
			get
			{
				yield return Sides[2].Lines[1].From;
				yield return Sides[2].Lines[1].To;
				yield return Sides[0].Lines[1].From;
				yield return Sides[0].Lines[1].To;
				yield return Sides[1].Lines[2].From;
				yield return Sides[1].Lines[2].To;
				yield return Sides[3].Lines[2].From;
				yield return Sides[3].Lines[2].To;
			}
		}

		public MyCuboid()
		{
			Sides[0] = new MyCuboidSide();
			Sides[1] = new MyCuboidSide();
			Sides[2] = new MyCuboidSide();
			Sides[3] = new MyCuboidSide();
			Sides[4] = new MyCuboidSide();
			Sides[5] = new MyCuboidSide();
		}

		public void CreateFromVertices(Vector3[] vertices)
		{
			Vector3 value = new Vector3(float.MaxValue);
			Vector3 value2 = new Vector3(float.MinValue);
			foreach (Vector3 value3 in vertices)
			{
				value = Vector3.Min(value3, value);
				value2 = Vector3.Min(value3, value2);
			}
			Line line = new Line(vertices[0], vertices[2], calculateBoundingBox: false);
			Line line2 = new Line(vertices[2], vertices[3], calculateBoundingBox: false);
			Line line3 = new Line(vertices[3], vertices[1], calculateBoundingBox: false);
			Line line4 = new Line(vertices[1], vertices[0], calculateBoundingBox: false);
			Line line5 = new Line(vertices[7], vertices[6], calculateBoundingBox: false);
			Line line6 = new Line(vertices[6], vertices[4], calculateBoundingBox: false);
			Line line7 = new Line(vertices[4], vertices[5], calculateBoundingBox: false);
			Line line8 = new Line(vertices[5], vertices[7], calculateBoundingBox: false);
			Line line9 = new Line(vertices[4], vertices[0], calculateBoundingBox: false);
			Line line10 = new Line(vertices[0], vertices[1], calculateBoundingBox: false);
			Line line11 = new Line(vertices[1], vertices[5], calculateBoundingBox: false);
			Line line12 = new Line(vertices[5], vertices[4], calculateBoundingBox: false);
			Line line13 = new Line(vertices[3], vertices[2], calculateBoundingBox: false);
			Line line14 = new Line(vertices[2], vertices[6], calculateBoundingBox: false);
			Line line15 = new Line(vertices[6], vertices[7], calculateBoundingBox: false);
			Line line16 = new Line(vertices[7], vertices[3], calculateBoundingBox: false);
			Line line17 = new Line(vertices[1], vertices[3], calculateBoundingBox: false);
			Line line18 = new Line(vertices[3], vertices[7], calculateBoundingBox: false);
			Line line19 = new Line(vertices[7], vertices[5], calculateBoundingBox: false);
			Line line20 = new Line(vertices[5], vertices[1], calculateBoundingBox: false);
			Line line21 = new Line(vertices[0], vertices[4], calculateBoundingBox: false);
			Line line22 = new Line(vertices[4], vertices[6], calculateBoundingBox: false);
			Line line23 = new Line(vertices[6], vertices[2], calculateBoundingBox: false);
			Line line24 = new Line(vertices[2], vertices[0], calculateBoundingBox: false);
			Sides[0].Lines[0] = line;
			Sides[0].Lines[1] = line2;
			Sides[0].Lines[2] = line3;
			Sides[0].Lines[3] = line4;
			Sides[0].CreatePlaneFromLines();
			Sides[1].Lines[0] = line5;
			Sides[1].Lines[1] = line6;
			Sides[1].Lines[2] = line7;
			Sides[1].Lines[3] = line8;
			Sides[1].CreatePlaneFromLines();
			Sides[2].Lines[0] = line9;
			Sides[2].Lines[1] = line10;
			Sides[2].Lines[2] = line11;
			Sides[2].Lines[3] = line12;
			Sides[2].CreatePlaneFromLines();
			Sides[3].Lines[0] = line13;
			Sides[3].Lines[1] = line14;
			Sides[3].Lines[2] = line15;
			Sides[3].Lines[3] = line16;
			Sides[3].CreatePlaneFromLines();
			Sides[4].Lines[0] = line17;
			Sides[4].Lines[1] = line18;
			Sides[4].Lines[2] = line19;
			Sides[4].Lines[3] = line20;
			Sides[4].CreatePlaneFromLines();
			Sides[5].Lines[0] = line21;
			Sides[5].Lines[1] = line22;
			Sides[5].Lines[2] = line23;
			Sides[5].Lines[3] = line24;
			Sides[5].CreatePlaneFromLines();
		}

		public void CreateFromSizes(float width1, float depth1, float width2, float depth2, float length)
		{
			float num = length * 0.5f;
			float num2 = width1 * 0.5f;
			float num3 = width2 * 0.5f;
			float num4 = depth1 * 0.5f;
			float num5 = depth2 * 0.5f;
			CreateFromVertices(new Vector3[8]
			{
				new Vector3(0f - num3, 0f - num, 0f - num5),
				new Vector3(num3, 0f - num, 0f - num5),
				new Vector3(0f - num3, 0f - num, num5),
				new Vector3(num3, 0f - num, num5),
				new Vector3(0f - num2, num, 0f - num4),
				new Vector3(num2, num, 0f - num4),
				new Vector3(0f - num2, num, num4),
				new Vector3(num2, num, num4)
			});
		}

		public BoundingBox GetAABB()
		{
			BoundingBox result = BoundingBox.CreateInvalid();
			foreach (Line uniqueLine in UniqueLines)
			{
				Vector3 point = uniqueLine.From;
				Vector3 point2 = uniqueLine.To;
				result = result.Include(ref point).Include(ref point2);
			}
			return result;
		}

		public BoundingBox GetLocalAABB()
		{
			BoundingBox aABB = GetAABB();
			Vector3 center = aABB.Center;
			aABB.Min -= center;
			aABB.Max -= center;
			return aABB;
		}

		public MyCuboid CreateTransformed(ref Matrix worldMatrix)
		{
			Vector3[] array = new Vector3[8];
			int num = 0;
			foreach (Vector3 vertex in Vertices)
			{
				array[num] = Vector3.Transform(vertex, worldMatrix);
				num++;
			}
			MyCuboid myCuboid = new MyCuboid();
			myCuboid.CreateFromVertices(array);
			return myCuboid;
		}
	}
}

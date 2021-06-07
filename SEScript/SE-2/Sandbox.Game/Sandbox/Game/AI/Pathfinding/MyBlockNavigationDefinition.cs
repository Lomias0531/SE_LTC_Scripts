using Sandbox.Definitions;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.AI.Pathfinding
{
	[MyDefinitionType(typeof(MyObjectBuilder_BlockNavigationDefinition), null)]
	public class MyBlockNavigationDefinition : MyDefinitionBase
	{
		private struct SizeAndCenter
		{
			private Vector3I Size;

			private Vector3I Center;

			public SizeAndCenter(Vector3I size, Vector3I center)
			{
				Size = size;
				Center = center;
			}

			public bool Equals(SizeAndCenter other)
			{
				if (other.Size == Size)
				{
					return other.Center == Center;
				}
				return false;
			}

			public override bool Equals(object obj)
			{
				if (obj == null)
				{
					return false;
				}
				if (obj.GetType() != typeof(SizeAndCenter))
				{
					return false;
				}
				return Equals((SizeAndCenter)obj);
			}

			public override int GetHashCode()
			{
				return Size.GetHashCode() * 1610612741 + Center.GetHashCode();
			}
		}

		private class Sandbox_Game_AI_Pathfinding_MyBlockNavigationDefinition_003C_003EActor : IActivator, IActivator<MyBlockNavigationDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyBlockNavigationDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBlockNavigationDefinition CreateInstance()
			{
				return new MyBlockNavigationDefinition();
			}

			MyBlockNavigationDefinition IActivator<MyBlockNavigationDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyGridNavigationMesh m_mesh;

		private static StringBuilder m_tmpStringBuilder = new StringBuilder();

		private static MyObjectBuilder_BlockNavigationDefinition m_tmpDefaultOb = new MyObjectBuilder_BlockNavigationDefinition();

		public MyGridNavigationMesh Mesh => m_mesh;

		public bool NoEntry
		{
			get;
			private set;
		}

		public MyBlockNavigationDefinition()
		{
			m_mesh = null;
			NoEntry = false;
		}

		public static MyObjectBuilder_BlockNavigationDefinition GetDefaultObjectBuilder(MyCubeBlockDefinition blockDefinition)
		{
			MyObjectBuilder_BlockNavigationDefinition tmpDefaultOb = m_tmpDefaultOb;
			m_tmpStringBuilder.Clear();
			m_tmpStringBuilder.Append("Default_");
			m_tmpStringBuilder.Append(blockDefinition.Size.X);
			m_tmpStringBuilder.Append("_");
			m_tmpStringBuilder.Append(blockDefinition.Size.Y);
			m_tmpStringBuilder.Append("_");
			m_tmpStringBuilder.Append(blockDefinition.Size.Z);
			tmpDefaultOb.Id = new MyDefinitionId(typeof(MyObjectBuilder_BlockNavigationDefinition), m_tmpStringBuilder.ToString());
			tmpDefaultOb.Size = blockDefinition.Size;
			tmpDefaultOb.Center = blockDefinition.Center;
			return tmpDefaultOb;
		}

		public static void CreateDefaultTriangles(MyObjectBuilder_BlockNavigationDefinition ob)
		{
			Vector3I vector3I = ob.Size;
			Vector3I value = ob.Center;
			int num = 4 * (vector3I.X * vector3I.Y + vector3I.X * vector3I.Z + vector3I.Y * vector3I.Z);
			ob.Triangles = new MyObjectBuilder_BlockNavigationDefinition.Triangle[num];
			int num2 = 0;
			Vector3 vector = vector3I * 0.5f - value - Vector3.Half;
			for (int i = 0; i < 6; i++)
			{
				Base6Directions.Direction direction = Base6Directions.EnumDirections[i];
				Vector3 vector2 = vector;
				Base6Directions.Direction direction2;
				Base6Directions.Direction direction3;
				switch (direction)
				{
				case Base6Directions.Direction.Right:
					direction2 = Base6Directions.Direction.Forward;
					direction3 = Base6Directions.Direction.Up;
					vector2 += new Vector3(0.5f, -0.5f, 0.5f) * vector3I;
					break;
				case Base6Directions.Direction.Left:
					direction2 = Base6Directions.Direction.Backward;
					direction3 = Base6Directions.Direction.Up;
					vector2 += new Vector3(-0.5f, -0.5f, -0.5f) * vector3I;
					break;
				case Base6Directions.Direction.Up:
					direction2 = Base6Directions.Direction.Right;
					direction3 = Base6Directions.Direction.Forward;
					vector2 += new Vector3(-0.5f, 0.5f, 0.5f) * vector3I;
					break;
				case Base6Directions.Direction.Down:
					direction2 = Base6Directions.Direction.Right;
					direction3 = Base6Directions.Direction.Backward;
					vector2 += new Vector3(-0.5f, -0.5f, -0.5f) * vector3I;
					break;
				case Base6Directions.Direction.Backward:
					direction2 = Base6Directions.Direction.Right;
					direction3 = Base6Directions.Direction.Up;
					vector2 += new Vector3(-0.5f, -0.5f, 0.5f) * vector3I;
					break;
				default:
					direction2 = Base6Directions.Direction.Left;
					direction3 = Base6Directions.Direction.Up;
					vector2 += new Vector3(0.5f, -0.5f, -0.5f) * vector3I;
					break;
				}
				Vector3 vector3 = Base6Directions.GetVector(direction2);
				Vector3 vector4 = Base6Directions.GetVector(direction3);
				int num3 = vector3I.AxisValue(Base6Directions.GetAxis(direction3));
				int num4 = vector3I.AxisValue(Base6Directions.GetAxis(direction2));
				for (int j = 0; j < num3; j++)
				{
					for (int k = 0; k < num4; k++)
					{
						MyObjectBuilder_BlockNavigationDefinition.Triangle triangle = new MyObjectBuilder_BlockNavigationDefinition.Triangle();
						triangle.Points = new SerializableVector3[3];
						triangle.Points[0] = vector2;
						triangle.Points[1] = vector2 + vector3;
						triangle.Points[2] = vector2 + vector4;
						ob.Triangles[num2++] = triangle;
						triangle = new MyObjectBuilder_BlockNavigationDefinition.Triangle();
						triangle.Points = new SerializableVector3[3];
						triangle.Points[0] = vector2 + vector3;
						triangle.Points[1] = vector2 + vector3 + vector4;
						triangle.Points[2] = vector2 + vector4;
						ob.Triangles[num2++] = triangle;
						vector2 += vector3;
					}
					vector2 -= vector3 * num4;
					vector2 += vector4;
				}
			}
		}

		protected override void Init(MyObjectBuilder_DefinitionBase ob)
		{
			base.Init(ob);
			MyObjectBuilder_BlockNavigationDefinition myObjectBuilder_BlockNavigationDefinition = ob as MyObjectBuilder_BlockNavigationDefinition;
			if (ob == null)
			{
				return;
			}
			if (myObjectBuilder_BlockNavigationDefinition.NoEntry || myObjectBuilder_BlockNavigationDefinition.Triangles == null)
			{
				NoEntry = true;
				return;
			}
			NoEntry = false;
			MyGridNavigationMesh myGridNavigationMesh = new MyGridNavigationMesh(null, null, myObjectBuilder_BlockNavigationDefinition.Triangles.Length);
			Vector3I max = myObjectBuilder_BlockNavigationDefinition.Size - Vector3I.One - myObjectBuilder_BlockNavigationDefinition.Center;
			Vector3I min = -(Vector3I)myObjectBuilder_BlockNavigationDefinition.Center;
			MyObjectBuilder_BlockNavigationDefinition.Triangle[] triangles = myObjectBuilder_BlockNavigationDefinition.Triangles;
			foreach (MyObjectBuilder_BlockNavigationDefinition.Triangle obj in triangles)
			{
				Vector3 a = obj.Points[0];
				Vector3 b = obj.Points[1];
				Vector3 c = obj.Points[2];
				MyNavigationTriangle tri = myGridNavigationMesh.AddTriangle(ref a, ref b, ref c);
				Vector3 value = (a + b + c) / 3f;
				Vector3 value2 = (value - a) * 0.0001f;
				Vector3 value3 = (value - b) * 0.0001f;
				Vector3 value4 = (value - c) * 0.0001f;
				Vector3I value5 = Vector3I.Round(a + value2);
				Vector3I value6 = Vector3I.Round(b + value3);
				Vector3I value7 = Vector3I.Round(c + value4);
				Vector3I.Clamp(ref value5, ref min, ref max, out value5);
				Vector3I.Clamp(ref value6, ref min, ref max, out value6);
				Vector3I.Clamp(ref value7, ref min, ref max, out value7);
				Vector3I.Min(ref value5, ref value6, out Vector3I result);
				Vector3I.Min(ref result, ref value7, out result);
				Vector3I.Max(ref value5, ref value6, out Vector3I result2);
				Vector3I.Max(ref result2, ref value7, out result2);
				Vector3I gridPos = result;
				Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref result, ref result2);
				while (vector3I_RangeIterator.IsValid())
				{
					myGridNavigationMesh.RegisterTriangle(tri, ref gridPos);
					vector3I_RangeIterator.GetNext(out gridPos);
				}
			}
			m_mesh = myGridNavigationMesh;
		}
	}
}

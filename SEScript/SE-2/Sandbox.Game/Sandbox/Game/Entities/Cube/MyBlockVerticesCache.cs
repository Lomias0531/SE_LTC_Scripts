using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	internal class MyBlockVerticesCache
	{
		private const bool ADD_INNER_BONES_TO_CONVEX = false;

		private static List<Vector3>[][] Cache;

		static MyBlockVerticesCache()
		{
			GenerateConvexVertices();
		}

		public static ListReader<Vector3> GetBlockVertices(MyCubeTopology topology, MyBlockOrientation orientation)
		{
			return new ListReader<Vector3>(Cache[(int)topology][(int)orientation.Forward * 6 + (int)orientation.Up]);
		}

		private static void GenerateConvexVertices()
		{
			List<Vector3> list = new List<Vector3>(27);
			Array values = Enum.GetValues(typeof(MyCubeTopology));
			Cache = new List<Vector3>[values.Length][];
			foreach (MyCubeTopology item in values)
			{
				GetTopologySwitch(item, list);
				Cache[(int)item] = new List<Vector3>[36];
				Base6Directions.Direction[] enumDirections = Base6Directions.EnumDirections;
				foreach (Base6Directions.Direction direction in enumDirections)
				{
					Base6Directions.Direction[] enumDirections2 = Base6Directions.EnumDirections;
					foreach (Base6Directions.Direction direction2 in enumDirections2)
					{
						if (direction != direction2 && !(Base6Directions.GetIntVector(direction) == -Base6Directions.GetIntVector(direction2)))
						{
							List<Vector3> list2 = new List<Vector3>(list.Count);
							Cache[(int)item][(int)direction * 6 + (int)direction2] = list2;
							MyBlockOrientation orientation = new MyBlockOrientation(direction, direction2);
							foreach (Vector3 item2 in list)
							{
								list2.Add(Vector3.TransformNormal(item2, orientation));
							}
						}
					}
				}
				list.Clear();
			}
		}

		private static void GetTopologySwitch(MyCubeTopology topology, List<Vector3> verts)
		{
			switch (topology)
			{
			case MyCubeTopology.StandaloneBox:
				break;
			case MyCubeTopology.Slope:
			case MyCubeTopology.RotatedSlope:
				verts.Add(new Vector3(-1f, 1f, -1f));
				verts.Add(new Vector3(1f, 1f, -1f));
				verts.Add(new Vector3(1f, -1f, 1f));
				verts.Add(new Vector3(-1f, -1f, 1f));
				verts.Add(new Vector3(-1f, -1f, -1f));
				verts.Add(new Vector3(1f, -1f, -1f));
				break;
			case MyCubeTopology.RoundSlope:
				verts.Add(new Vector3(-1f, 1f, -1f));
				verts.Add(new Vector3(1f, 1f, -1f));
				verts.Add(new Vector3(1f, -1f, 1f));
				verts.Add(new Vector3(-1f, -1f, 1f));
				verts.Add(new Vector3(-1f, -1f, -1f));
				verts.Add(new Vector3(1f, -1f, -1f));
				verts.Add(new Vector3(-1f, 0.414f, 0.414f));
				verts.Add(new Vector3(1f, 0.414f, 0.414f));
				break;
			case MyCubeTopology.Corner:
			case MyCubeTopology.RotatedCorner:
				verts.Add(new Vector3(1f, 1f, -1f));
				verts.Add(new Vector3(1f, -1f, -1f));
				verts.Add(new Vector3(-1f, -1f, -1f));
				verts.Add(new Vector3(1f, -1f, 1f));
				break;
			case MyCubeTopology.RoundCorner:
				verts.Add(new Vector3(1f, 1f, -1f));
				verts.Add(new Vector3(1f, -1f, -1f));
				verts.Add(new Vector3(-1f, -1f, -1f));
				verts.Add(new Vector3(1f, -1f, 1f));
				verts.Add(new Vector3(-0.414f, 0.414f, -1f));
				verts.Add(new Vector3(-0.414f, -1f, 0.414f));
				verts.Add(new Vector3(1f, 0.414f, 0.414f));
				break;
			case MyCubeTopology.InvCorner:
				verts.Add(new Vector3(1f, 1f, 1f));
				verts.Add(new Vector3(1f, 1f, -1f));
				verts.Add(new Vector3(1f, -1f, 1f));
				verts.Add(new Vector3(-1f, 1f, 1f));
				verts.Add(new Vector3(-1f, 1f, -1f));
				verts.Add(new Vector3(-1f, -1f, 1f));
				verts.Add(new Vector3(-1f, -1f, -1f));
				break;
			case MyCubeTopology.RoundInvCorner:
				verts.Add(new Vector3(1f, 1f, 1f));
				verts.Add(new Vector3(1f, 1f, -1f));
				verts.Add(new Vector3(1f, -1f, 1f));
				verts.Add(new Vector3(-1f, 1f, 1f));
				verts.Add(new Vector3(-1f, 1f, -1f));
				verts.Add(new Vector3(-1f, -1f, 1f));
				verts.Add(new Vector3(-1f, -1f, -1f));
				verts.Add(new Vector3(0.414f, -0.414f, -1f));
				verts.Add(new Vector3(0.414f, -1f, -0.414f));
				verts.Add(new Vector3(1f, -0.414f, -0.414f));
				break;
			case MyCubeTopology.Box:
			case MyCubeTopology.RoundedSlope:
				verts.Add(new Vector3(1f, 1f, 1f));
				verts.Add(new Vector3(1f, 1f, -1f));
				verts.Add(new Vector3(1f, -1f, 1f));
				verts.Add(new Vector3(1f, -1f, -1f));
				verts.Add(new Vector3(-1f, 1f, 1f));
				verts.Add(new Vector3(-1f, 1f, -1f));
				verts.Add(new Vector3(-1f, -1f, 1f));
				verts.Add(new Vector3(-1f, -1f, -1f));
				break;
			case MyCubeTopology.Slope2Base:
				verts.Add(new Vector3(1f, 0f, 1f));
				verts.Add(new Vector3(1f, 1f, -1f));
				verts.Add(new Vector3(1f, -1f, 1f));
				verts.Add(new Vector3(1f, -1f, -1f));
				verts.Add(new Vector3(-1f, 0f, 1f));
				verts.Add(new Vector3(-1f, 1f, -1f));
				verts.Add(new Vector3(-1f, -1f, 1f));
				verts.Add(new Vector3(-1f, -1f, -1f));
				break;
			case MyCubeTopology.Slope2Tip:
				verts.Add(new Vector3(-1f, 0f, -1f));
				verts.Add(new Vector3(1f, 0f, -1f));
				verts.Add(new Vector3(1f, -1f, 1f));
				verts.Add(new Vector3(-1f, -1f, 1f));
				verts.Add(new Vector3(-1f, -1f, -1f));
				verts.Add(new Vector3(1f, -1f, -1f));
				break;
			case MyCubeTopology.Corner2Base:
				verts.Add(new Vector3(-1f, 1f, -1f));
				verts.Add(new Vector3(1f, 0f, -1f));
				verts.Add(new Vector3(1f, -1f, 0f));
				verts.Add(new Vector3(-1f, -1f, 1f));
				verts.Add(new Vector3(-1f, -1f, -1f));
				verts.Add(new Vector3(1f, -1f, -1f));
				break;
			case MyCubeTopology.Corner2Tip:
				verts.Add(new Vector3(1f, 0f, -1f));
				verts.Add(new Vector3(1f, -1f, -1f));
				verts.Add(new Vector3(0f, -1f, -1f));
				verts.Add(new Vector3(1f, -1f, 1f));
				break;
			case MyCubeTopology.InvCorner2Base:
				verts.Add(new Vector3(1f, 1f, 1f));
				verts.Add(new Vector3(1f, 1f, -1f));
				verts.Add(new Vector3(1f, -1f, 1f));
				verts.Add(new Vector3(1f, 0f, -1f));
				verts.Add(new Vector3(0f, -1f, -1f));
				verts.Add(new Vector3(-1f, 1f, 1f));
				verts.Add(new Vector3(-1f, 1f, -1f));
				verts.Add(new Vector3(-1f, -1f, 1f));
				verts.Add(new Vector3(-1f, -1f, -1f));
				break;
			case MyCubeTopology.InvCorner2Tip:
				verts.Add(new Vector3(1f, 1f, 1f));
				verts.Add(new Vector3(1f, 1f, -1f));
				verts.Add(new Vector3(-1f, 1f, 1f));
				verts.Add(new Vector3(-1f, 1f, -1f));
				verts.Add(new Vector3(-1f, -1f, 1f));
				verts.Add(new Vector3(-1f, -1f, -1f));
				verts.Add(new Vector3(1f, 0f, 1f));
				verts.Add(new Vector3(0f, -1f, 1f));
				break;
			case MyCubeTopology.HalfBox:
				verts.Add(new Vector3(1f, 1f, 0f));
				verts.Add(new Vector3(1f, -1f, 0f));
				verts.Add(new Vector3(1f, 1f, -1f));
				verts.Add(new Vector3(1f, -1f, -1f));
				verts.Add(new Vector3(-1f, 1f, 0f));
				verts.Add(new Vector3(-1f, -1f, 0f));
				verts.Add(new Vector3(-1f, 1f, -1f));
				verts.Add(new Vector3(-1f, -1f, -1f));
				break;
			case MyCubeTopology.HalfSlopeBox:
				verts.Add(new Vector3(-1f, 0f, -1f));
				verts.Add(new Vector3(1f, 0f, -1f));
				verts.Add(new Vector3(-1f, -1f, 0f));
				verts.Add(new Vector3(1f, -1f, 0f));
				verts.Add(new Vector3(-1f, -1f, -1f));
				verts.Add(new Vector3(1f, -1f, -1f));
				break;
			}
		}
	}
}
